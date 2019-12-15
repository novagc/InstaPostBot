using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InstaPostBot.Services
{
	public class LogService
	{
		public string Path { get; }

		private FileStream logFileStream;
		private StreamWriter logWriter;

		public LogService(string path)
		{
			Path = path;

			logFileStream = new FileStream(path, FileMode.OpenOrCreate);
			logWriter = new StreamWriter(logFileStream, Encoding.UTF8);

			new Timer(SaveLog, null, 5000, 5000);
		}

		public Task WriteAsync(string senderName, string type, string text)
		{
			return Task.Factory.StartNew(() =>
				{
					lock (logWriter)
					{
						logWriter.WriteLine(
							$"[{DateTime.Now:yyyy.MM.dd HH:mm:ss}] [{senderName}] [{type}]: {text}\n");
					}
				}
			);
		}

		public void SaveLog(object sender)
		{
			lock (logWriter)
			{
				logWriter.Close();
				logWriter.Dispose();

				logFileStream.Close();
				logFileStream.Dispose();

				logFileStream = new FileStream(Path, FileMode.OpenOrCreate);
				logWriter = new StreamWriter(logFileStream, Encoding.UTF8);
			}
		}
	}
}
