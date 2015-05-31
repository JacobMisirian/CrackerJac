using System;

namespace CrackerJac
{
	public static class Advanced
	{
		public static bool NumAppend(string curHash, string name, int lower, int upper)
		{
                	for (int x = 0; x < Program.Dictionary.Length; x++)
                        {
				for (int y = lower; y < upper; y++)
				{
                        		if (Cracking.GenHash(Program.Dictionary[x] + y.ToString()) == curHash)
                                	{
                                		Console.WriteLine("Password found for " + name + ", it is " + Program.Dictionary[x] + y.ToString());
                                       		return true;
                                	}
                        	}
			}
			return false;
		}
		public static bool NumAppendSalt(string hash, string name, string salt, int lower, int upper)
		{
			for (int x = 0; x < Program.Dictionary.Length; x++)
                        {
				for (int y = lower; y < upper; y++)
				{
                                	if (Salting.Run(Program.Dictionary[x] + y.ToString(), salt) == hash)
                                	{
                                        	Console.WriteLine("Password found for " + name + ", it is " + Program.Dictionary[x] + y.ToString());
                                        	return true;;
                                	}
                        	}
			}
			return false;
		}
	}
}
