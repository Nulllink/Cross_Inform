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
        static string[] lines1;
        static async Task Main(string[] args)
        {
            int th = 2;
            Dictionary<string, int>[] triplets = new Dictionary<string, int>[th];
            Console.Write("Inter path to file: ");
            string path = Console.ReadLine();
            path = "C:/Users/sasho/Downloads/bible.txt";
            DateTime start_time = DateTime.Now;
            lines1 = File.ReadAllLines(path);
            
            int mid = lines1.Length / th;
            Console.WriteLine("t1 " + (DateTime.Now - start_time).ToString());
            List<Task<Dictionary<string, int>>> tasks = new List<Task<Dictionary<string, int>>>();
            for (int i = 0;i < th; i++)
            {
                triplets[i] = new Dictionary<string, int>();
                triplets[i] = await Line_process(i * mid, i * mid + mid);
            }
            Console.WriteLine("t2 " + (DateTime.Now - start_time).ToString());
            for (int i = 1; i < th; i++)
            {
                Merging_dictionaries(triplets[0], triplets[i]);
            }
            Console.WriteLine("t3 " + (DateTime.Now - start_time).ToString());
            var sortedDict = (from entry in triplets[0] orderby entry.Value descending  select entry).Take(10);
            foreach(var pair in sortedDict)
            {
                Console.WriteLine(pair.Key+", "+pair.Value);
            }
            TimeSpan end_time = DateTime.Now - start_time;
            Console.WriteLine($"Run time: {end_time.TotalMilliseconds} ms");
        }
        static async Task<Dictionary<string, int>> Line_process(int start, int end)
        {
            Dictionary<string, int> triplet = new Dictionary<string, int>();
            for (; start < end; start++)
            {
                string line = lines1[start];
                string[] words = Regex.Split(line, @"\W+");
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
            return triplet;
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
//1t - 798ms 1t - 725ms 2t - 592ms 2t - 843ms 2t - 984ms 2t - 613ms