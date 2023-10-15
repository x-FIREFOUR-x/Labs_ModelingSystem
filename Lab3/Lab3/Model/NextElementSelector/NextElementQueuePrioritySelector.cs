using System.Collections.Generic;
using System.Linq;

using Lab3.Model.Elements;

namespace Lab3.Model.NextElementSelector
{
    public class NextElementQueuePrioritySelector : NextElementSelector
    {
        public NextElementQueuePrioritySelector(List<(Element, double)> nextElements)
            : base(nextElements)
        {
            _nextElements = _nextElements.OrderByDescending(num => num.Item2).ToList();
        }

        public override Element GetNextElement()
        {
            if (_nextElements.Count == 1)
                return _nextElements[0].Item1;

            if (_nextElements.Count == 0)
                return null;

            int minCount = int.MaxValue;
            Element nextElement = _nextElements[0].Item1;
            foreach (var element in _nextElements)
            {
                if (element.Item1.Processing)
                {
                    if (minCount > ((Process)element.Item1).QueueSize)
                    {
                        minCount = ((Process)element.Item1).QueueSize;
                        nextElement = element.Item1;
                    }
                }
                else
                {
                    nextElement = element.Item1;
                    break;
                }
            }

            return nextElement;
        }
    }
}
