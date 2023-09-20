using System;
using System.Collections.Generic;
using System.Linq;

using ScottPlot;
using ScottPlot.Statistics;

using MathNet.Numerics.Distributions;

using Lab1.Generators;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine("\nGenerator1:");
            IGenerator generator1 = new Generator1(100);
            List<double> list1 = generator1.Generate(10000);
            ShowPlot(list1, "Diagram\\generate1.png");
            ShowStats(list1);
            CheckHypothesis(list1, generator1);
            */

            /*
            Console.WriteLine("\nGenerator2:");
            IGenerator generator2 = new Generator2(0.1, 0.5);
            List<double> list2 = generator2.Generate(10000);
            ShowPlot(list2, "Diagram\\generate2.png");
            ShowStats(list2);
            CheckHypothesis(list2, generator2);
            */

            /*
            Console.WriteLine("\nGenerator3:");
            IGenerator generator3 = new Generator3(Math.Pow(5, 15), Math.Pow(2, 31));
            List<double> list3 = generator3.Generate(10000);
            ShowPlot(list3, "Diagram\\generate3.png");
            ShowStats(list3);
            CheckHypothesis(list3, generator3);
            */

            AverageTest(3, 100);
        }

        static private void AverageTest(int numbGenerator, int countTest)
        {
            double SumConfidenceСhance = 0;

            switch (numbGenerator)
            {
                case 1: 
                    {
                        IGenerator generator1 = new Generator1(100);
                        for (int i = 0; i < countTest; i++)
                        {
                            List<double> list1 = generator1.Generate(10000);
                            ShowStats(list1);
                            SumConfidenceСhance += CheckHypothesis(list1, generator1);
                        }
                    }
                    break;

                case 2:
                    {
                        IGenerator generator2 = new Generator2(0.1, 0.5);
                        for (int i = 0; i < countTest; i++)
                        {
                            List<double> list2 = generator2.Generate(10000);
                            ShowStats(list2);
                            SumConfidenceСhance += CheckHypothesis(list2, generator2);
                        }
                    }
                    break;

                case 3:
                    {
                        IGenerator generator3 = new Generator3(Math.Pow(5, 15), Math.Pow(2, 31));
                        for (int i = 0; i < countTest; i++)
                        {
                            List<double> list3 = generator3.Generate(10000);
                            ShowStats(list3);
                            SumConfidenceСhance += CheckHypothesis(list3, generator3);
                        }
                    }
                    break;

                default: break;
            }

            Console.WriteLine("\n==========================================");
            Console.WriteLine($"Average confidence chance: {Math.Round(SumConfidenceСhance / countTest, 2)}");
        }


        static private void ShowPlot(List<double> list, string name)
        {
            Plot plt = new Plot (1500, 1000);
            Histogram hist = new(list.Min(), list.Max(), 100);

            hist.AddRange(list);

            var bar = plt.AddBar(values: hist.Counts, positions: hist.Bins);
            bar.BarWidth = (list.Max() - list.Min()) / hist.BinCount;

            plt.SaveFig(name);
        }

        static private void ShowStats(List<double> list)
        {
            double average = list.Average();

            double variance = 0;
            foreach (var item in list)
            {
                variance += Math.Pow(item - average, 2);
            }
            variance /= list.Count;

            Console.WriteLine($"Average: {average}");
            Console.WriteLine($"Variance: {variance}");
        }

        static private double CheckHypothesis(List<double> listNumber, IGenerator generator)
        {
            int intervalsCount = 100;

            double min = listNumber.Min();
            double max = listNumber.Max();
            double intervalsSize = (max - min) / intervalsCount;

                // Create 100 intervals with generate numbers 
            int[] countsInIntervals = new int[intervalsCount];
            for (int i = 0; i < listNumber.Count; i++)
            {
                int indexInterval = (int)((listNumber[i] - min) / intervalsSize);

                if(listNumber[i] == max)
                    countsInIntervals[indexInterval - 1] += 1;
                else
                    countsInIntervals[indexInterval] += 1;
            }

                //Merge intervals n >= 5
            List<int> newCountsInIntervals = new List<int>();
            List<(int, int)> newIndexsIntervals = new List<(int, int)>();
            int currentCount = 0;
            int lowIndex = 0;
            for (int i = 0; i < countsInIntervals.Length; i++)
            {
                currentCount += countsInIntervals[i];

                if(currentCount >= 5)
                {
                    newCountsInIntervals.Add(currentCount);
                    newIndexsIntervals.Add((lowIndex, i + 1));

                    lowIndex = i + 1;
                    currentCount = 0;
                }
            }
            if (currentCount != 0)
            {
                newCountsInIntervals[newCountsInIntervals.Count - 1] += currentCount;
                newIndexsIntervals.Add((lowIndex, countsInIntervals.Length));
            }

                //Calculation of the criterion chi^2(x2)
            double x2 = 0;
            for (int i = 0; i < newCountsInIntervals.Count; i++)
            {
                double low = min + intervalsSize * newIndexsIntervals[i].Item1;
                double lowDistribution = generator.CalculateDistribution(low);

                double high = min + intervalsSize * newIndexsIntervals[i].Item2;
                double highDistribution = generator.CalculateDistribution(high);

                double expectedCount = listNumber.Count * (highDistribution - lowDistribution);

                x2 += Math.Pow(newCountsInIntervals[i] - expectedCount, 2) / expectedCount;
            }

            Console.WriteLine($"Intervals: {newCountsInIntervals.Count}");
            Console.WriteLine($"X2: {x2}");

                //Selection of the highest confidence сhance that satisfies the criterion chi^2(x2)
            ChiSquared chiSquared = new ChiSquared(newCountsInIntervals.Count - 2);
            double tableX2 = 0;
            double confidenceСhance = 0;
            for (double i = 0.01; i <= 1; i+= 0.01)
            {
                tableX2 = chiSquared.InverseCumulativeDistribution(i);

                if (x2 < tableX2)
                {
                    confidenceСhance = 1.0 - i;
                    break;
                }
            }

            Console.WriteLine($"Table value X2: {tableX2},  confidence Сhance: {Math.Round(confidenceСhance, 2)}");

            return Math.Round(confidenceСhance, 2);
        }
    }
}
