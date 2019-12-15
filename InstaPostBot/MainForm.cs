using InstaPostBot.Other;
using InstaPostBot.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace InstaPostBot
{
	public partial class MainForm : Form
	{
		public static SQLService MySQL;
		public static LogService Logger;
		public static InstagramService Instagram;
		public static FileSystemService FS;

		private static Dictionary<int, Timer> tasks = new Dictionary<int, Timer>();
		private static MainForm Instance;

		public MainForm()
		{
			InitializeComponent();
			Instance = this;
		}

		public static bool InitLogger()
		{
			try
			{
				Logger = new LogService($"{Directory.GetCurrentDirectory()}/log.txt");

				AppDomain.CurrentDomain.UnhandledException += (sender, exArgs) =>
				{
					Logger.WriteAsync(sender.ToString(), "ERROR", $"Unhandled Exception\n\t{((Exception)exArgs.ExceptionObject).Message}");
					if (exArgs.IsTerminating)
					{
						Logger.WriteAsync("InstaPostBot", "INFO", "Emergency shutdown");
					}
				};
				Logger.WriteAsync("InstaPostBot", "INFO", $"Logger initialized");
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				return false;
			}
		}

		public static bool InitDB(string dbUrl, string dbPort, string dbLogin, string dbPassword, string dbName)
		{
			Logger.WriteAsync("InstaPostBot", "INFO", "MySqlService initialization start");
			try
			{
				MySQL = new SQLService(dbUrl, dbLogin, dbPassword, dbName, ushort.Parse(dbPort));
				Logger.WriteAsync("InstaPostBot", "INFO", "MySqlService initialization end");
				return true;
			}
			catch (Exception e)
			{
				Logger.WriteAsync("InstaPostBot", "INFO", $"MySqlService initialization failed\n\t{e.Message}");
				return false;
			}
		}

		public static void InitInstagram(string login, string password, string phone)
		{
			Logger.WriteAsync("InstaPostBot", "INFO", $"InstagramService async initialization start");
			Instagram = new InstagramService(login, password, phone);
			Instagram.MediaUploaded += FS.MassDelete;
			Instagram.Login();
		}

		public static void InitFS()
		{
			Logger.WriteAsync("InstaPostBot", "INFO", $"FileSystemService initialization start");
			FS = new FileSystemService();
			Logger.WriteAsync("InstaPostBot", "INFO", $"FileSystemService initialization end");
		}

		private static void Start()
		{
			Logger.WriteAsync("InstaPostBot", "INFO", $"Posting started");
			var shedules = MySQL.Shedules;
			for (int i = 0; i < shedules.Count; i++)
			{
				var temp = shedules[i];
				tasks.Add(temp.Id,
					temp.Type == 1
						? new Timer(ForTypeOne, temp, 0, ParseMask(temp))
						: new Timer(ForTypeTwo, temp, 0, 60000));
			}
		}

		private static void ForTypeOne(object shedule)
		{
			var temp = (Shedule)shedule;
			if (Directory.GetFiles($"{Directory.GetCurrentDirectory()}/temp/{temp.Id}").Length == 0)
			{
				Delete(temp);
				return;
			}
			var files = Directory.GetFiles($"{Directory.GetCurrentDirectory()}/temp/{temp.Id}");
			var file = files[new Random().Next(0, files.Length - 1)];

			Instagram.UploadImageAsync(file);
		}

		private static void ForTypeTwo(object shedule)
		{
			if (DateTime.Now >= DateTime.Parse(((Shedule)shedule).Mask))
			{
				var path = $"{Directory.GetCurrentDirectory()}/temp/{((Shedule)shedule).Id}/{FS.GetName(((Shedule)shedule).Path)}";
				if (FS.IsImage(path))
					Instagram.UploadImageAsync(path);
				else
					Instagram.UploadAlbumAsync(Directory.GetFiles(path));
				Delete((Shedule)shedule);
			}
		}

		private static long ParseMask(Shedule shedule)
		{
			return Int64.Parse(shedule.Mask.Substring(0, shedule.Mask.Length - 1)) *
				   (shedule.Mask.Last() == 'm' ? 60000 : shedule.Mask.Last() == 'h' ? 3600000 : 86400000);
		}

		public static void Add(Shedule shedule)
		{
			MySQL.Add(shedule);
			Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/temp/{shedule.Id}");
			FS.Copy(shedule.Path, $"{Directory.GetCurrentDirectory()}/temp/{shedule.Id}");
			tasks.Add(shedule.Id,
				shedule.Type == 1
					? new Timer(ForTypeOne, shedule, 5000, ParseMask(shedule))
					: new Timer(ForTypeTwo, shedule, 5000, 60000));
			Instance.UpdateDGV();
			Logger.WriteAsync("InstaPostBot", "INFO", $"Added new shedule\n\t{shedule.Id}-{shedule.Mask}-{shedule.Path}-{shedule.Type}");
		}

		public static void Delete(Shedule shedule)
		{
			FS.Delete($"{Directory.GetCurrentDirectory()}/temp/{shedule.Id}");
			tasks[shedule.Id].Dispose();
			tasks.Remove(shedule.Id);
			MySQL.Remove(shedule);
			Instance.Invoke(new Action(Instance.UpdateDGV));
			Logger.WriteAsync("InstaPostBot", "INFO", $"Deleted shedule\n\t{shedule.Id}-{shedule.Mask}-{shedule.Path}-{shedule.Type}");
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			new AddForm().ShowDialog();
		}

		private void UpdateDGV()
		{
			dataGridView1.Rows.Clear();
			foreach (var shedule in MySQL.Shedules)
			{
				dataGridView1.Rows.Add(shedule.Id, shedule.Mask, shedule.Path, $"{Directory.GetCurrentDirectory().Replace('\\','/')}/temp/{shedule.Id}");
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			new SettingsForm().ShowDialog();
			Start();
			UpdateDGV();
			Logger.WriteAsync("InstaPostBot", "INFO", $"Main form loaded");
		}
	}
}
