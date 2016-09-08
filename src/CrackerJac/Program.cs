using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerJac
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new CrackerJacArgumentParser(args).Parse();
            var hashCracker = new CrackerJacHashCracker(config);
            hashCracker.HashCracked += hashCracker_OnHashCracked;
            hashCracker.StartAttack();
        }

        static void hashCracker_OnHashCracked(object sender, HashCrackedEventArgs e)
        {
            Console.WriteLine("{0} {1}", e.Name, e.PlainText);
        }
    }
}
