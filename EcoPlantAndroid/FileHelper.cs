using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoPlantAndroid
{
	internal static class FileHelper
	{
		public static void WriteAllLines(string path,string[] lines)
		{
			var backingFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);
			using (var writer = File.CreateText(backingFile))
			{
				foreach (string line in lines)
				{
					writer.WriteLine(line);
				}
			}
		}

		public static string[] ReadAllLines(string path)
		{
			var backingFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);
			return File.ReadAllLines(backingFile);
		}

		public static bool Exists(string path)
		{
			var backingFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);
			return File.Exists(backingFile);
		}

		public static void Delete(string path)
		{
			var backingFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);
			File.Delete(backingFile);
		}
	}
}
