using System.Collections.Generic;
using System.Linq;
using System;

using Lab2.Model.Elemets;

namespace Lab2.Model
{
    public class Model
    {
        private double _currentTime;

        private readonly List<Element> _elements;

        public Model(List<Element> elements)
        {
            _elements = elements;
            _currentTime = 0;
        }

        public void Simulation(double simulationTime, bool stepsStats = false)
        {
            while (_currentTime < simulationTime)
            {
                Element nextElement = _elements.OrderBy(item => item.NextTime()).First();
                _currentTime = nextElement.NextTime();

                foreach (var element in _elements)
                    element.UpdatedCurrentTime(_currentTime);

                Console.WriteLine();
                foreach (var element in _elements)
                {
                    if (element.TryFinish())
                    {
                        element.FinishService();
                    }
                    
                }
                
                if (stepsStats)
                {
                    foreach (var element in _elements)
                    {
                        element.PrintStats(false);
                    }
                }
            }

            Console.WriteLine("\n=========================================================");
            foreach (var element in _elements)
            {
                element.PrintStats(true);
            }
            Console.WriteLine("=========================================================");
        }
    }
}
