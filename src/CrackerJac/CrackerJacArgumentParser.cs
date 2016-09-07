using System;

namespace CrackerJac
{
    public class CrackerJacArgumentParser
    {
        private string[] args;
        private int position;

        public CrackerJacArgumentParser(string[] args)
        {
            if (args.Length == 0)
                displayHelp();
            this.args = args;
        }

        public CrackerJacConfig Parse()
        {
            CrackerJacConfig config = new CrackerJacConfig();
            for (position = 0; position < args.Length; position++)
            {
                switch (args[position++].ToLower())
                {
                    case "-a":
                    case "--append":
                        while (position < args.Length)
                        {
                            if (args[position].StartsWith("-"))
                            {
                                position--;
                                break;
                            }
                            config.TryAppends.Add(args[position++]);
                        }
                        break;
                    case "-ar":
                    case "--append-range":
                        int min = Convert.ToInt32(expectData("[MIN]"));
                        position++;
                        int max = Convert.ToInt32(expectData("[MAX]"));
                        while (min <= max)
                            config.TryAppends.Add(min++.ToString());
                        break;
                    case "-b":
                    case "--brute":
                        config.IsBruteForce = true;
                        config.BruteForceLength = Convert.ToInt32(expectData("[LENGTH]"));
                        position++;
                        config.BruteForceLettersFile = expectData("[FILE]");
                        break;
                    case "-c":
                    case "--caps":
                        config.TryCaps = true;
                        position--;
                        break;
                    case "-d":
                    case "--dict":
                        config.DictionaryFile = expectData("[FILE]");
                        break;
                    case "-h":
                    case "--help":
                        displayHelp();
                        break;
                    case "-f":
                    case "--file":
                        config.HashFile = expectData("[FILE]");
                        Console.WriteLine(config.HashFile);
                        break;
                    case "-g":
                    case "--generate":
                        Console.WriteLine(new HashCracker(config.Method).Hash(expectData("[STRING]")));
                        Environment.Exit(0);
                        break;
                    case "-m":
                    case "--method":
                        config.Method = expectData("[ALGO]");
                        break;
                    case "-o":
                    case "--output":
                        config.OutputFile = expectData("[FILE]");
                        break;
                    case "-t":
                    case "--threads":
                        config.ThreadCount = Convert.ToInt32(expectData("[NUM]"));
                        break;
                    default:
                        Console.WriteLine("Unknown flag or floating data {0}!", args[position - 1]);
                        Environment.Exit(0);
                        break;
                }
            }
            return config;
        }

        private string expectData(string type)
        {
            if (args[position].StartsWith("-"))
                throw new Exception(string.Format("Expected data type {0}, got flag {1}!", type, args[position]));
            return args[position];
        }

        private void displayHelp()
        {
            Console.WriteLine("CrackerJac [FLAGS]");
            Console.WriteLine("-a --append [STRING] [STRING] [STRING]... Adds all the strings until the next flag to the append list.");
            Console.WriteLine("-ar --append-range [MIN] [MAX]   Adds all the numbers between [MIN] and [MAX] to the append list.");
            Console.WriteLine("-b --brute [LENGTH [FILE]  Turns on bruteforce mode using the [LENGTH] and letters in [FILE].");
            Console.WriteLine("-c --caps            Turns on caps mode to try capitalized dictionary entries.");
            Console.WriteLine("-d --dict [FILE]     Sets the dict file to [DICT].");
            Console.WriteLine("-h --help            Displays this help and exits.");
            Console.WriteLine("-f --file [FILE]     Sets the hash file to [FILE].");
            Console.WriteLine("-g --generate [STRING]  Displays the hash for the [STRING] and exits.");
            Console.WriteLine("-m --method [ALGO]   Sets the hash algo to [ALGO].");
            Console.WriteLine("-o --output [FILE]   Appends the output to [FILE].");
            Environment.Exit(0);
        }
    }
}

