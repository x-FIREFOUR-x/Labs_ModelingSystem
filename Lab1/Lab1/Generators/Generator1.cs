using System;
using System.Collections.Generic;

namespace Lab1.Generators
{
    public class Generator1 : IGenerator
    {
        private double _lamda;

        public Generator1(double lamda)
        {
            _lamda = lamda;
        }

        public List<double> Generate(int count)
        {
            Random random = new Random();

            List<double> list = new();
            double x = 0;

            for (int i = 0; i < count; i++)
            {
                x = (-1 / _lamda) * Math.Log(random.NextDouble());
                list.Add(x);
            }

            return list;
        }

        public double CalculateDistribution(double x)
        {
            return 1 - Math.Pow(Math.E, -_lamda * x); ;
        }
    }
}
