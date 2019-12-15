using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;

namespace InstaPostBot.Other
{
	public class Shedule
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key, Column(Order = 0)]
		public int Id { get; set; }
		[Column(Order = 1)]
		public int Type { get; set; }
		[Column(Order = 2)]
		public string Path { get; set; }
		[Column(Order = 3)]
		public string Mask { get; set; }

		public Shedule() { }

		public Shedule(string path, string mask)
		{
			Type = GetType(mask);
			Path = path;
			Mask = mask;
		}

		public override string ToString()
		{
			return $"{Id}: M-{Mask} P-{Path}";
		}

		public static bool CheckMask(string mask)
		{
			if (GetType(mask) > 0)
				return true;
			return false;
		}

		private static int GetType(string mask)
		{
			var type1 = new Regex("^[0-9]{1,6}[m, h, d]{1}$");

			if (type1.IsMatch(mask) && int.Parse(String.Join("", mask.Take(mask.Length - 1))) > 0)
				return 1;

			DateTime time;

			if (DateTime.TryParse(mask, out time) && time > DateTime.Now)
				return 2;

			return 0;
		}
	}
}
