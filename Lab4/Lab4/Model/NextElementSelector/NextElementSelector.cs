using System.Collections.Generic;

using Lab4.Model.Elements;
using Lab4.Model.Queue;

namespace Lab4.Model.NextElementSelector
{
    public abstract class NextElementSelector<T> where T: DefaultQueueItem
    {
        protected List<(Element<T>, double)> _nextElements;

        public NextElementSelector(List<(Element<T>, double)> nextElements)
        {
            _nextElements = nextElements;
        }

        public abstract Element<T> GetNextElement(DefaultQueueItem item);
    }
}
