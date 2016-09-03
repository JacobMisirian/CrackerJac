using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CrackerJac
{
	class MainClass
	{
		public const int BUFFER_LENGTH = 500;
        private static TextWriter output;

		public static void Main(string[] args)
		{
			var config = new CrackerJacArgumentParser(args).Parse();
            HashCracker cracker = new HashCracker(config.Method);
            cracker.HashCracked += cracker_hashCracked;
            new Thread(() => inputThread(cracker)).Start();
			StreamReader reader = new StreamReader(config.HashFile);
            output = config.OutputFile == string.Empty ? Console.Out : new StreamWriter(config.OutputFile);
            Thread[] threads = new Thread[config.ThreadCount];
			
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
            reader.Close();
            output.Close();
		}

        private static void inputThread(HashCracker cracker)
        {
            while (true)
            {
                switch ((char)Console.Read())
                {
                    case 'p':
                        Console.WriteLine("Time Elapsed: {0}", cracker.Statistics.Stopwatch.Elapsed);
                        Console.WriteLine("Processed {0} hashes.", cracker.Statistics.HashesProcessed);
                        Console.WriteLine("Cracked {0} hashes.", cracker.Statistics.CrackedHashes);
                        break;
                }
            }
        }

		private static void cracker_hashCracked(object sender, HashCrackedEventArgs e)
		{
			output.WriteLine("{0} {1}", e.ID, e.PlainText);
            output.Flush();
		}
	}
}
