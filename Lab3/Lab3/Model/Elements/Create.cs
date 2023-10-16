using System;

using Lab3.Model.DelayGenerator;
using Lab3.Model.Queue;

namespace Lab3.Model.Elements
{
    public class Create<T> : Element<T> where T : DefaultQueueItem
    {
        public Create(string name, IDelayGenerator delayGenerator)
           : base(name, delayGenerator)
        {
            Processing = true;
            _currentTime = 0;
            SetNextTime(_currentTime + _delayGenerator.GetDelay());
        }

        public override void StartService() => throw new NotSupportedException();

        public override void FinishService()
        {
            base.FinishService();

            Console.WriteLine($"{Name}: finish, time: {_currentTime}");

            SetNextTime(_currentTime + _delayGenerator.GetDelay());

            Element<T> nextElement = NextElementSelector.GetNextElement();
            nextElement?.StartService();
        }

        public override void PrintStats(bool finalStats)
        {
            base.PrintStats(finalStats);
            Console.WriteLine($"\t\tCreated items: {_countProcessed}");
        }
    }
}
