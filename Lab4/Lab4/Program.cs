using System.Collections.Generic;
using System;
using System.Diagnostics;

using Lab4.Model;
using Lab4.Model.Queue;

namespace Lab4
{ 
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("test sequantial model");
            Test(() => { return ModelsFactory.CreateSequentialModel(20); },
                new List<int> { 500, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000}
            );

            Console.WriteLine("\ntest branches model");
            Test(() => { return ModelsFactory.CreateBranchesModel(5, 4); },
                new List<int> { 500, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000 }
            );
        }

        private static void Test(Func<Model<DefaultQueueItem>> createModel, List<int> timesSimulations)
        {
            Model<DefaultQueueItem> model;
            int countRepeatTest = 20;

            Console.WriteLine("time     AverageTime");
            for (int i = 0; i < timesSimulations.Count; i++)
            {
                model = createModel();
                model.Simulation(timesSimulations[i], false);

                double time = 0;
                for (int j = 0; j < countRepeatTest; j++)
                {
                    model = createModel();

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    model.Simulation(timesSimulations[i], false);

                    stopwatch.Stop();
                    time += stopwatch.Elapsed.TotalMilliseconds;
                }

                double averateTime = Math.Round(time, 2);
                Console.WriteLine(timesSimulations[i].ToString() + "     " + averateTime.ToString() + " ms");
            }
        }
    }
}
