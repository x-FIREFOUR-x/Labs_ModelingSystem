using System;
using System.Collections.Generic;
using System.Linq;

using Lab3.Model.Elements;

namespace Lab3.Model.NextElementSelector
{
    public class NextElementProbabilitySelector : NextElementSelector
    {
        private Random _random;

        public NextElementProbabilitySelector(List<(Element, double)> nextElements)
            : base(nextElements)
        {
            if (nextElements.Count != 0 && nextElements.Sum(t => t.Item2) != 1.0)
            {
                throw new ArgumentException("Sum chanse have to 1.0");
            }

            _random = new Random();
        }

        public override Element GetNextElement()
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
