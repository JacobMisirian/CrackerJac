using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CrackerJac
{
    public class HashCracker
    {
        public static string HashingMethod { get { return hashingMethod; } set { hashingMethod = value; } }
        private static string hashingMethod = "MD5";

        public string DictionaryAttackUnsalted(string hash, string[] dictionary, string append = "")
        {
            foreach (string entry in dictionary)
            {
                string word = (append == "") ? entry : entry + append;
                if (Hash(word) == hash)
                    return word;
            }

            return "";
        }

        public string DictionaryAttackUnsalted(string hash, string dictionaryPath, string append = "")
        {
            return DictionaryAttackUnsalted(hash, new StreamReader(dictionaryPath).BaseStream, append);
        }
        public string DictionaryAttackUnsalted(string hash, Stream dictionary, string append = "")
        {
            StreamReader reader = new StreamReader(dictionary);
            while (!reader.EndOfStream)
            {
                string word = (append == "") ? reader.ReadLine() : reader.ReadLine() + append;
                if (Hash(word) == hash)
                    return word;
            }

            return "";
        }

        public string DictionaryAttackUnsaltedAppend(string hash, string[] dictionary, int appendFrom, int appendTo)
        {
            string tryWithoutAppend = DictionaryAttackUnsalted(hash, dictionary);
            if (tryWithoutAppend != "")
                return tryWithoutAppend;

            while (appendFrom <= appendTo)
            {
                string res = DictionaryAttackUnsalted(hash, dictionary, appendFrom++.ToString());
                if (res != "")
                    return res;
            }

            return "";
        }

        public string DictionaryAttackUnsaltedAppend(string hash, string dictionaryPath, int appendFrom, int appendTo)
        {
            return DictionaryAttackUnsaltedAppend(hash, new StreamReader(dictionaryPath).BaseStream, appendFrom, appendTo);
        }
        public string DictionaryAttackUnsaltedAppend(string hash, Stream dictionary, int appendFrom, int appendTo)
        {
            string tryWithoutAppend = DictionaryAttackUnsalted(hash, dictionary);
            if (tryWithoutAppend != "")
                return tryWithoutAppend;

            while (appendFrom <= appendTo)
            {
                string res = DictionaryAttackUnsalted(hash, dictionary, appendFrom++.ToString());
                if (res != "")
                    return res;
            }

            return "";
        }

        public string DictionaryAttackSalted(string hash, string salt, string[] dictionary, string append = "")
        {
            foreach (string entry in dictionary)
            {
                string word = (append == "") ? entry : entry + append;
                if (Hash(Hash(salt) + Hash(word)) == hash)
                    return word;
            }

            return "";
        }

        public string DictionaryAttackSalted(string hash, string salt, string dictionaryPath, string append = "")
        {
            return DictionaryAttackSalted(hash, salt, new StreamReader(dictionaryPath).BaseStream, append);
        }
        public string DictionaryAttackSalted(string hash, string salt, Stream dictionary, string append = "")
        {
            
            StreamReader reader = new StreamReader(dictionary);
            while (!reader.EndOfStream)
            {
                string word = (append == null) ? reader.ReadLine() : reader.ReadLine() + append;
                if (Hash(Hash(salt) + Hash(word)) == hash)
                    return word;
            }

            return "";
        }

        public string DictionaryAttackSaltedAppend(string hash, string salt, string[] dictionary, int appendFrom, int appendTo)
        {
            string tryWithoutAppend = DictionaryAttackSalted(hash, salt, dictionary);
            if (tryWithoutAppend != "")
                return tryWithoutAppend;

            while (appendFrom <= appendTo)
            {
                string res = DictionaryAttackSalted(hash, salt, dictionary, appendFrom++.ToString());
                if (res != "")
                    return res;
            }

            return "";
        }

        public string DictionaryAttackSaltedAppend(string hash, string salt, string dictionaryPath, int appendFrom, int appendTo)
        {
            return DictionaryAttackSaltedAppend(hash, salt, new StreamReader(dictionaryPath).BaseStream, appendFrom, appendTo);
        }
        public string DictionaryAttackSaltedAppend(string hash, string salt, Stream dictionary, int appendFrom, int appendTo)
        {
            string tryWithoutAppend = DictionaryAttackSalted(hash, salt, dictionary);
            if (tryWithoutAppend != "")
                return tryWithoutAppend;

            while (appendFrom <= appendTo)
            {
                string res = DictionaryAttackSalted(hash, salt, dictionary, appendFrom++.ToString());
                if (res != "")
                    return res;
            }

            return "";
        }

        public string BruteforceAttackUnsalted(string hash, string letters, int maxLength)
        {
            return bruteForce(hash, letters, maxLength);
        }
        public string BruteforceAttackSalted(string hash, string salt, string letters, int maxLength)
        {
            return bruteForce(hash, letters, maxLength, salt);
        }
        private string bruteForce(string hash, string letters, int maxLength, string salt = "")
        {
            char firstLetter = letters.First();
            char lastLetter = letters.Last();

            for (int length = 1; length < maxLength; ++length)
            {
                StringBuilder accum = new StringBuilder(new String(firstLetter, length));
                while (true)
                {
                    if (accum.ToString().All(val => val == lastLetter))
                        break;
                    for (int i = length - 1; i >= 0; --i)
                        if (accum[i] != lastLetter)
                        {
                            accum[i] = letters[letters.IndexOf(accum[i]) + 1];
                            break;
                        }
                        else
                            accum[i] = firstLetter;
                    if (salt == "")
                    {
                        if (Hash(accum.ToString()) == hash)
                            return accum.ToString();
                    }
                    else if (Hash(Hash(salt) + Hash(accum.ToString())) == hash)
                        return accum.ToString();
//                }
            }
            return "";
        }

        public static string Hash(string text)
        {
            return BitConverter.ToString(((HashAlgorithm)CryptoConfig.CreateFromName(HashingMethod)).ComputeHash(new UTF8Encoding().GetBytes(text))).Replace("-", string.Empty).ToLower();
        }
    }
}