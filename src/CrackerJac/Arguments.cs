    using System;
using System.IO;

namespace CrackerJac
{
    public class Arguments
    {
        private string[] args { get; set; }
        private int position = 0;

        public Arguments(string[] args)
        {
            this.args = args;
        }

        public HashCrackerConfiguration Scan()
        {
            HashCrackerConfiguration config = new HashCrackerConfiguration();
            if (args.Length <= 0)
                displayHelp();
            for (position = 0; position < args.Length; position++)
            {
                switch (args[position].ToLower())
                {
                    case "-a":
                    case "--append":
                        config.AppendMode = true;
                        config.AppendMinLength = Convert.ToInt32(expectData("[MIN_LENGTH]"));
                        config.AppendMaxLength = Convert.ToInt32(expectData("[MAX_LENGTH]"));
                        break;
                    case "-b":
                    case "--brute":
                        config.HashCrackerMode = HashCrackerMode.BruteForce;
                        config.BruteForceAlphabetPath = expectData("[ALPHABET_FILE]");
                        config.BruteForceLength = Convert.ToInt32(expectData("[MAX_LENGTH]"));
                        break;
                    case "-d":
                    case "--dictionary":
                        config.DictionaryFilePath = expectData("[DICTIONARY_FILE]");
                        break;
                    case "-e":
                    case "--exists":
                        string query = expectData("[QUERY]");
                        StreamReader reader = new StreamReader(expectData("[DICTIONARY_FILE"));
                        while (!reader.EndOfStream)
                        {
                            string entry = reader.ReadLine();
                            if (entry == query)
                            {
                                Console.WriteLine(query + " exists in file.");
                                Environment.Exit(0);
                            }
                        }
                        Console.WriteLine(query + " was not found in file.");
                        Environment.Exit(0);
                        break;
                    case "-h":
                    case "--help":
                        displayHelp();
                        break;
                    case "-gu":
                    case "--generate-unsalted":
                        Console.WriteLine(HashCracker.Hash(expectData("[TEXT]")));
                        Environment.Exit(0);
                        break;
                    case "-gs":
                    case "--generate-salted":
                        string salt = expectData("[SALT]");
                        string text = expectData("[TEXT]");
                        Console.WriteLine(HashCracker.Hash(HashCracker.Hash(salt) + HashCracker.Hash(text)));
                        Environment.Exit(0);
                        break;
                    case "-m":
                    case "--method":
                        HashCracker.HashingMethod = expectData("[METHOD]");
                        break;
                    case "-o":
                    case "--output":
                        config.OutputMode = true;
                        config.OutputFilePath = expectData("[FILE]");
                        break;
                    case "-s":
                    case "--salted":
                        config.HashCrackerFormat = HashCrackerFormat.Salted;
                        config.HashFilePath = expectData("[HASH_FILE]");
                        break;
                    case "-t":
                    case "--time-show":
                        config.ShowTime = true;
                        break;
                    case "-u":
                    case "--unsalted":
                        config.HashCrackerFormat = HashCrackerFormat.Unsalted;
                        config.HashFilePath = expectData("[HASH_FILE");
                        break;
                    default:
                        Console.WriteLine("Unknown " + ((args[position].StartsWith("-")) ? "flag" : "data") + args[position]);
                        break;
                }
            }
            return config;
        }
        
        private string expectData(string expectedType)
        {
            if (!args[++position].ToLower().StartsWith("-"))
                return args[position];
            Console.WriteLine("Expected " + expectedType + " after " + args[position - 1]);
            Environment.Exit(0);
            return "";
        }

        private void displayHelp()
        {
            Console.WriteLine("CrackerJac.exe [OPTIONS]]");
            Console.WriteLine("-a --append [MIN_VALUE] [MAX_VALUE]     Sets the number that CrackerJac will append to dictionary entries from [MIN_VALUE] to [MAX_VALUE].");
            Console.WriteLine("-b --brute [ALPHABET_FILE] [MAX_LENGTH] Turns on brute force mode using the characters in [ALPHABET_FILE].");
            Console.WriteLine("-d --dictionary [DICTIONARY_FILE]       Sets the dictionary path for CrackerJac.");
            Console.WriteLine("-e --exists [QUERY] [DICTIONARY_FILE]   Determines if [QUERY] exists in [DICTIONARY_FILE].");
            Console.WriteLine("-gu --generate-unsalted [TEXT]          Generates an unsalted hash from [TEXT].");
            Console.WriteLine("-gs --generate-salted [SALT] [TEXT]     Generates a hash from [TEXT] with [SALT].");
            Console.WriteLine("-h --help                               Displays this help and exits.");
            Console.WriteLine("-m --method [METHOD]                    Changes the hashing method from MD5 to [METHOD].");
            Console.WriteLine("-o --output [FILE]                      Redirects output to [FILE].");
            Console.WriteLine("-s --salted [HASH_FILE]                 Turns on salted mode using [HASH_FILE].");
            Console.WriteLine("-t --time-show                          Displays time for hash cracking.");
            Console.WriteLine("-u --unsalted [HASH_FILE]               Turns on unsalted mode using [HASH_FILE].");
            Environment.Exit(0);
        }
    }
}