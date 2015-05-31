using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace CrackerJac
{
	public static class Cracking
	{

		public static bool Unsalted(string line)
		{               
				string name = line.Substring(0, line.IndexOf(" "));
				string curHash = line.Substring(line.IndexOf(" ") + 1);

				for (int x = 0; x < Program.Dictionary.Length; x++)
				{
					if (GenHash(Program.Dictionary[x]) == curHash)
					{
						Console.WriteLine("Password found for " + name + ", it is " + Program.Dictionary[x]);
						return true;
					}
				}
				Console.WriteLine("\tNot found in base words, making the first letter capital");
				if (Advanced.AddCap(curHash, name))
				{
					return true;
				}

				Console.WriteLine("\tNot found in capital words, appending 0-10");
				if (Advanced.NumAppend(curHash, name, 0, 10))
				{
					return true;
				}
				Console.WriteLine("\t\tTrying capital");
				if (Advanced.NumAppend(curHash, name, 0, 10, true))
				{
					return true;
				}

				Console.WriteLine("\tNot found in append 0-10, appending 10-100");
				if (Advanced.NumAppend(curHash, name, 10, 100))
				{
					return true;
				}
				Console.WriteLine("\t\tTrying capital");
				if (Advanced.NumAppend(curHash, name, 10, 100, true))
				{
					return true;
				}

				Console.WriteLine("\tNot found in append 10-100, appending 100-1000");
				if (Advanced.NumAppend(curHash, name, 100, 1000))
				{
					return true;
				}
				Console.WriteLine("\t\tTrying capital");
				if (Advanced.NumAppend(curHash, name, 100, 1000, true))
				{
					return true;
				}

				return false;
		}
	
		public static bool Salted(string line)
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
					return true;
				}
			}

			Console.WriteLine("\tNot found in base words, making first letter capital");
			if (Advanced.AddCapSalt(hash, name, salt))
			{
				return true;
			}

			Console.WriteLine("\tNot found in capital words, appending 0-10");
			if (Advanced.NumAppendSalt(hash, name, salt, 0, 10))
			{
				return true;
			}
			Console.WriteLine("\t\tTrying capital");
			if (Advanced.NumAppendSalt(hash, name, salt, 0, 10, true))
			{
				return true;
			}

			Console.WriteLine("\tNot found in append 1-10, appending 10-100");
			if (Advanced.NumAppendSalt(hash, name, salt, 10, 100))
			{
				return true;
			}
			Console.WriteLine("\t\tTrying capital");
			if (Advanced.NumAppendSalt(hash, name, salt, 10, 100, true))
			{
				return true;
			}

			Console.WriteLine("\tNot found in append 10-100, appending 100-1000");
			if (Advanced.NumAppendSalt(hash, name, salt, 100, 1000))
			{
				return true;
			}
			Console.WriteLine("\t\tTrying capital");
			if (Advanced.NumAppendSalt(hash, name, salt, 100, 1000, true))
			{
				return true;
			}

			return false;
		}

		public static bool Numbers(string line)
		{
		        string name = line.Substring(0, line.IndexOf(" "));
                        string curHash = line.Substring(line.IndexOf(" ") + 1);	

			for (int x = 0; x < 1000000; x++)
			{
				if (GenHash(x.ToString()) == curHash)
				{
					Console.WriteLine("Password found for " + name + ", it is " + x);
					return true;
				}
			}
			return false;
		}
		public static string GenHash(string text)
		{
			byte[] encodedText = new UTF8Encoding().GetBytes(text);
			byte[] hash = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedText);
			return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
		}
	}
}	
