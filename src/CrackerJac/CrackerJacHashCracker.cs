using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CrackerJac
{
    public class CrackerJacHashCracker
    {
        public CrackerJacConfig Config { get; private set; }

        public CrackerJacHashCracker(CrackerJacConfig config)
        {
            Config = config;
            threads = new Thread[config.ThreadCount];
        }

        public event EventHandler<HashCrackedEventArgs> HashCracked;
        protected virtual void OnHashCracked(HashCrackedEventArgs e)
        {
            var handler = HashCracked;
            if (handler != null)
                handler(this, e);
        }

        public void StartAttack()
        {
            threads = new Thread[Config.ThreadCount];
            if (Config.IsBruteForce)
                startBruteForceAttack();
            else
                startDictionaryAttack();
        }

        private Thread[] threads;
        private const int BLOCK_SIZE = 512;

        private void startDictionaryAttack()
        {
            StreamReader hashFile = new StreamReader(Config.HashFile);
            string[] dictionary = File.ReadAllLines(Config.DictionaryFile);
            while (hashFile.BaseStream.Position < hashFile.BaseStream.Length)
            {
                for (int i = 0; i < Config.ThreadCount && hashFile.BaseStream.Position < hashFile.BaseStream.Length; i++)
                {
                    string[] names = new string[BLOCK_SIZE];
                    string[] hashes = new string[BLOCK_SIZE];
                    for (int j = 0; j < BLOCK_SIZE && hashFile.BaseStream.Position < hashFile.BaseStream.Length; j++)
                    {
                        string[] parts = hashFile.ReadLine().Split(' ');

                        if (parts.Length >= 2)
                        {
                            names[j] = parts[0];
                            hashes[j] = parts[1];
                        }
                    }
                    threads[i] = new Thread(() => breakHashes(dictionary, names, hashes));
                    threads[i].Start();
                }
                foreach (var thread in threads)
                    while (thread.IsAlive)
                        Thread.Sleep(5);
            }
        }

        private void breakHashes(string[] dictionary, string[] names, string[] hashes)
        {
            for (int i = 0; i < names.Length; i++)
            {
                string hash = hashes[i];
                if (hash == null || hash == string.Empty)
                    continue;
                string name = names[i];
                for (int j = 0; j < dictionary.Length; j++)
                {
                    string[] parts = dictionary[j].Split(' ');
                    string entry = parts[0];
                    string entryHash = parts[1];
                    if (checkHash(name, entry, entryHash, hash))
                        break;
                    if (checkHashWithAppends(name, entry, hash))
                        break;
                    if (Config.TryCaps)
                    {
                        StringBuilder sb = new StringBuilder(entry);
                        sb[0] = char.ToUpper(sb[0]);
                        string capsEntry = sb.ToString();
                        sb = null;
                        if (checkHash(name, capsEntry, entryHash, hash))
                            break;
                        if (checkHashWithAppends(name, capsEntry, hash))
                            break;
                    }
                }
            }
        }

        private void startBruteForceAttack()
        {

        }

        private bool checkHash(string name, string entry, string entryHash, string hash)
        {
            //Console.WriteLine("{0}\t{1}", Hash(entry), hash);
            if (entryHash == hash)
            {
                OnHashCracked(new HashCrackedEventArgs { Hash = hash, Name = name, PlainText = entry });
                return true;
            }
            return false;
        }

        private bool checkHashWithAppends(string name, string entry, string hash)
        {
            foreach (string append in Config.TryAppends)
                if (checkHash(name, entry + append, "", hash))
                    return true;
            return false;
        }

        public string Hash(string text)
        {
            return BitConverter.ToString(((HashAlgorithm)CryptoConfig.CreateFromName(Config.Method)).ComputeHash(new UTF8Encoding().GetBytes(text))).Replace("-", string.Empty).ToLower();
        }
    }
}
