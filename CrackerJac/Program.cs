using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace CrackerJac
{
	public static class Program
	{
		public static string[] Dictionary = new string[1000];
		public static void Main(string[] args)
		{
			if (args[0] == "-h")
			{
				Console.WriteLine("Usage: CrackerJac [DICTIONARY]... [HASHES]... [OPTIONS]...");
				Console.WriteLine("Dictionary based Unsalted and MyBB Salted MD5 hash cracker.");
				Console.WriteLine("OPTIONS:");
				Console.WriteLine("-h\tDisplays this help and exits");
				Console.WriteLine("-s\tCrackes salted MyBB passwords");
				Console.WriteLine("-u\tCrackes unsalted (regular) passwords");
				Console.WriteLine("-n\tCrackes numeric passwords");
				Console.WriteLine("FILES:");
				Console.WriteLine("[DICTIONARY] represents a plain text dictionary file.");
				Console.WriteLine("[HASHES] represents a plain text file containing names and hashes");
				Environment.Exit(0);
			}
 
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
			string[] hashes = File.ReadAllLines(args[1]);
			using (var fdict = File.OpenRead(args[0]))
			using (var reader = new StreamReader(fdict))
			{
				for (int x = 0; x < hashes.Length; x++)
				{
					while (reader.BaseStream.Position < reader.BaseStream.Length)
					{
						for (int y = 0; y < 1000; y++)
						{
							Dictionary[y] = reader.ReadLine();

							if (Dictionary[y] != null)
							{
								Dictionary[y].Replace("\r", "");
							}
						}
						if (args[2] == "-u")
						{
							if (Cracking.Unsalted(hashes[x]))
							{
								goto nextHash;
							}
						}
						if (args[2] == "-s")
						{
							if (Cracking.Salted(hashes[x]))
							{
								goto nextHash;
							}
						}
						if (args[2] == "-n")
						{
							if (Cracking.Numbers(hashes[x]))
							{
								goto nextHash;
							}
						}
					}
					nextHash:
						Console.Write("");
				}
			}
		}
	}
}
