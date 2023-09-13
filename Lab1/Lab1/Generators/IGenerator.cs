using System.Collections.Generic;

namespace Lab1.Generators
{
    public interface IGenerator
    {
        public double CalculateDistribution(double x);

        public List<double> Generate(int count);
    }
}
