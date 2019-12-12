using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TxtSplitter
{
    class Program
    internal class Program
    {
        static void Main(string[] args)
        private static void Main()
        {
            Console.WriteLine("Write files path");
            string path = Console.ReadLine();
            Console.WriteLine("Chooosen path is " + path + "\n");

            Console.WriteLine("Write mark for splitting (!WARNING ALL FILES MUST CONTAIN THE SAME SPLITTING MARK!)");
            string splittingMark = Console.ReadLine();
            var files = Directory.GetFiles(path);

            Console.WriteLine($"Files found {files.Length}:");
            Array.ForEach(files, (x) => Console.WriteLine("\t" + x));
            Console.WriteLine("\nContinue? (Y/N)");

            if (Console.ReadKey(true).Key != ConsoleKey.Y)
            try
            {
                End();
                return;
            }
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

            string newPath = Directory.CreateDirectory(path + "\\splitted").FullName;
                            foreach (string filepath in files)
                            {
                                string[] names = filepath.Split('\\');
                                string name = names[names.Length - 1].Replace(".txt", "");
                                Console.WriteLine("Reading file nº" + filen + " " + name);
                                StreamReader sr = new StreamReader(filepath);
                                Task t = sr.ReadToEndAsync().ContinueWith((x) => SplitFile(x.Result, name, splittingMark, newPath));
                                filecontents.Add(t);
                            }

            int filen = 1;
            List<Task> filecontents = new List<Task>();

            foreach (string filepath in files)
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
                string[] names = filepath.Split('\\');
                string name = names[names.Length - 1].Replace(".txt", "");
                Console.WriteLine("Reading file nº"+filen+" "+name);
                StreamReader sr = new StreamReader(filepath);
                Task t = sr.ReadToEndAsync().ContinueWith((x) => SplitFile(x.Result, name, splittingMark, newPath));
                filecontents.Add(t);
                Console.WriteLine(e.Message);
            }

            Task.WaitAll(filecontents.ToArray());
            Console.WriteLine("\nFinished splitting");
            End();
        }

        static void End()
        private static void End()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }

        static Task SplitFile(string filecontent, string file, string splittingMark, string path)
        private static Task SplitFile(string filecontent, string file, string splittingMark, string path)
        {
            string[] splittedArray = filecontent.Split(splittingMark);
            for (int i = 0; i < splittedArray.Length; i++)
            {
                splittedArray[i] = splittedArray[i].Trim();
                //splittedArray[i] = splittedArray[i].Replace("\r\n", "");
            }
            List<Task> tasks = new List<Task>();

            int n = 0;
            foreach (var splitted in splittedArray)
            {
                StreamWriter sw = File.CreateText($"{path}\\{file}_{n++}.txt");
                tasks.Add(sw.WriteAsync(splitted).ContinueWith((x) => sw.Close())); 

                tasks.Add(sw.WriteAsync(splitted).ContinueWith((x) => sw.Close()));
            }
            return Task.WhenAll(tasks).ContinueWith((x) => Console.WriteLine("File " + file + " splitted"));
        }
