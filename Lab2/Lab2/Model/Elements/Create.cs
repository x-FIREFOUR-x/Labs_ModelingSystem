using System;

using Lab2.Model.DelayGenerator;

namespace Lab2.Model.Elemets
{
    public class Create : Element
    {
        public Create(string name, Element nextElement, IDelayGenerator delayGenerator)
           : base(name, nextElement, delayGenerator)
        {
            _currentTime = 0;
            SetNextTime(_currentTime + _delayGenerator.GetDelay());
        }

        public override void StartService() => throw new NotSupportedException();

        public override void FinishService()
        {
            base.FinishService();

            Console.WriteLine($"{Name}: finish, time: {_currentTime}");

            SetNextTime(_currentTime + _delayGenerator.GetDelay());
            _nextElement?.StartService();
        }

        public override void PrintStats(bool finalStats)
        {
            base.PrintStats(finalStats);
            Console.WriteLine($"\t\tCreated items: {_countProcessed}");
        }

        public override void UpdatedCurrentTime(double currentTime) { _currentTime = currentTime; }
    }
}
