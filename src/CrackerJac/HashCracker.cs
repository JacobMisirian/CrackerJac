using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CrackerJac
{
	public class HashCracker
	{
        private string method;
        public Stats Statistics { get; private set; }

		public HashCracker(string algoString = "MD5")
		{
            method = algoString;
            Statistics = new Stats();
		}

		public void StartDictionaryAttack(CrackerJacConfig config, string[] dictionary, string[] IDs, string[] hashes)
		{
            Statistics.Stopwatch.Reset();
            Statistics.Stopwatch.Start();
			if (IDs.Length != hashes.Length)
				throw new Exception(string.Format("IDs and hashes must be the same length! Got {0} IDs and {1} hashes!", IDs.Length, hashes.Length));
			int len = IDs.Length;
			for (int i = 0; i < len; i++)
			{
                string hash = hashes[i];
                Statistics.HashesProcessed++;
                Statistics.BytesProcessed += (ulong)(IDs[i].Length + hash.Length + 2);
				foreach (string entry in dictionary)
                {
                    if (tryCrack(IDs[i], entry, hash))
                        break;
                    tryAppends(config, IDs[i], entry, hash);
                    if (config.TryCaps)
                    {
                        var sb = new StringBuilder(entry);
                        sb[0] = char.ToUpper(sb[0]);
                        if (tryCrack(IDs[i], sb.ToString(), hash))
                            break;
                        tryAppends(config, IDs[i], sb.ToString(), hash);
                    }
				}
			}
		}

        private bool tryCrack(string id, string entry, string hash)
        {
            if (Hash(entry) == hash)
            {
                Statistics.CrackedHashes++;
                OnHashCracked(new HashCrackedEventArgs { Hash = hash, ID = id, PlainText = entry });
                return true;
            }
            return false;
        }

        private bool tryAppends(CrackerJacConfig config, string id, string entry, string hash)
        {
            foreach (string append in config.TryAppends)
                if (tryCrack(id, new StringBuilder(entry).Append(append).ToString(), hash))
                    return true;
            return false;
        }

        public void StartBruteForceAttack(string letters, int maxLength, string id, string hash)
        {
            char firstLetter = letters.First();
            char lastLetter = letters.Last();

            for (int length = 1; length < maxLength; ++length)
            {
                StringBuilder accum = new StringBuilder(new String(firstLetter, length));
                while (true)
                {
                    if (accum.ToString().All(val => val == lastLetter))
                        break;
                    for (int i = length - 1; i >= 0; --i)
                        if (accum[i] != lastLetter)
                        {
                            accum[i] = letters[letters.IndexOf(accum[i]) + 1];
                            break;
                        }
                        else
                            accum[i] = firstLetter;
                    Statistics.HashesProcessed++;
                    if (Hash(accum.ToString()) == hash)
                    {
                        Statistics.CrackedHashes++;
                        OnHashCracked(new HashCrackedEventArgs { Hash = hash, ID = id, PlainText = accum.ToString() });
                    }
                }
            }
        }

		public event EventHandler<HashCrackedEventArgs> HashCracked;
		protected virtual void OnHashCracked(HashCrackedEventArgs e)
		{
			var handler = HashCracked;
			if (handler != null)
				handler(this, e);
		}

		public string Hash(string text)
		{
			if (text == null)
				return string.Empty;
			return BitConverter.ToString(((HashAlgorithm)CryptoConfig.CreateFromName(method)).ComputeHash(new UTF8Encoding().GetBytes(text))).Replace("-", string.Empty).ToLower();
		}

        public void DisplayStats()
        {
            Console.WriteLine("Time Elapsed: {0}", Statistics.Stopwatch.Elapsed);
            Console.WriteLine("Processed {0} bytes", Statistics.BytesProcessed);
            Console.WriteLine("Processed {0} hashes.", Statistics.HashesProcessed);
            Console.WriteLine("Cracked {0} hashes.", Statistics.CrackedHashes);
        }

        public class Stats
        {
            public ulong BytesProcessed { get; set; }
            public ulong CrackedHashes { get; set; }
            public ulong HashesProcessed { get; set; }
            public Stopwatch Stopwatch { get; private set; }

            public Stats()
            {
                BytesProcessed = 0;
                CrackedHashes = 0;
                HashesProcessed = 0;
                Stopwatch = new Stopwatch();
            }
        }
	}
}