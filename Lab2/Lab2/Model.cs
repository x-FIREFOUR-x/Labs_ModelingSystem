using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
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

        public void Simulation(double simulationTime)
        {
            while (_currentTime < simulationTime)
            {
                Element nextElement = _elements.OrderBy(item => item.NextTime).First();
                _currentTime = nextElement.NextTime;

                foreach (var element in _elements)
                    element.UpdatedCurrentTime(_currentTime);

                foreach (var element in _elements)
                {
                    if (Math.Abs(element.NextTime - _currentTime) < .0001f)
                    {
                        element.FinishService();
                    }
                }
            }
        }
    }
}
