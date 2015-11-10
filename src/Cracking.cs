using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrackerJac
{
    public class Cracking
    {
        private string hash { get; set; }
        private string salt { get; set; }
        private string dictionaryLocation { get; set; }

        public Cracking(string hash, string dictionaryLocation, string salt = "")
        {
            this.hash = hash;
            this.dictionaryLocation = dictionaryLocation;
            this.salt = salt;
        }

        public string Crack()
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
    }
}
