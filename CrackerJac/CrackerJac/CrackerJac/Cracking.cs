using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CrackerJac
{
    /// <summary>
    /// The class containing methods for hash cracking
    /// </summary>
	public static class Cracking
	{
		public static bool Cap1 = false;
		public static bool Cap10 = false;
		public static bool Cap100 = false;
		public static bool cap1000 = false;
		public static bool Num1 = false;
		public static bool Num10 = false;
		public static bool Num100 = false;
        /// <summary>
        /// Tries to crack the specified unsalted line
        /// </summary>
        /// <param name="line">Line.</param>
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
			Task cap1 = new Task(() => Advanced.AddCap(curHash, name));
			cap1.Start();

			Task num10 = new Task(() => Advanced.NumAppend(curHash, name, 0, 10, false));
			num10.Start();

			Task num10c = new Task(() => Advanced.NumAppend(curHash, name, 0, 10, true));
			num10c.Start();

			Task num100 = new Task(() => Advanced.NumAppend(curHash, name, 10, 100, false));
			num100.Start();

			Task num100c = new Task(() => Advanced.NumAppend(curHash, name, 10, 100, true));
			num100c.Start();

			Task num1000 = new Task(() => Advanced.NumAppend(curHash, name, 100, 1000, false));
			num1000.Start();

			Task num1000c = new Task(() => Advanced.NumAppend(curHash, name, 100, 1000, true));
			num1000c.Start();
			return false;
		}
        /// <summary>
        /// Tries to crack the specified salted hash
        /// </summary>
        /// <param name="line">Line.</param>
		public static bool Salted(string line)
		{
			string name = line.Substring(0, line.IndexOf(" "));
			string curHash = line.Substring(line.IndexOf(" ") + 1);
			string[] saltHash = curHash.Split(':');
			string salt = saltHash[1];
			string hash = saltHash[0];
			for (int x = 0; x < Program.Dictionary.Length; x++)
			{
				if (Cracking.GenSaltedHash(Program.Dictionary[x], salt) == hash)
				{
					Console.WriteLine("Password found for " + name + ", it is " + Program.Dictionary[x]);
					Supervisor.TermThreads = true;
					return true;
				}
			}

			Supervisor.Reset();
			Task supervisor = new Task(() => Supervisor.Run());
			supervisor.Start();

			Task cap1 = new Task(() => Advanced.AddCapSalt(hash, name, salt));
			cap1.Start();

			Task num1 = new Task(() => Advanced.NumAppendSalt(hash, name, salt, 0, 10, false));
			num1.Start();

			Task num1c = new Task(() => Advanced.NumAppendSalt(hash, name, salt, 0, 10, true));
			num1c.Start();

			Task num10 = new Task(() => Advanced.NumAppendSalt(hash, name, salt, 10, 100, false));
			num10.Start();

			Task num10c = new Task(() => Advanced.NumAppendSalt(hash, name, salt, 10, 100, true));
			num10c.Start();

			Task num100 = new Task(() => Advanced.NumAppendSalt(hash, name, salt, 100, 1000, false));
			num100.Start();

			Task num100c = new Task(() => Advanced.NumAppendSalt(hash, name, salt, 100, 1000, true));
			num100c.Start();

			return false;
		}
        /// <summary>
        /// Numbers the specified line.
        /// </summary>
        /// <param name="line">Line.</param>
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
        /// <summary>
        /// Generates the hash.
        /// </summary>
        /// <returns>The hash.</returns>
        /// <param name="text">Text.</param>
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
        /// <summary>
        /// Generates the salted hash
        /// </summary>
        /// <returns>The salted hash.</returns>
        /// <param name="text">Text.</param>
        /// <param name="salt">Salt.</param>
        public static string GenSaltedHash(string text, string salt)
        {
            return GenHash(GenHash(salt) + GenHash(text));
        }
	}
}	