using System;

using Lab3.Model.DelayGenerator;
using Lab3.Model.Queue;


namespace Lab3.Model.Elements
{
    public class Dispose<T> : Element<T> where T : DefaultQueueItem
    {
        public Dispose(string name)
            :base(name)
        {
            _currentTime = 0;
            SetNextTime(Double.PositiveInfinity);
        }

        public override void StartService(T item) 
        {
            if(item is Patient patient)
            {
                patient.Finish(_currentTime);
            }
            _countProcessed++;
        }

        public override void FinishService()
        {
        }

        public override void PrintStats(bool finalStats)
        {
            base.PrintStats(finalStats);
            Console.WriteLine($"\t\tFinish items: {_countProcessed}");
        }
    }
}
