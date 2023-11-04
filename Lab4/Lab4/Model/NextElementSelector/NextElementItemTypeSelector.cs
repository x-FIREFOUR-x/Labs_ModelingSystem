using System.Collections.Generic;
using System.Linq;

using Lab4.Model.Elements;
using Lab4.Model.Queue;

namespace Lab4.Model.NextElementSelector
{
    public class NextElementItemTypeSelector<T> : NextElementSelector<T> where T : DefaultQueueItem
    {
        public NextElementItemTypeSelector(List<(Element<T>, double)> nextElements)
           : base(nextElements)
        {
        }

        public override Element<T> GetNextElement(DefaultQueueItem item)
        {
            if(item is Patient patient)
            {
                return _nextElements.FirstOrDefault(c => (int)(c.Item2) == patient.TypePatient).Item1;
            }  

            return null;
        }
    }
}
