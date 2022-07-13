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
        static Dictionary<string, int> triplets3 = new Dictionary<string, int>();
        static async Task Main(string[] args)
        {
            int th = 2;
            Dictionary<string, int> triplets1 = new Dictionary<string, int>();
            Dictionary<string, int> triplets2 = new Dictionary<string, int>();
            //List<Dictionary<string, int>> triplets = new List<Dictionary<string, int>>();
            Dictionary<string, int>[] triplets = new Dictionary<string, int>[th];
            Console.Write("Inter path to file: ");
            string path = Console.ReadLine();
            path = "C:/Users/sasho/Downloads/bible.txt";
            DateTime start_time = DateTime.Now;
            lines1 = File.ReadAllLines(path);
            
            int mid = lines1.Length / th;
            Console.WriteLine("t1 " + (DateTime.Now - start_time).ToString());
            List<Task<Dictionary<string, int>>> tasks = new List<Task<Dictionary<string, int>>>();
            //triplets.Add(new Dictionary<string, int>());
            //triplets[0] = new Dictionary<string, int>();
            for (int i = 0;i < th; i++)
            {
                //triplets.Add(new Dictionary<string, int>());
                triplets[i] = new Dictionary<string, int>();
                //tasks.Add(Task.Factory.StartNew(() => Line_process(i*mid, i*mid+mid)));//0 7595 7595 15190 15190 22785
                triplets[i] = await Line_process(i * mid, i * mid + mid);
                //Task<Dictionary<string, int>> task = Line_process(i * mid, i * mid + mid);
                //tasks.Add(task);
            }
            Console.WriteLine("t4 " + (DateTime.Now - start_time).ToString());
            //for (int i = 0; i < th; i++)
            //{
            //    tasks[i].Wait();
            //    triplets[i] = tasks[i].Result;
            //}
            //Task t1 = Task.Factory.StartNew(() => Line_process(triplets1, 0, mid));
            //Task t2 = Task.Factory.StartNew(() => Line_process(triplets2, mid, lines1.Length));
            //t1.Wait();
            //t2.Wait();
            Console.WriteLine("t2 " + (DateTime.Now - start_time).ToString());
            for (int i = 1; i < th; i++)
            {
                Merging_dictionaries(triplets[0], triplets[i]);
            }
            //Merging_dictionaries(triplets1, triplets2);
            Console.WriteLine("t3 " + (DateTime.Now - start_time).ToString());
            var sortedDict = (from entry in triplets[0] orderby entry.Value descending  select entry).Take(10);
            foreach(var pair in sortedDict)
            {
                Console.WriteLine(pair.Key+", "+pair.Value);
            }
            TimeSpan end_time = DateTime.Now - start_time;
            Console.WriteLine($"Run time: {end_time.TotalMilliseconds} ms");
        }
        //static void Line_process(Dictionary<string,int> triplet,int start, int end)
        //{
        //    for (; start < end; start++) {
        //        string line = lines1[start];
        //        string[] words = line.Split();
        //        foreach (string word in words)
        //        {
        //            for (int i = 0; i < word.Length - 2; i++)
        //            {
        //                string trip = word.Substring(i, 3).ToLower();
        //                if (triplet.ContainsKey(trip))
        //                {
        //                    triplet[trip]++;
        //                }
        //                else
        //                {
        //                    triplet.Add(trip, 1);
        //                }
        //            }
        //        } 
        //    }

        //}
        static async Task<Dictionary<string, int>> Line_process(int start, int end)
        {
            Dictionary<string, int> triplet = new Dictionary<string, int>();
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
            await Task.Delay(0);
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