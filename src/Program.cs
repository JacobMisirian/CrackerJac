using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerJac
{
    class Program
    {
        private static string hashLocation = "";
        private static string dictionaryLocation = "";
        private static bool mybb = false;

        static void Main(string[] args)
        {
            if (args.Length <= 0)
                args = new string[1] { "--help" };

            switch(args[0].ToLower())
            {
                case "-h":
                case "--help":
                    Console.WriteLine("CrackerJac.exe [OPTIONS] [HASH_FILE] [DICTIONARY_FILE]");
                    Console.WriteLine("Options:");
                    Console.WriteLine("-h --help\tDisplays this help and exits.");
                    Console.WriteLine("-m --mybb\tCracks salted MyBB style passwords.");
                    Environment.Exit(0);
                    break;
                case "-m":
                case "-mybb":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Missing [HASH_FILE] or [DICTIONARY_FILE] after option " + args[0]);
                        Environment.Exit(0);
                    }
                    mybb = true;
                    hashLocation = args[1];
                    dictionaryLocation = args[2];
                    break;
                default:
                    if (args[0].StartsWith("-"))
                    {
                        Console.WriteLine("Unknown option: " + args[0]);
                        Environment.Exit(0);
                    }
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Missing [HASH_FILE] or [DICTIONARY_FILE]");
                        Environment.Exit(0);
                    }
                    hashLocation = args[0];
                    dictionaryLocation = args[1];
                    break;
            }

            string[] hashes = File.ReadAllLines(hashLocation);
            foreach (string entry in hashes)
            {
                if (entry.Trim().StartsWith("#"))
                    continue;

                string[] parts = entry.Split(' ');
                if (mybb)
                    new Task(() => userThread(parts[0], parts[1], parts[2])).Start();
                else
                    new Task(() => userThread(parts[0], parts[1])).Start();
            }

            Console.Read();
        }

        private static void userThread(string name, string hash, string salt = "")
        {
            Console.WriteLine("Name: " + name + " Cracked Password: " + new Cracking(hash, dictionaryLocation, salt).Crack());
        }
    }
}
