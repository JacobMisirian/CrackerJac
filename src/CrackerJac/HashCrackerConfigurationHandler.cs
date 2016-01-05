using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CrackerJac
{
    public class HashCrackerConfigurationHandler
    {
        private HashCrackerConfiguration config { get; set; }
        private HashCracker hashCracker = new HashCracker();
        private List<Task> tasks = new List<Task>();

        public HashCrackerConfigurationHandler(HashCrackerConfiguration config)
        {
            this.config = config;
        }

        public void Handle(int waitPeriod = 20)
        {
            StreamReader reader = new StreamReader(config.HashFilePath);
            while (!reader.EndOfStream)
            {
                string[] entry = reader.ReadLine().Split(' ');
                Task task;
                switch (config.HashCrackerMode)
                {
                    case HashCrackerMode.BruteForce:
                        switch (config.HashCrackerFormat)
                        {
                            case HashCrackerFormat.Salted:
                                tasks.Add(new Task(() => processOutput(entry[0], hashCracker.BruteforceAttackSalted(entry[1], entry[2], File.ReadAllText(config.BruteForceAlphabetPath), config.BruteForceLength))));
                                break;
                            case HashCrackerFormat.Unsalted:
                                tasks.Add(new Task(() => processOutput(entry[0], hashCracker.BruteforceAttackUnsalted(entry[1], File.ReadAllText(config.BruteForceAlphabetPath), config.BruteForceLength))));
                                break;
                        }
                        break;
                    case HashCrackerMode.Dictionary:
                        switch (config.HashCrackerFormat)
                        {
                            case HashCrackerFormat.Salted:
                                if (config.AppendMode)
                                    task = new Task(() => processOutput(entry[0], hashCracker.DictionaryAttackSaltedAppend(entry[1], entry[2], config.DictionaryFilePath, config.AppendMinLength, config.AppendMaxLength)));
                                else
                                    task = new Task(() => processOutput(entry[0], hashCracker.DictionaryAttackSalted(entry[1], entry[2], config.DictionaryFilePath)));
                                tasks.Add(task);
                                break;
                            case HashCrackerFormat.Unsalted:
                                if (config.AppendMode)
                                    task = new Task(() => processOutput(entry[0], hashCracker.DictionaryAttackSaltedAppend(entry[1], entry[2], config.DictionaryFilePath, config.AppendMinLength, config.AppendMaxLength)));
                                else
                                    task = new Task(() => processOutput(entry[0], hashCracker.DictionaryAttackUnsalted(entry[1], config.DictionaryFilePath)));
                                tasks.Add(task);
                                break;
                        }
                        break;
                }
            }

            foreach (Task task in tasks)
                task.Start();
            while (true)
            {
                bool allTasksStopped = true;
                foreach (Task task in tasks)
                    if (!task.IsCompleted)
                        allTasksStopped = false;
                if (allTasksStopped)
                    return;
                Thread.Sleep(waitPeriod);
            }

        }

        private void processOutput(string name, string output)
        {
            if (config.OutputMode)
                File.AppendAllText(config.OutputFilePath, name + " " + output);
            else
                Console.WriteLine(name +  " " + output);
        }
    }
}