using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Cross_Inform
{
    class Program
    {
        static string[] all_lines;
        const int th = 2;
        //static Dictionary<string, int>[] triplets = new Dictionary<string, int>[th];
        static void Main(string[] args)
        {
            Console.Write("Inter path to file: ");
            string path = Console.ReadLine();
            path = "C:/Users/sasho/Downloads/bible.txt";
            DateTime start_time = DateTime.Now;
            all_lines = File.ReadAllLines(path);
            int mid = all_lines.Length / th;
            Dictionary<string, int> triplets1 = new Dictionary<string, int>();
            Dictionary<string, int> triplets2 = new Dictionary<string, int>();
            Task[] tasks = new Task[th];
            tasks[0] = Task.Factory.StartNew(() => Line_process(triplets1, 0, mid));
            tasks[1] = Task.Factory.StartNew(() => Line_process(triplets2, mid, all_lines.Length));
            //for (int i = 0; i < th; i++)
            //{
            //    triplets[i] = new Dictionary<string, int>();
            //}
            //for (int j = 0; j < th-1; j++)
            //{
            //    tasks[j] = Task.Factory.StartNew(() => Line_process(j, j*mid, (j+1)*mid));
            //    Thread.Sleep(10);
            //}
            //tasks[th-1] = Task.Factory.StartNew(() => Line_process(th-1,  (th-1)* mid, all_lines.Length));
            Task.WaitAll(tasks);
            for (int i = 1; i < th; i++)
            {
                Merging_dictionaries(triplets1, triplets2);
            }
            var sortedDict = (from entry in triplets1 orderby entry.Value descending  select entry).Take(10);
            foreach(var pair in sortedDict)
            {
                Console.Write(pair.Key+",");
            }
            Console.WriteLine();
            TimeSpan end_time = DateTime.Now - start_time;
            Console.WriteLine($"Run time: {end_time.TotalMilliseconds} ms");
        }
        static void Line_process(Dictionary<string, int> triplets, int start, int end)
        {
            for (; start < end; start++)
            {
                string line = all_lines[start];
                string[] words = Regex.Split(line,@"\W+");
                foreach (string word in words)
                {
                    for (int i = 0; i < word.Length - 2; i++)
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
        static void Merging_dictionaries(Dictionary<string,int> in_d, Dictionary<string, int> from_d)
        {
            foreach (var pair in from_d)
            {
                if (in_d.ContainsKey(pair.Key))
                {
                    in_d[pair.Key] += pair.Value;
                }
                else
                {
                    in_d.Add(pair.Key, pair.Value);
                }
            }
        }
        
    }
}
//1t - 798ms 1t - 725ms 2t - 592ms 2t - 843ms 2t - 984ms 2t - 613ms 4t = 676ms 3t = 570ms 4t = 472ms 2t = 633ms 4t = 548ms