using System;
using System.IO;

namespace CrackerJac
{
	public static class PrepareDict
	{
		public static void Run(string path)
		{
			string[] lines = File.ReadAllLines(path);

			for (int x = 0; x < lines.Length; x++)
			{
				lines[x] = lines[x] + " " + Cracking.GenHash(lines[x]);
			}
			Program.Dictionary = lines;
			Console.WriteLine("Dictionary prepared, MD5s calculated");

		}
	}
}
