using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace CrackerJac
{
	public static class Program
	{
		public static string[] Dictionary;
		public static void Main(string[] args)
		{
			if (!File.Exists(args[0]))
			{
				Console.WriteLine("CrackerJac: ERROR dictionary " + args[0] + " could not be loaded as the file does not exist");
				Environment.Exit(-1);
			}
		 	if (!File.Exists(args[1]))
			{
				Console.WriteLine("CrackerJac: ERROR hash file " + args[1] + " could not be loaded as the file does not exist");
				Environment.Exit(-1);
			}	
			Dictionary = File.ReadAllLines(args[0]);
			string[] hashes = File.ReadAllLines(args[1]);
			if (args[2] == "-u" || args[2] == "")
			{
				for (int x = 0; x < hashes.Length; x++)
				{
					Cracking.Unsalted(hashes[x]);
				}
			}
			if (args[2] == "-s")
			{
				for (int x = 0; x < hashes.Length; x++)
				{
					Cracking.Salted(hashes[x]);
				}
			}
		}
	}
}
