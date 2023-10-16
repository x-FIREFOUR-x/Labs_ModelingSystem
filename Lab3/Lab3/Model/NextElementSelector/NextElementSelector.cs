using System.Collections.Generic;

using Lab3.Model.Elements;
using Lab3.Model.Queue;

namespace Lab3.Model.NextElementSelector
{
    public abstract class NextElementSelector<T> where T: DefaultQueueItem
    {
        protected List<(Element<T>, double)> _nextElements;

        public NextElementSelector(List<(Element<T>, double)> nextElements)
        {
            _nextElements = nextElements;
        }

        public abstract Element<T> GetNextElement();
    }
}
