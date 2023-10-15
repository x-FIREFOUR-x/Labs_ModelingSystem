using System;

using Lab3.Model.DelayGenerator;

namespace Lab3.Model.Elements
{
    public class SimpleProcessor: Element
    {
        public SimpleProcessor(string name, IDelayGenerator delayGenerator)
            :base(name, delayGenerator)
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
