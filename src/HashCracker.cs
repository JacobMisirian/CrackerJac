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
        private string hash { get; set; }
        private string salt { get; set; }
        private string dictionaryLocation { get; set; }

        public HashCracker(string hash, string dictionaryLocation = "", string salt = "")
        {
            this.hash = hash;
            this.dictionaryLocation = dictionaryLocation;
            this.salt = salt;
        }

        public string DictionaryCrack()
        {
            StreamReader sr = new StreamReader(dictionaryLocation);
            if (salt == "")
                while (!sr.EndOfStream)
                {
                    string entry = sr.ReadLine();
                    if (Md5(entry) == hash)
                        return entry;
                }
            else
                while (!sr.EndOfStream)
                {
                    string entry = sr.ReadLine();
                    if (Md5(Md5(salt) + Md5(entry)) == hash)
                        return entry;
                }
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
            return BitConverter.ToString(((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(new UTF8Encoding().GetBytes(unhashed))).Replace("-", string.Empty).ToLower();
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
