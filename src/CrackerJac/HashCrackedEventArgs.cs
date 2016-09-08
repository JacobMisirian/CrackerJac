using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerJac
{
    public class HashCrackedEventArgs : EventArgs
    {
        public string Hash { get; set; }
        public string Name { get; set; }
        public string PlainText { get; set; }
    }
}
