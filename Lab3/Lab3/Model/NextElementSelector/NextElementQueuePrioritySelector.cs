﻿using System.Collections.Generic;
using System.Linq;

using Lab3.Model.Elements;
using Lab3.Model.Queue;

namespace Lab3.Model.NextElementSelector
{
    public class NextElementQueuePrioritySelector<T>: NextElementSelector<T> where T : DefaultQueueItem
    {
        public NextElementQueuePrioritySelector(List<(Element<T>, double)> nextElements)
            : base(nextElements)
        {
            _nextElements = _nextElements.OrderByDescending(num => num.Item2).ToList();
        }

        public override Element<T> GetNextElement()
        {
            if (_nextElements.Count == 1)
                return _nextElements[0].Item1;

            if (_nextElements.Count == 0)
                return null;

            int minCount = int.MaxValue;
            Element<T> nextElement = _nextElements[0].Item1;
            foreach (var element in _nextElements)
            {
                if (element.Item1.Processing)
                {
                    if (minCount > ((Process<T>)element.Item1).QueueSize)
                    {
                        minCount = ((Process<T>)element.Item1).QueueSize;
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
