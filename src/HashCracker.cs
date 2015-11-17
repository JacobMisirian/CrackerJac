using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrackerJac
{
    public class HashCracker
    {
        public static string HashingMethod = "MD5";
        private string hash { get; set; }
        private string salt { get; set; }
        private string dictionaryLocation { get; set; }
        
        private int suffixCount { get; set; }
        private int advancedLength { get; set; }

        public HashCracker(string hash, string dictionaryLocation = "", string salt = "", int advancedLength = 999)
        {
            this.hash = hash;
            this.dictionaryLocation = dictionaryLocation;
            this.salt = salt;
            this.suffixCount = 0;
            this.advancedLength = advancedLength;
        }

        public string DictionaryCrack(bool caps = false, bool advanced = false, string suffix = "")
        {
            StreamReader sr = new StreamReader(dictionaryLocation);
            if (salt == "")
                while (!sr.EndOfStream)
                {
                    string entry = sr.ReadLine() + suffix;
                    if (caps)
                        entry = entry[0].ToString().ToUpper() + entry.Substring(1);
                    if (Md5(entry) == hash)
                        return entry;
                }
            else
                while (!sr.EndOfStream)
                {
                    string entry = sr.ReadLine() + suffix;
                    if (caps)
                        entry = entry[0].ToString().ToUpper() + entry.Substring(1);
                    if (Md5(Md5(salt) + Md5(entry)) == hash)
                        return entry;
                }
            if (suffixCount < advancedLength && advanced)
                return DictionaryCrack(caps, advanced, suffixCount++.ToString());
            return "";
        }

        public string BruteCrack(string letters, int maxLength = 10)
        {
            char letters_first = letters.First();
            char letters_last = letters.Last();
            
            for (int currentLength = 1; currentLength <= maxLength; ++currentLength)
            {
                StringBuilder current = new StringBuilder(new String(letters_first, currentLength));

                while (true)
                {

                    if (current.ToString().All(val => val == letters_last))
                        break;

                    for (int i = currentLength - 1; i >= 0; --i)
                    {
                        if (current[i] != letters_last)
                        {
                            current[i] = letters[letters.IndexOf(current[i]) + 1];
                            break;
                        }
                        else
                            current[i] = letters_first;
                    }

                    if (salt == "")
                    {
                        if (Md5(current.ToString()) == hash)
                            return current.ToString();
                    }
                    else if (Md5(Md5(salt) + Md5(current.ToString())) == hash)
                            return current.ToString();
                }
            }
            return "";
        }

        public static bool Exists(string query, string dictionaryLocation)
        {
            StreamReader sr = new StreamReader(dictionaryLocation);
            while (!sr.EndOfStream)
                if (sr.ReadLine() == query)
                    return true;
            return false;
        }

        public static string Md5(string unhashed)
        {
            return BitConverter.ToString(((HashAlgorithm)CryptoConfig.CreateFromName(HashingMethod)).ComputeHash(new UTF8Encoding().GetBytes(unhashed))).Replace("-", string.Empty).ToLower();
        }

        public static class Alphabets
        {
            public const string STANDARD_COMPLETE = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            public const string STANDARD_LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
            public const string STANDARD_UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            public const string NUMBERS = "0123456789";
            public const string SYMBOLS = "`~!@#$%^&*()_-+=\\<>,./?";
        }
    }
}
