using System.Collections.Generic;
using System.Linq;

using Lab3.Model.Elements;

namespace Lab3.Model.NextElementSelector
{
    public class NextElementPrioritySelector : NextElementSelector
    {
        public NextElementPrioritySelector(List<(Element, double)> nextElements)
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

            foreach (var element in _nextElements)
            {
                if(!element.Item1.Processing)
                {
                    return element.Item1;
                }
            }

            return _nextElements[0].Item1;
        }
    }
}
