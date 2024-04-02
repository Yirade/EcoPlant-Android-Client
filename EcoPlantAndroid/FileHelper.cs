using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoPlantAndroid
{
	internal static class FileHelper
	{
		public static async Task WriteAllLines(string path,string[] lines)
		{
			var backingFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);
			using (var writer = File.CreateText(backingFile))
			{
				foreach (string line in lines)
				{
					await writer.WriteLineAsync(line);
				}
			}
		}

		public static async Task<string[]> ReadAllLines(string path)
		{
			var backingFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);
			return await File.ReadAllLinesAsync(backingFile);
		}
	}
}
