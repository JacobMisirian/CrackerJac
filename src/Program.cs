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
        private static bool brute = false;
        private static int bruteForceLength = 0;

        static void Main(string[] args)
        {
            try
            {
                if (args.Length <= 0)
                    args = new string[1] { "--help" };

                switch (args[0].ToLower())
                {
                    case "-h":
                    case "--help":
                        Console.WriteLine("CrackerJac.exe [OPTIONS] [HASH_FILE] [DICTIONARY_FILE]");
                        Console.WriteLine("Options:");
                        Console.WriteLine("-b --brute-force [(-m)] [LENGTH] [HASH_FILE]\tAttempts a brute force attack optionally with the MyBB option AFTER -b");
                        Console.WriteLine("-gu --generate-unsalted [STRING]\tGenerates an unsalted hash from the next string.");
                        Console.WriteLine("-gs --generate-salted [STRING] [SALT]\tGenerates a salted hash from the next two strings.");
                        Console.WriteLine("-h --help\tDisplays this help and exits.");
                        Console.WriteLine("-m --mybb\tCracks salted MyBB style passwords.");
                        Console.WriteLine("-s --search [QUERY] [DICTIONARY_FILE]\tSearches the dictionary file to see if query exists.");
                        Environment.Exit(0);
                        break;
                    case "-b":
                    case "--brute-force":
                        switch (args[1].ToLower())
                        {
                            case "-m":
                            case "--mybb":
                                if (args.Length < 4)
                                {
                                    Console.WriteLine("Missing [HASH_FILE] or length after option " + args[1]);
                                    Environment.Exit(0);
                                }
                                mybb = true;
                                bruteForceLength = Convert.ToInt32(args[2]);
                                hashLocation = args[3];
                                break;
                            default:
                                if (args.Length < 3)
                                {
                                    Console.WriteLine("Missing [HASH_FILE] or length after option " + args[0]);
                                    Environment.Exit(0);
                                }
                                bruteForceLength = Convert.ToInt32(args[1]);
                                hashLocation = args[2];
                                break;
                        }
                        brute = true;
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
                        Console.WriteLine(HashCracker.Md5(args[1]));
                        Environment.Exit(0);
                        break;
                    case "-gs":
                    case "--generate-salted":
                        if (args.Length < 3)
                        {
                            Console.WriteLine("String and Salt must follow " + args[0]);
                            Environment.Exit(0);
                        }
                        Console.WriteLine(HashCracker.Md5(HashCracker.Md5(args[2]) + HashCracker.Md5(args[1])));
                        Environment.Exit(0);
                        break;
                    case "-s":
                    case "--search":
                        if (args.Length < 3)
                        {
                            Console.WriteLine("Query and [DICTIONARY_FILE] must follow " + args[0]);
                            Environment.Exit(0);
                        }
                        if (HashCracker.Exists(args[1], args[2]))
                            Console.WriteLine(args[1] + " was found in dictionary.");
                        else
                            Console.WriteLine(args[1] + " was not found in dictionary.");
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
                    string line = entry.Trim();
                    if (line.StartsWith("#") || line == "" || line == "\n")
                        continue;

                    string[] parts = line.Split(' ');
                    if (mybb)
                        new Task(() => userThread(parts[0], parts[1], parts[2])).Start();
                    else
                        new Task(() => userThread(parts[0], parts[1])).Start();
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.FileName + " was not found! Try running CrackerJac --help for help");
                Environment.Exit(0);
            }
            catch (InternalBufferOverflowException ex)
            {
                Console.WriteLine("Internal Buffer Overflow encountered. Something has gone horribly wrong.\nRestart your computer or use one with more RAM");
                Environment.Exit(0);
            }
            Console.Read();
        }

        private static void userThread(string name, string hash, string salt = "")
        {
            if (brute)
            {
                string result = new HashCracker(hash, dictionaryLocation, salt).BruteCrack(HashCracker.Alphabets.STANDARD_COMPLETE, bruteForceLength);
                if (result != "")
                    Console.WriteLine("Name: " + name + " Cracked Password: " + result);
                else
                    Console.WriteLine(name + ": password could not be bruteforced");
            }
            else
            {
                string result = new HashCracker(hash, dictionaryLocation, salt).DictionaryCrack();
                if (result != "")
                    Console.WriteLine("Name: " + name + " Cracked Password: " + result);
                else
                    Console.WriteLine(name + ": password was not in the dictionary");
            }
        }
    }
}
