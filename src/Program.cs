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
        public static string HashLocation = "";
        public static string DictionaryLocation = "";
        public static bool Mybb = false;
        public static bool BruteForce = false;
        public static bool Advanced = false;
        public static bool Caps = false;
        public static int BruteForceLength = 0;
        public static int AdvancedLength = 0;

        static void Main(string[] args)
        {
            try
            {
                new Arguments(args).HandleArguments();
                string[] hashes = File.ReadAllLines(HashLocation);
                foreach (string entry in hashes)
                {
                    string line = entry.Trim();
                    if (line.StartsWith("#") || line == "" || line == "\n")
                        continue;

                    string[] parts = line.Split(' ');
                    if (Mybb)
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
            if (BruteForce)
            {
                string result = new HashCracker(hash, DictionaryLocation, salt).BruteCrack(HashCracker.Alphabets.STANDARD_COMPLETE, BruteForceLength);
                if (result != "")
                    processResult("Name: " + name + " Cracked Password: " + result);
                else
                    processResult(name + ": password could not be bruteforced");
            }
            else
            {
                string result = new HashCracker(hash, DictionaryLocation, salt, AdvancedLength).DictionaryCrack(Advanced);
                if (Caps)
                    result = new HashCracker(hash, DictionaryLocation, salt, AdvancedLength).DictionaryCrack(true, Advanced);
                if (result != "")
                    processResult("Name: " + name + " Cracked Password: " + result);
                else
                    processResult(name + ": password was not in the dictionary");
            }
        }

        private static void processResult(string message)
        {
            Console.WriteLine(message);
            if (!File.Exists("result.txt"))
                File.Create("result.txt");
            File.AppendAllText("result.txt", message);
        }
    }
}
