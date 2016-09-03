using System;

namespace CrackerJac
{
    public class HashCrackedEventArgs : EventArgs
    {
        public string Hash { get; set; }
        public string ID { get; set; }
        public string PlainText { get; set; }
    }
}

