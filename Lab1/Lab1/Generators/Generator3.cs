using System;
using System.Collections.Generic;

namespace Lab1.Generators
{
    public class Generator3 : IGenerator
    {
        private double _a;
        private double _c;

        public Generator3(double a, double c)
        {
            _a = a;
            _c = c;
        }

        public List<double> Generate(int count)
        {
            Random random = new Random();
            double z = random.NextDouble();

            List<double> list = new();
            double x = 0;

            for (int i = 0; i < count + 1; i++)
            {
                z = (_a * z) % _c;

                if (i != 0)
                {
                    x = z / _c;

                    list.Add(x);
                }
            }

            return list;
        }

        public double CalculateDistribution(double x)
        {
            if (x < 0)
                return 0;
            else if (x > 1)
                return 1;
            else
                return x;
        }
    }
}
