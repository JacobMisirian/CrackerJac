using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerJac
{
    class Program
    {
        public static string Alphabet = HashCracker.Alphabets.STANDARD_COMPLETE;
        public static string HashLocation = "";
        public static string DictionaryLocation = "";
        public static string OutputFile = "";
        public static bool Mybb = false;
        public static bool BruteForce = false;
        public static bool Advanced = false;
        public static bool Caps = false;
        public static bool Debug = false;
        public static Stopwatch DebugWatch = new Stopwatch();
        public static int BruteForceLength = 0;
        public static int AdvancedLength = 0;

        static void Main(string[] args)
        {
            try
            {
                new Arguments(args).HandleArguments();
                if (Debug)
                    DebugWatch.Start();
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
                string result = new HashCracker(hash, DictionaryLocation, salt).BruteCrack(Alphabet, BruteForceLength);
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
            processResult("Time: " + DebugWatch.Elapsed.ToString());
        }

        private static void processResult(string message)
        {
            Console.WriteLine(message);
            if (OutputFile != "")
            {
                if (!File.Exists(OutputFile))
                    File.WriteAllText(OutputFile, "");
                File.AppendAllText(OutputFile, message + "\n");
            }
        }
    }
}
