using System;
using System.Threading;

namespace CrackerJac
{
    /// <summary>
    /// Supervises the cracking threads
    /// </summary>
	public static class Supervisor
	{
		public static bool TermThreads = false;
        /// <summary>
        /// Runs the supervisor
        /// </summary>
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
        /// <summary>
        /// Reset the supervisor
        /// </summary>
		public static void Reset()
		{
			TermThreads = false;
		}
	}
}