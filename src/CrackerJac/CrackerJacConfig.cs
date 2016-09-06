using System;

namespace CrackerJac
{
    public class CrackerJacConfig
    {
        public int BruteForceLength { get; set; }
        public string BruteForceLettersFile { get; set; }
        public string DictionaryFile { get; set; }
        public bool IsBruteForce { get; set; }
        public string HashFile { get; set; }
        public string Method { get; set; }
        public string OutputFile { get; set; }
        public int ThreadCount { get; set; }

        public CrackerJacConfig()
        {
            IsBruteForce = false;
            OutputFile = string.Empty;
            ThreadCount = 1;
        }
    }
}

