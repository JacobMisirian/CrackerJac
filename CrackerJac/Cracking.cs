using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace CrackerJac
{
	public static class Cracking
	{

		public static void Unsalted(string line)
		{               
				string name = line.Substring(0, line.IndexOf(" "));
				string curHash = line.Substring(line.IndexOf(" ") + 1);
				for (int x = 0; x < Program.Dictionary.Length; x++)
				{
					if (GenHash(Program.Dictionary[x]) == curHash)
					{
						Console.WriteLine("Password found for " + name + ", it is " + Program.Dictionary[x]);
						return;
					}
				}
				Console.WriteLine("The password for " + name + " was not found in the dictionary");
		}
	
		public static void Salted(string line)
		{
			string name = line.Substring(0, line.IndexOf(" "));
                        string curHash = line.Substring(line.IndexOf(" ") + 1);
			string[] saltHash = curHash.Split(':');
			string salt = saltHash[1];
			string hash = saltHash[0];
			for (int x = 0; x < Program.Dictionary.Length; x++)
			{
				if (Salting.Run(Program.Dictionary[x], salt) == hash)
				{
					Console.WriteLine("Password found for " + name + ", it is " + Program.Dictionary[x]);
					return;
				}
			}
			Console.WriteLine("The password for " + name + " was not found in the dictionary");
		}
		public static string GenHash(string text)
		{
			byte[] encodedText = new UTF8Encoding().GetBytes(text);
			byte[] hash = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedText);
			return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
		}
	}
}	
