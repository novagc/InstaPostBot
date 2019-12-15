using InstaPostBot.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace InstaPostBot.Services
{
	public class FileSystemService
	{
		public FileSystemService() { }

		public PathType GetType(string path) =>
			File.Exists(path) ? PathType.File : Directory.Exists(path) ? PathType.Directory : PathType.Incorrect;

		public string GetName(string path) => path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries).Last();

		public bool Delete(string path)
		{
			var type = GetType(path);

			if (type == PathType.Incorrect)
			{
				return true;
			}

			if (type == PathType.File)
			{
				try
				{
					File.Delete(path);
					return true;
				}
				catch
				{
					return false;
				}
			}

			try
			{
				Directory.GetFiles(path).AsParallel().ForAll(File.Delete);
				Directory.Delete(path);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public void MassDelete(IEnumerable<string> pathes)
		{
			pathes.AsParallel().ForAll(x => Delete(x));
		}

		public bool Copy(string oldPath, string newPath, bool withContent = true)
		{
			var type = GetType(oldPath);

			if (type == PathType.Incorrect)
			{
				return false;
			}

			if (type == PathType.File)
			{
				try
				{
					File.Copy(oldPath, $"{newPath}/{GetName(oldPath)}");
					return true;
				}
				catch
				{
					return false;
				}
			}

			try
			{
				if (!Directory.Exists(newPath))
				{
					Directory.CreateDirectory(newPath);
				}

				if (withContent)
				{
					Directory.GetFiles(oldPath)
						.AsParallel()
						.Where(IsImage)
						.ForAll(file => File.Copy(file, $"{newPath}/{GetName(file)}"));
				}

				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool Move(string oldPath, string newPath, bool withContent = true, bool copy = false)
		{
			var type = GetType(oldPath);

			if (type == PathType.Incorrect)
			{
				return false;
			}

			if (type == PathType.File)
			{
				try
				{

					File.Move(oldPath, newPath);
					return true;
				}
				catch
				{
					return false;
				}
			}

			try
			{
				if (!Directory.Exists(newPath))
				{
					Directory.CreateDirectory(newPath);
				}

				if (withContent)
				{
					Directory.GetFiles(oldPath)
						.AsParallel()
						.ForAll(file => File.Move(file, $"{newPath}/{GetName(file)}"));
				}

				return Delete(oldPath);
			}
			catch
			{
				return false;
			}
		}

		public string[] GetFiles(string path, bool usePattern = false, string pattern = "*")
		{
			var type = GetType(path);

			if (type != PathType.Directory)
				return new string[] { };

			try
			{
				return usePattern ? Directory.GetFiles(path, pattern) : Directory.GetFiles(path);
			}
			catch
			{
				return new string[] { };
			}
		}

		public bool IsImage(string path)
		{
			var reg = new Regex(@"\w*\.(jpg|png)");
			return reg.IsMatch(GetName(path));
		}

		public bool ValidPath(string path)
		{
			var type = GetType(path);
			switch (type)
			{
				case PathType.Incorrect:
					return false;
				case PathType.Directory:
					return true;
				default:
					return IsImage(path);
			}
		}

		public FileStream GetFileStream(string path, bool clear = false)
		{
			return Directory.Exists(path) ? null : new FileStream(path, clear ? FileMode.Create : FileMode.OpenOrCreate);
		}
	}
}
