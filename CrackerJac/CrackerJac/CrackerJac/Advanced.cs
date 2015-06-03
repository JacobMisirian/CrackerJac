using System;

namespace CrackerJac
{
	public static class Advanced
	{
        /// <summary>
        /// Appends a number to the dictionary entries for unsalted hashes
        /// </summary>
        /// <returns><c>true</c>, if append was numbered, <c>false</c> otherwise.</returns>
        /// <param name="curHash">Current hash.</param>
        /// <param name="name">Name.</param>
        /// <param name="lower">Lower.</param>
        /// <param name="upper">Upper.</param>
        /// <param name="FirstCharUp">If set to <c>true</c> first char up.</param>
		public static bool NumAppend(string curHash, string name, int lower, int upper, bool FirstCharUp)
		{
			for (int x = 0; x < Program.Dictionary.Length; x++)
			{
				for (int y = lower; y < upper; y++)
				{
					if (Supervisor.TermThreads)
					{
						return false;
					}
					if (Cracking.GenHash(ShouldAddCap(Program.Dictionary[x], FirstCharUp) + y.ToString()) == curHash)
					{
						Console.WriteLine("Password found for " + name + ", it is " + ShouldAddCap(Program.Dictionary[x], FirstCharUp) + y.ToString());
						Supervisor.TermThreads = true;
						return true;
					}
				}
			}
			return false;
		}
        /// <summary>
        /// Appends a number to the dictionary entries for salted hashes
        /// </summary>
        /// <returns><c>true</c>, if append salt was numbered, <c>false</c> otherwise.</returns>
        /// <param name="hash">Hash.</param>
        /// <param name="name">Name.</param>
        /// <param name="salt">Salt.</param>
        /// <param name="lower">Lower.</param>
        /// <param name="upper">Upper.</param>
        /// <param name="FirstCharUp">If set to <c>true</c> first char up.</param>
		public static bool NumAppendSalt(string hash, string name, string salt, int lower, int upper, bool FirstCharUp = false)
		{
			for (int x = 0; x < Program.Dictionary.Length; x++)
			{
				for (int y = lower; y < upper; y++)
				{
					if (Supervisor.TermThreads)
					{
						return false;
					}
					if (Cracking.GenSaltedHash(ShouldAddCap(Program.Dictionary[x], FirstCharUp) + y.ToString(), salt) == hash)
					{
						Console.WriteLine("Password found for " + name + ", it is " + ShouldAddCap(Program.Dictionary[x], FirstCharUp) + y.ToString());
						Supervisor.TermThreads = true;
						return true;
					}
				}
			}
			return false;
		}
        /// <summary>
        /// Adds a capital to the dictionary entries for salted hashes
        /// </summary>
        /// <returns><c>true</c>, if cap salt was added, <c>false</c> otherwise.</returns>
        /// <param name="hash">Hash.</param>
        /// <param name="name">Name.</param>
        /// <param name="salt">Salt.</param>
		public static bool AddCapSalt(string hash, string name, string salt)
		{
			for (int x = 0; x < Program.Dictionary.Length; x++)
			{
				if (Supervisor.TermThreads)
				{
					return false;
				}
				if (Cracking.GenSaltedHash(ShouldAddCap(Program.Dictionary[x], true), salt) == hash)
				{
					Console.WriteLine("Password found for " + name + ", it is " + ShouldAddCap(Program.Dictionary[x], true));
					Supervisor.TermThreads = true;
					return true;
				}
			}
			return false;
		}
        /// <summary>
        /// Adds a capital to the dictionary entries for unsalted hashes
        /// </summary>
        /// <param name="curHash">Current hash.</param>
        /// <param name="name">Name.</param>
		public static void AddCap(string curHash, string name)
		{
			for (int x = 0; x < Program.Dictionary.Length; x++)
			{
				if (Supervisor.TermThreads)
				{
					return;
				}
				if (Cracking.GenHash(ShouldAddCap(Program.Dictionary[x], true)) == curHash)
				{
					Console.WriteLine("Password found for " + name + ", it is " + ShouldAddCap(Program.Dictionary[x], true));
					Supervisor.TermThreads = true;
					return;
				}
			}
			return;
		}
        /// <summary>
        /// Decides if we should add a capital letter
        /// </summary>
        /// <returns>The add cap.</returns>
        /// <param name="text">Text.</param>
        /// <param name="FirstCharUp">If set to <c>true</c> first char up.</param>
		public static string ShouldAddCap(string text, bool FirstCharUp)
		{
			if (text == null)
			{
				return "";
			}
			if (FirstCharUp)
			{
				return text[0].ToString().ToUpper() + text.Substring(1);
			}
			else
			{
				return text;
			}
		}
	}
}