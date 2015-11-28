using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HashBuilder
{
    class Program
    {
        private static string outputFilePath = "";
        private static bool salted = false;
        static void Main(string[] args)
        {
            string buffer = "";
            Console.WriteLine("Welcome to HashBuilder UI for CrackerJac.\n\nThe purpose of this program is to build hash files wish MD5s that you may have.");
            outputFilePath = getResultFile();
            salted = yesNoDialog("Will these hashes be MyBB salted?");
            int upperBound = promptForInteger("Enter the number of hashes that will go in your file: ");
            for (int i = 0; i < upperBound; i++)
                buffer += userLine(salted);
            if (yesNoDialog("\n\n" + buffer + "\nDoes this look right?"))
                File.WriteAllText(outputFilePath, buffer);
            else
                Main(args);
            Console.Read();
        }

        private static string getResultFile()
        {
            Console.WriteLine("Start by entering the file path to the ending result file:");
            string path = Console.ReadLine();
            if (File.Exists(path))
                if (!yesNoDialog("Overwrite existing file?"))
                    return getResultFile();
            if (!Directory.Exists(path.Substring(0, path.LastIndexOf("\\"))))
            {
                Console.WriteLine("Enter a flie that is in an existing directory!");
                return getResultFile();
            }
            return path;
        }

        private static string userLine(bool promptForSalt = false)
        {
            string buffer = "";
            Console.WriteLine("Enter the name for this hash:");
            buffer += Console.ReadLine() + " ";
            Console.WriteLine("Enter the password hash for " + buffer + ":");
            buffer += Console.ReadLine() + " ";
            if (promptForSalt)
            {
                Console.WriteLine("Enter the salt:");
                buffer += Console.ReadLine();
            }
            buffer += "\n";

            return buffer;
        }

        private static int promptForInteger(string message = "")
        {
            Console.Write(message);
            try
            {
                return Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Enter a valid integer!");
                return promptForInteger(message);
            }
        }

        private static bool yesNoDialog(string message = "")
        {
            Console.Write(message + " Y or N: ");
            switch(Console.ReadLine().ToUpper())
            {
                case "Y":
                case "YES":
                    return true;
                case "N":
                case "NO":
                    return false;
                default:
                    return yesNoDialog(message);
            }
        }
    }
}
