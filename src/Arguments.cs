using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CrackerJac
{
    public class Arguments
    {
        private string[] args { get; set; }
        private int position { get; set; }

        public Arguments(string[] args)
        {
            this.args = args;
        }

        public void HandleArguments()
        {
            if (args.Length <= 0)
                displayHelp();
            for (position = 0; position < args.Length; position++)
            {
                switch (args[position].ToLower())
                {
                    case "-a":
                    case "--advanced":
                        Program.AdvancedLength = Convert.ToInt32(expectData("advanced length"));
                        Program.Advanced = true;
                        break;
                    case "-h":
                    case "--help":
                        displayHelp();
                        break;
                    case "-b":
                    case "--brute-force":
                        Program.BruteForce = true;
                        Program.BruteForceLength = Convert.ToInt32(expectData("brute force length"));
                        Program.HashLocation = expectData("hash file");
                        break;
                    case "-c":
                    case "--caps":
                        Program.Caps = true;
                        break;
                    case "-d":
                    case "--debug":
                        Program.Debug = true;
                        break;
                    case "-m":
                    case "--mybb":
                        Program.Mybb = true;
                        break;
                    case "-gu":
                    case "--generate-unsalted":
                        Console.WriteLine(HashCracker.Hash(expectData("unhashed string")));
                        Environment.Exit(0);
                        break;
                    case "-gs":
                    case "--generate-salted":
                        Console.WriteLine(HashCracker.Hash(HashCracker.Hash(expectData("salt")) + HashCracker.Hash("unhashed string")));
                        Environment.Exit(0);
                        break;
                    case "-l":
                    case "--letters":
                        string alpha = expectData("alphabet");
                        switch(alpha.ToUpper())
                        {
                            case "STDCOMPLETE":
                                Program.Alphabet = HashCracker.Alphabets.STANDARD_COMPLETE;
                                break;
                            case "STDLOWER":
                                Program.Alphabet = HashCracker.Alphabets.STANDARD_LOWERCASE;
                                break;
                            case "STDUPPER":
                                Program.Alphabet = HashCracker.Alphabets.STANDARD_UPPERCASE;
                                break;
                            case "SYMS":
                                Program.Alphabet = HashCracker.Alphabets.SYMBOLS;
                                break;
                            case "NUMS":
                                Program.Alphabet = HashCracker.Alphabets.NUMBERS;
                                break;
                            default:
                                if (File.Exists(alpha))
                                    Program.Alphabet = File.ReadAllText(alpha);
                                else
                                {
                                    Console.WriteLine("Alphabet needs to be STDCOMPLETE, STDLOWER, STDUPPER, NUMS, or SYMS.");
                                    Environment.Exit(0);
                                }
                                break;
                        }
                        break;
                    case "-o":
                    case "--output":
                        Program.OutputFile = expectData("output file");
                        break;
                    case "-s":
                    case "--search":
                        string query = expectData("query");
                        if (HashCracker.Exists(query, expectData("dictionary location")))
                            Console.WriteLine(query + " was found on dictionary");
                        else
                            Console.WriteLine(query + " was not found in dictionary");
                        Environment.Exit(0);
                        break;
                    case "-t":
                    case "--type":
                        HashCracker.HashingMethod = expectData("hashing method");
                        break;
                    default:
                        if (args[position].StartsWith("-"))
                        {
                            Console.WriteLine("Unknown option " + args[position]);
                            Environment.Exit(0);
                        }
                        Program.HashLocation = args[position];
                        Program.DictionaryLocation = expectData("dictionary location");
                        break;
                }
            }
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
            Console.WriteLine("CrackerJac.exe [OPTIONS] [HASH_FILE] [DICTIONARY_FILE]");
            Console.WriteLine("Options:");
            Console.WriteLine("-a --advanced [MAX_LENGTH]\tTries the normal dictionary method, then begins appending 0-[MAX_LENGTH].");
            Console.WriteLine("-b --brute-force [(-m)] [LENGTH] [HASH_FILE]\tAttempts a brute force attack optionally with the MyBB option AFTER -b");
            Console.WriteLine("-c --caps\tAlterates and tries the dictionary entries with the first letter capitalized.");
            Console.WriteLine("-gu --generate-unsalted [STRING]\tGenerates an unsalted hash from the next string.");
            Console.WriteLine("-gs --generate-salted [SALT] [STRING]\tGenerates a salted hash from the next two strings.");
            Console.WriteLine("-h --help\tDisplays this help and exits.");
            Console.WriteLine("-l --letters [ALPHA]\tChanges the letter scheme for brute. Options are STDCOMPLETE STDLOWER STDUPPER NUMS SYMS or a file_path.");
            Console.WriteLine("-m --mybb\tCracks salted MyBB style passwords.");
            Console.WriteLine("-o --output [FILE]\tSends the results of a crack to a file.");
            Console.WriteLine("-s --search [QUERY] [DICTIONARY_FILE]\tSearches the dictionary file to see if query exists.");
            Console.WriteLine("-t --type [METHOD]\tChanges the type of hash from md5 to [METHOD].");
            Environment.Exit(0);
        }
    }
}
