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
                    Console.WriteLine("-gu --generate-unsalted [STRING]\tGenerates an unsalted hash from the next string.");
                    Console.WriteLine("-gs --generate-salted [STRING] [SALT]\tGenerates a salted hash from the next two strings.");
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
                case "-gu":
                case "--generate-unsalted":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("String must follow " + args[0]);
                        Environment.Exit(0);
                    }
                    Console.WriteLine(Cracking.Md5(args[1]));
                    Environment.Exit(0);
                    break;
                case "-gs":
                case "--generate-salted":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("String and Salt must follow " + args[0]);
                        Environment.Exit(0);
                    }
                    Console.WriteLine(Cracking.Md5(Cracking.Md5(args[2]) + Cracking.Md5(args[1])));
                    Environment.Exit(0);
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
                if (entry.Trim().StartsWith("#") || entry == "" || entry == "\n")
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
