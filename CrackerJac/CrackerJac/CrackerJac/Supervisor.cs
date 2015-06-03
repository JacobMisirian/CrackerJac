using System;
using System.Threading;

namespace CrackerJac
{
	public static class Supervisor
	{
		public static bool TermThreads = false;

		public static void Run()
		{
			bool inLoop = false;
			while(!inLoop)
			{
				if (TermThreads)
				{
					TermThreads = true;
					inLoop = true;
				}
			}
		}
		public static void Reset()
		{
			TermThreads = false;
		}
	}
}