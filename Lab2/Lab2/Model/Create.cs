using System;

using Lab2.Model.DelayGenerator;

namespace Lab2.Model
{
    public class Create : Element
    {
        public Create(string name, Element nextElement, IDelayGenerator delayGenerator)
           : base(name, nextElement, delayGenerator)
        {
            _currentTime = 0;
            NextTime = _currentTime + _delayGenerator.GetDelay();
        }

        public override void StartService() => throw new NotSupportedException();

        public override void FinishService()
        {
            base.FinishService();

            NextTime = _currentTime + _delayGenerator.GetDelay();
            _nextElement?.StartService();
        }

        public override void PrintStats(bool finalStats)
        {
            base.PrintStats(finalStats);
            Console.WriteLine($"\t\tCreated items: {_countProcessed}");
        }
    }
}
