using System;
using System.Collections.Generic;

using Lab3.Model.Queue;

namespace Lab3.Model.Elements
{
    public class Dispose<T> : Element<T> where T : DefaultQueueItem
    {
        private List<T> _finishItems;

        public Dispose(string name)
            :base(name)
        {
            _currentTime = 0;
            SetNextTime(Double.PositiveInfinity);

            _finishItems = new List<T>();
        }

        public override void StartService(T item) 
        {
            if(item is Patient patient)
            {
                patient.Finish(_currentTime);
            }
            _countProcessed++;
            _finishItems.Add(item);
        }

        public override void FinishService()
        {
        }

        public override void PrintStats(bool finalStats)
        {
            base.PrintStats(finalStats);
            Console.WriteLine($"\t\tFinished items: {_countProcessed}");

            if(finalStats)
            {
                if (typeof(T) == typeof(Patient))
                {
                    Console.WriteLine("\t\tPatients Type   StartTime   FinishTime");
                    foreach (var item in _finishItems)
                    {
                        item.PrintStats();
                    }
                }
            }
        }
    }
}
