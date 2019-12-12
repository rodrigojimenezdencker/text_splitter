using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TxtSplitter
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                Console.WriteLine("Write files path:");
                string path = Console.ReadLine();
                Console.WriteLine("Chosen path: " + path + "\n");
                var files = Directory.GetFiles(path, "*.txt");
                if (files.Length < 1)
                {
                    Console.WriteLine("No text files found.");
                    End();
                    return;
                }
                Console.WriteLine("Write splitting mark (!!WARNING!! ALL FILES MUST CONTAIN THE SAME SPLITTING MARK):");
                string splittingMark = Console.ReadLine();
                Console.WriteLine($"{files.Length} files found:");
                Array.ForEach(files, (x) => Console.WriteLine("— " + x));
                ConsoleKeyInfo cki;
                do
                {
                    Console.WriteLine("\nContinue? (Y/N)");
                    cki = Console.ReadKey(true);
                    switch (cki.Key)
                    {
                        case ConsoleKey.Y:
                            string newPath = Directory.CreateDirectory(path + "\\splitted").FullName;
                            int filen = 1;
                            List<Task> filecontents = new List<Task>();

                            foreach (string filepath in files)
                            {
                                string[] names = filepath.Split('\\');
                                string name = names[names.Length - 1].Replace(".txt", "");
                                Console.WriteLine("Reading file nº" + filen + " " + name);
                                StreamReader sr = new StreamReader(filepath);
                                Task t = sr.ReadToEndAsync().ContinueWith((x) => SplitFile(x.Result, name, splittingMark, newPath));
                                filecontents.Add(t);
                            }

                            Task.WaitAll(filecontents.ToArray());
                            Console.WriteLine("\nSplitted files successfully!");
                            End();
                            break;
                        case ConsoleKey.N:
                            End();
                            break;
                        default:
                            Console.WriteLine("Please, insert a valid option.");
                            break;
                    }
                } while (cki.Key != ConsoleKey.Y && cki.Key != ConsoleKey.N);                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void End()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }

        private static Task SplitFile(string filecontent, string file, string splittingMark, string path)
        {
            string[] splittedArray = filecontent.Split(splittingMark);
            for (int i = 0; i < splittedArray.Length; i++)
            {
                splittedArray[i] = splittedArray[i].Trim();
            }
            List<Task> tasks = new List<Task>();

            int n = 0;
            foreach (var splitted in splittedArray)
            {
                StreamWriter sw = File.CreateText($"{path}\\{file}_{n++}.txt");
                tasks.Add(sw.WriteAsync(splitted).ContinueWith((x) => sw.Close()));
            }
            return Task.WhenAll(tasks).ContinueWith((x) => Console.WriteLine("File " + file + " splitted"));
        }
    }
}
