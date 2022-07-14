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
        
        const int th = 2; //количество потоков, в этой версии не изменяемая
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Inter path to file: ");
                string path = Console.ReadLine();

                if (File.Exists(path))
                {
                    DateTime start_time = DateTime.Now;
                    string[] all_lines = File.ReadAllLines(path); //чтение файла в массив
                    int mid = all_lines.Length / th; //деление массива на блоки

                    Dictionary<string, int> triplets1 = new Dictionary<string, int>(); //словари для триплетов
                    Dictionary<string, int> triplets2 = new Dictionary<string, int>();
                    Task[] tasks = new Task[th]; //массив тасков. с двумя отдельными тасками работало быстрее, но нужно было ждать каждую отдельно

                    tasks[0] = Task.Factory.StartNew(() => Line_process(all_lines, triplets1, 0, mid)); //запуск задачи. передал словарь, начальный и конечный индекс блока массива
                    tasks[1] = Task.Factory.StartNew(() => Line_process(all_lines, triplets2, mid, all_lines.Length));
                    Task.WaitAll(tasks); //ожидание задач. ждать отдельно быстрее

                    Merging_dictionaries(triplets1, triplets2); //функция объединения словарей, работает очень шустро
                    var sortedDict = (from entry in triplets1 orderby entry.Value descending select entry).Take(10); //выборка 10 самых популярный триплета с помощью LINQ
                    foreach (var pair in sortedDict)
                    {
                        Console.Write(pair.Key + ",");
                    }
                    Console.WriteLine();
                    TimeSpan end_time = DateTime.Now - start_time;
                    Console.WriteLine($"Run time: {end_time.TotalMilliseconds} ms");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine("Файла не существует, проверьте верен ли путь");
                }
            }
        }
        /// <summary>
        /// Добавляет в словарь триплеты из массива строк, в промежутке указанных индексов
        /// </summary>
        /// <param name="lines">Массив строк</param>
        /// <param name="triplets">Словарь</param>
        /// <param name="start">Начальный индекс</param>
        /// <param name="end">Конечный индекс</param>
        static void Line_process(string[] lines, Dictionary<string, int> triplets, int start, int end)
        {
            for (; start < end; start++)
            {
                string line = lines[start];
                string[] words = Regex.Split(line,@"\W+"); //вырезка слов из строки без знаков
                foreach (string word in words)
                {
                    for (int i = 0; i < word.Length - 2; i++)
                    {
                        string trip = word.Substring(i, 3).ToLower(); //вырезка триплетов
                        if (triplets.ContainsKey(trip)) //проверка на существование ключа в словаре
                        {
                            triplets[trip]++; //инкремент, если есть
                        }
                        else
                        {
                            triplets.Add(trip, 1); //добавление, если нет
                        }
                    }
                }
            }

        }
        /// <summary>
        /// Объединяет словари
        /// </summary>
        /// <param name="in_d">Словарь в который объединить</param>
        /// <param name="from_d">Словарь из которого берутся даные</param>
        static void Merging_dictionaries(Dictionary<string,int> in_d, Dictionary<string, int> from_d)
        {
            foreach (var pair in from_d) //выборка пар
            {
                if (in_d.ContainsKey(pair.Key)) //проверка на наличие
                {
                    in_d[pair.Key] += pair.Value; //сумма
                }
                else
                {
                    in_d.Add(pair.Key, pair.Value); //добавление
                }
            }
        }
        
    }
}
//1t - 798ms 1t - 725ms 2t - 592ms 2t - 843ms 2t - 984ms 2t - 613ms 4t = 676ms 3t = 570ms 4t = 472ms 2t = 633ms 4t = 548ms 10t = 700ms