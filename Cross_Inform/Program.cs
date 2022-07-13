using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cross_Inform
{
    class Program
    {
        static Dictionary<string,int> triplets = new Dictionary<string, int>();
        static Dictionary<string, int> triplets1 = new Dictionary<string, int>();
        static Dictionary<string, int> triplets2 = new Dictionary<string, int>();
        static string[] lines1;
        static string[] lines2;
        static List<string> all_lines = new List<string>();
        static List<string> triplets3 = new List<string>();
        static int index = 0;
        static string path;
        static void Main(string[] args)
        {
            Console.Write("Inter path to file: ");
            path = Console.ReadLine();
            path = "C:/Users/sasho/Downloads/bible.txt";
            DateTime start_time = DateTime.Now;

            //File_process(path);
            //all_lines = File.ReadAllLines(path);
            //int delt = all_lines.Length / 2;
            //lines1 = new string[delt];
            //lines2 = new string[delt+1];
            //Array.Copy(all_lines, lines1, delt);
            //Array.Copy(all_lines, delt + 1, lines2, 0, delt);
            Task task = Task.Factory.StartNew(() => Read_file());
            Task task2 = Task.Factory.StartNew(() => Line_process3(task));
            //Task task3 = Task.Factory.StartNew(() => Trip_process());
            //int old_i = 0;
            //while (!task.IsCompleted)
            //{
            //    if (old_i <= all_lines.Count - 1)
            //    {
            //        Task task2 = Task.Factory.StartNew(() => Line_process(all_lines[old_i]));
            //        old_i++;
            //    }
            //}
            //Action a = new Action(Line_process);
            //Task t = new Task(a);
            //t.Start();
            //a = new Action(Line_process2);
            //Task t2 = new Task(a);
            //t2.Start();
            //while (!t.IsCompleted || !t2.IsCompleted)
            //{
            //    Thread.Sleep(10);
            //}
            //foreach (var pair in triplets2)
            //{
            //    if (triplets1.ContainsKey(pair.Key))
            //    {
            //        triplets1[pair.Key] += pair.Value;
            //    }
            //    else
            //    {
            //        triplets1.Add(pair.Key, pair.Value);
            //    }
            //}

            task2.Wait();
            var sortedDict = (from entry in triplets1 orderby entry.Value descending  select entry).Take(10);
            foreach(var pair in sortedDict)
            {
                Console.WriteLine(pair.Key+", "+pair.Value);
            }
            TimeSpan end_time = DateTime.Now - start_time;
            Console.WriteLine($"Run time: {end_time.TotalMilliseconds} ms");
        }
        static void File_process(string path)
        {
            string[] lines = File.ReadAllLines(path);
            //Array.Resize<string>(ref lines, 1);//test
            foreach (string line in lines)
            {
                string[] words = line.Split();
                foreach(string word in words)
                {
                    for(int i = 0; i < word.Length - 2; i++)
                    {
                        string trip = word.Substring(i, 3).ToLower();
                        if (triplets.ContainsKey(trip))
                        {
                            triplets[trip]++;
                        }
                        else
                        {
                            triplets.Add(trip, 1);
                        }
                        
                    }
                }
            }
            
        }
        static void Line_process()
        {
            while (index < all_lines.Count)
            {
                string line = all_lines[index];
                index++;

                string[] words = line.Split();
                foreach (string word in words)
                {
                    for (int i = 0; i < word.Length - 2; i++)
                    {
                        string trip = word.Substring(i, 3).ToLower();
                        if (triplets1.ContainsKey(trip))
                        {
                            triplets1[trip]++;
                        }
                        else
                        {
                            triplets1.Add(trip, 1);
                        }

                    }
                }
            }
        }
        static void Line_process(string line)
        {
            string[] words = line.Split();
            foreach (string word in words)
            {
                for (int i = 0; i < word.Length - 2; i++)
                {
                    triplets3.Add(word.Substring(i, 3).ToLower());
                }
            }

        }
        static void Trip_process()
        {
            Thread.Sleep(100);
            int old_i = 0;
            int old_count = 1;
            while(old_count < triplets3.Count && old_i < old_count)
            {
                old_count = triplets3.Count;
                string trip = triplets3[old_i];
                if (triplets1.ContainsKey(trip))
                {
                    triplets1[trip]++;
                }
                else
                {
                    triplets1.Add(trip, 1);
                }
                old_i++;
            }
        }
        static void Line_process2()
        {
            while (index < all_lines.Count)
            {
                string line = all_lines[index];
                index++;

                string[] words = line.Split();
                foreach (string word in words)
                {
                    for (int i = 0; i < word.Length - 2; i++)
                    {
                        string trip = word.Substring(i, 3).ToLower();
                        if (triplets2.ContainsKey(trip))
                        {
                            triplets2[trip]++;
                        }
                        else
                        {
                            triplets2.Add(trip, 1);
                        }

                    }
                }
            }
        }
        static void Read_file()
        {
            StreamReader sr = new StreamReader(path);
            string line;
            while(null != (line = sr.ReadLine()))
            {
                all_lines.Add(line);
            }
        }
        
        static void Line_process3(Task t)
        {
            Thread.Sleep(10);
            int ind = 0;
            while (!t.IsCompleted || ind < all_lines.Count)
            {
                string line = all_lines[ind];
                string[] words = line.Split();
                foreach (string word in words)
                {
                    for (int i = 0; i < word.Length - 2; i++)
                    {
                        string trip = word.Substring(i, 3).ToLower();
                        if (triplets1.ContainsKey(trip))
                        {
                            triplets1[trip]++;
                        }
                        else
                        {
                            triplets1.Add(trip, 1);
                        }

                    }
                }
                ind++;
            }
        }
    }
}
//1t - 798ms 1t - 725ms 2t - 592ms 2t - 843ms