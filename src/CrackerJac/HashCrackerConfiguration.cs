using System;
using System.Diagnostics;

namespace CrackerJac
{
    public enum HashCrackerMode
    {
        BruteForce,
        Dictionary
    }

    public enum HashCrackerFormat
    {
        Salted,
        Unsalted
    }

    public class HashCrackerConfiguration
    {
        public HashCrackerMode HashCrackerMode { get { return hashCrackerMode; } set { hashCrackerMode = value; } }
        private HashCrackerMode hashCrackerMode = HashCrackerMode.Dictionary;
        public HashCrackerFormat HashCrackerFormat { get; set; }

        public string BruteForceAlphabetPath { get; set; }
        public int BruteForceLength { get { return bruteForceLength; } set { bruteForceLength = value; } }
        private int bruteForceLength = 8;

        public string HashFilePath { get; set; }
        public string DictionaryFilePath { get; set; }

        public bool OutputMode { get { return outputMode; } set {outputMode = value; } }
        private bool outputMode = false;
        public string OutputFilePath { get; set; }

        public bool AppendMode { get { return appendMode; } set { appendMode = value; }}
        private bool appendMode = false;
        public int AppendMinLength { get; set; }
        public int AppendMaxLength { get; set; }
    
        public bool ShowTime { get { return showTime; } set { showTime = value; } }
        private bool showTime = false;
        public Stopwatch Stopwatch { get { return stopWatch; } set { stopWatch = value; } }
        private Stopwatch stopWatch = new Stopwatch();
    }
}