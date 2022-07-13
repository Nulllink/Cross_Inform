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
        static string[] lines1;
        static void Main(string[] args)
        {
            int th = 4;
            Dictionary<string, int> triplets1 = new Dictionary<string, int>();
            Dictionary<string, int> triplets2 = new Dictionary<string, int>();
            Dictionary<string, int> triplets3 = new Dictionary<string, int>();
            Dictionary<string, int> triplets4 = new Dictionary<string, int>();
            //List<Dictionary<string, int>> triplets = new List<Dictionary<string, int>>();
            Dictionary<string, int>[] triplets = new Dictionary<string, int>[th];
            Console.Write("Inter path to file: ");
            string path = Console.ReadLine();
            path = "C:/Users/sasho/Downloads/bible.txt";
            DateTime start_time = DateTime.Now;
            lines1 = File.ReadAllLines(path);
            
            int mid = lines1.Length / th;
            Console.WriteLine("t1 " + (DateTime.Now - start_time).ToString());
            //List<Task> tasks = new List<Task>();
            //for (int i = 0; i < th; i++)
            //{
            //    //tasks.Add(Task t1 = Task.Factory.StartNew(() => Line_process(triplets1, 0, mid)));
            //}
            Task t1 = Task.Factory.StartNew(() => Line_process(triplets1, 0, mid));
            Task t2 = Task.Factory.StartNew(() => Line_process(triplets2, mid, 2*mid));
            Task t3 = Task.Factory.StartNew(() => Line_process(triplets3, 2*mid, 3*mid));
            Task t4 = Task.Factory.StartNew(() => Line_process(triplets4, 3 * mid, lines1.Length));
            Console.WriteLine("t4 " + (DateTime.Now - start_time).ToString());
            //for (int i = 0; i < th; i++)
            //{
            //    tasks[i].Wait();
            //    triplets[i] = tasks[i].Result;
            //}
            t1.Wait();
            t2.Wait();
            t3.Wait();
            t4.Wait();
            Console.WriteLine("t2 " + (DateTime.Now - start_time).ToString());
            //for (int i = 1; i < th; i++)
            //{
            //    Merging_dictionaries(triplets[0], triplets[i]);
            //}
            Merging_dictionaries(triplets1, triplets2);
            Merging_dictionaries(triplets1, triplets3);
            Merging_dictionaries(triplets1, triplets4);
            Console.WriteLine("t3 " + (DateTime.Now - start_time).ToString());
            var sortedDict = (from entry in triplets1 orderby entry.Value descending  select entry).Take(10);
            foreach(var pair in sortedDict)
            {
                Console.WriteLine(pair.Key+", "+pair.Value);
            }
            TimeSpan end_time = DateTime.Now - start_time;
            Console.WriteLine($"Run time: {end_time.TotalMilliseconds} ms");
        }
        static void Line_process(Dictionary<string, int> triplet, int start, int end)
        {
            for (; start < end; start++)
            {
                string line = lines1[start];
                string[] words = line.Split();
                foreach (string word in words)
                {
                    for (int i = 0; i < word.Length - 2; i++)
                    {
                        string trip = word.Substring(i, 3).ToLower();
                        if (triplet.ContainsKey(trip))
                        {
                            triplet[trip]++;
                        }
                        else
                        {
                            triplet.Add(trip, 1);
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
//1t - 798ms 1t - 725ms 2t - 592ms 2t - 843ms 2t - 984ms 2t - 613ms 4t = 676ms 3t = 570ms 4t = 472ms