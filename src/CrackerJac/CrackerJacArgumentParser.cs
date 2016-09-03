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
            Console.WriteLine("-d --dict [FILE]     Sets the dict file to [DICT].");
            Console.WriteLine("-h --help            Displays this help and exits.");
            Console.WriteLine("-f --file [FILE]     Sets the hash file to [FILE].");
            Console.WriteLine("-o --output [FILE]   Appends the output to [FILE].");
            Environment.Exit(0);
        }
    }
}

