using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CrackerJac
{
	class MainClass
	{
		public const int BUFFER_LENGTH = 100;
		public static void Main(string[] args)
		{
			var config = new CrackerJacArgumentParser(args).Parse();
			HashCracker cracker = new HashCracker(config.Method);
			cracker.HashCracked += cracker_hashCracked;
			Thread[] threads = new Thread[config.ThreadCount];
			StreamReader reader = new StreamReader(config.HashFile);

			while (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				for (int i = 0; i < config.ThreadCount; i++)
				{
					var IDs = new List<string>();
					var hashes = new List<string>();
					for (int j = 0; j < BUFFER_LENGTH && reader.BaseStream.Position < reader.BaseStream.Length; j++)
					{
						string[] parts = reader.ReadLine().Split(' ');
						IDs.Add(parts[0]);
						hashes.Add(parts[1]);
					}
					threads[i] = new Thread(() => cracker.StartDictionaryAttack(File.Open(config.DictionaryFile, FileMode.Open, FileAccess.Read, FileShare.Read), IDs.ToArray(), hashes.ToArray()));
					threads[i].Start();
				}
			checkThreads:
				Thread.Sleep(10);
				foreach (var thread in threads)
					if (thread.IsAlive)
						goto checkThreads;
			}
		}

		private static void cracker_hashCracked(object sender, HashCrackedEventArgs e)
		{
			Console.WriteLine("{0} {1}", e.ID, e.PlainText);
		}
	}
}
