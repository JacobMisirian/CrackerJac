using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Security.Cryptography;

namespace CrackerJac
{
	public static class Cracking
	{
		public static bool Cap1 = false;
		public static bool Cap10 = false;
		public static bool Cap100 = false;
		public static bool cap1000 = false;
		public static bool Num1 = false;
		public static bool Num10 = false;
		public static bool Num100 = false;

		public static bool Unsalted(string line)
		{               
				string name = line.Substring(0, line.IndexOf(" "));
				string curHash = line.Substring(line.IndexOf(" ") + 1);

				for (int x = 0; x < Program.Dictionary.Length; x++)
				{
					if (GenHash(Program.Dictionary[x]) == curHash)
					{
						Console.WriteLine("Password found for " + name + ", it is " + Program.Dictionary[x]);
						Supervisor.TermThreads = true;
						return true;
					}
				}
				Thread cap1 = new Thread(() => Advanced.AddCap(curHash, name));
				cap1.Start();

				Thread num10 = new Thread(() => Advanced.NumAppend(curHash, name, 0, 10, false));
				num10.Start();
				
				Thread num10c = new Thread(() => Advanced.NumAppend(curHash, name, 0, 10, true));
				num10c.Start();

				Thread num100 = new Thread(() => Advanced.NumAppend(curHash, name, 10, 100, false));
				num100.Start();

				Thread num100c = new Thread(() => Advanced.NumAppend(curHash, name, 10, 100, true));
				num100c.Start();

				Thread num1000 = new Thread(() => Advanced.NumAppend(curHash, name, 100, 1000, false));
				num1000.Start();

				Thread num1000c = new Thread(() => Advanced.NumAppend(curHash, name, 100, 1000, true));
				num1000c.Start();
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
					Supervisor.TermThreads = true;
					return true;
				}
			}

			Supervisor.Reset();
			Thread supervisor = new Thread(() => Supervisor.Run());
			supervisor.Start();
			
			Thread cap1 = new Thread(() => Advanced.AddCapSalt(hash, name, salt));
			cap1.Start();

			Thread num1 = new Thread(() => Advanced.NumAppendSalt(hash, name, salt, 0, 10, false));
			num1.Start();

			Thread num1c = new Thread(() => Advanced.NumAppendSalt(hash, name, salt, 0, 10, true));
			num1c.Start();

			Thread num10 = new Thread(() => Advanced.NumAppendSalt(hash, name, salt, 10, 100, false));
			num10.Start();

			Thread num10c = new Thread(() => Advanced.NumAppendSalt(hash, name, salt, 10, 100, true));
			num10c.Start();

			Thread num100 = new Thread(() => Advanced.NumAppendSalt(hash, name, salt, 100, 1000, false));
			num100.Start();

			Thread num100c = new Thread(() => Advanced.NumAppendSalt(hash, name, salt, 100, 1000, true));
			num100c.Start();

			return false;
		}

		public static bool Numbers(string line)
		{
		        string name = line.Substring(0, line.IndexOf(" "));
                        string curHash = line.Substring(line.IndexOf(" ") + 1);	

			for (int x = 0; x < 2000000000; x++)
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
			if (text == null)
			{
				return "";
			}
			byte[] encodedText = new UTF8Encoding().GetBytes(text);
			byte[] hash = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedText);
			return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
		}
	}
}	
