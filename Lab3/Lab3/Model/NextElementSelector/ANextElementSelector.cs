using System.Collections.Generic;

using Lab3.Model.Elements;

namespace Lab3.Model.NextElementSelector
{
    public abstract class NextElementSelector
    {
        protected List<(Element, double)> _nextElements;

        public NextElementSelector(List<(Element, double)> nextElements)
        {
            _nextElements = nextElements;
        }

        public abstract Element GetNextElement();
    }
}
