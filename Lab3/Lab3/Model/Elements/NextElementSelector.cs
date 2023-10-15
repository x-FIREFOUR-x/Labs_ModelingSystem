using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab3.Model.Elements
{
    public class NextElementSelector
    {
        private List<(Element, double)> _nextElements;

        private Random _random;

        public NextElementSelector(List<(Element, double)> nextElements)
        {
            if (nextElements.Count != 0 && nextElements.Sum(t => t.Item2) != 1.0)
            {
                throw new ArgumentException("Sum chanse have to 1.0");
            }

            _nextElements = nextElements;

            _random = new Random();
        }

        public Element GetNextElement()
        {
            if (_nextElements.Count == 1)
                return _nextElements[0].Item1;

            double randNumber = _random.NextDouble();

            double minInterval = 0;
            double maxInterval = 0;
            foreach (var element in _nextElements)
            {
                maxInterval += element.Item2;
                if (randNumber >= minInterval && randNumber <= maxInterval)
                {
                    return element.Item1;
                }

                minInterval = maxInterval;
            }

            return null;
        }
    }
}
