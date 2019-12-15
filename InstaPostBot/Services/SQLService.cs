using InstaPostBot.Other;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstaPostBot.Services
{
	public class SQLService
	{
		public string DBName { get; }
		public string Url { get; }
		public ushort Port { get; }

		private MySqlConnection connection;
		private MySqlContext context;
		private string config;

		private LogService logger;

		public List<Shedule> Shedules => context?.Shedules.ToList();

		public SQLService(string url, string login, string password, string dbName, ushort port = 3306, LogService log = null)
		{
			logger = log ?? new LogService("./db.log");

			try
			{
				DBName = dbName;
				Url = url;
				Port = port;

				config = $"server={url};user id={login};persistsecurityinfo=True;password={password};database={dbName};port={port}";

				connection = new MySqlConnection(config);
				context = new MySqlContext(connection, false);

				context.Database.CreateIfNotExists();
				connection.Open();

				logger.WriteAsync("SqlService", "INFO", $"Creating completed");
			}
			catch (Exception e)
			{
				logger.WriteAsync("SqlService", "ERROR", $"Creating failed\n\t{e.Message}");
			}
		}

		public bool Add(Shedule shedule)
		{
			return AddRange(new[] { shedule });
		}

		public bool AddRange(IEnumerable<Shedule> shedules)
		{
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					context.Shedules.AddRange(shedules);
					context.SaveChanges();

					transaction.Commit();
					logger.WriteAsync("SqlService", "INFO", $"Addition completed\n\t{String.Join("\n\t", shedules.Select(x => x.Id))}");
					return true;
				}
				catch (Exception e)
				{
					transaction.Rollback();
					logger.WriteAsync("SqlService", "ERROR", $"Addition failed\n\t{e.Message}");
					return false;
				}
			}
		}

		public bool Remove(Shedule shedule)
		{
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					context.Shedules.Remove(shedule);
					context.SaveChanges();

					transaction.Commit();
					logger.WriteAsync("SqlService", "INFO", $"Removing <{shedule.Id}> completed");
					return true;
				}
				catch (Exception e)
				{
					transaction.Rollback();
					logger.WriteAsync("SqlService", "ERROR", $"Removing <{shedule.Id}> failed\n\t{e.Message}");
					return false;
				}
			}
		}
	}
}
