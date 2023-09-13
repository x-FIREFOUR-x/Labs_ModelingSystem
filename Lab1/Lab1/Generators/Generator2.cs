using System;
using System.Collections.Generic;

namespace Lab1.Generators
{
    public class Generator2: IGenerator
    {
        private double _a;
        private double _sigma;

        public Generator2(double a, double sigma)
        {
            _a = a;
            _sigma = sigma;
        }


        public List<double> Generate(int count)
        {
            Random random = new Random();

            List<double> list = new();
            double x = 0;

            for (int i = 0; i < count; i++)
            {
                double u = 0;
                for (int j = 1; j <= 12; j++)
                {
                    u += random.NextDouble();
                }
                u = u - 6;

                x = _sigma * u + _a;
                list.Add(x);
            }

            return list;
        }

        public double CalculateDistribution(double x)
        {
            return ( 1 + Erf((x - _a)/(Math.Sqrt(2) * _sigma))) / 2;
        }

        private double Erf(double x)
        {
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            int sign = 1;
            if (x < 0)
                sign = -1;
            x = Math.Abs(x);

            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }
    }
}
