using System;

namespace CrackerJac
{
    public class CrackerJacConfig
    {
        public string DictionaryFile { get; set; }
        public string HashFile { get; set; }
        public string Method { get; set; }
        public string OutputFile { get; set; }
        public int ThreadCount { get; set; }

        public CrackerJacConfig()
        {
            OutputFile = string.Empty;
            ThreadCount = 1;
        }
    }
}

