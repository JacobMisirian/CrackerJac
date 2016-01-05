using System;

namespace CrackerJac
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            HashCrackerConfiguration config = new Arguments(args).Scan();
            new HashCrackerConfigurationHandler(config).Handle();
        }
    }
}