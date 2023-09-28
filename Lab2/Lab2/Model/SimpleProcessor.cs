using System;

using Lab2.Model.DelayGenerator;

namespace Lab2.Model
{
    public class SimpleProcessor: Element
    {
        public SimpleProcessor(string name, IDelayGenerator delayGenerator)
            :base(name, null, delayGenerator)
        {
            Processing = false;

            SetNextTime(Double.PositiveInfinity);
        }

        public override void StartService()
        {
            Console.WriteLine($".{Name}: start service, time: {_currentTime}");
            Processing = true;
            SetNextTime(_currentTime + _delayGenerator.GetDelay());
        }

        public override void FinishService()
        {
            base.FinishService();
            Processing = false;
        }
    }
}
