using System;

using Lab3.Model.DelayGenerator;
using Lab3.Model.Queue;

namespace Lab3.Model.Elements
{
    public class SimpleProcessor<T> : Element<T> where T : DefaultQueueItem
    {
        public SimpleProcessor(string name, IDelayGenerator delayGenerator, IDelayGenerator nextDelayGenerator = null)
            :base(name, delayGenerator)
        {
            Processing = false;
            SetNextTime(Double.PositiveInfinity);

            if (nextDelayGenerator != null)
            {
                Processing = true;
                SetNextTime(_delayGenerator.GetDelay());
                _delayGenerator = nextDelayGenerator;
            }
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

        public void SetStartConditions(IDelayGenerator newDelayGenerator)
        {
            Processing = true;
            SetNextTime(_delayGenerator.GetDelay());
            _delayGenerator = newDelayGenerator;
        }
    }
}
