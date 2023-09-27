using System;

namespace Lab2
{
    public class Create : Element
    {
        public Create(string name, Element nextElement, double averageDelay, Distribution distribution = Distribution.Constant)
           : base(name, nextElement, averageDelay, distribution)
        {
            _averageDelay = averageDelay;

            _currentTime = 0;
            NextTime = _currentTime + _delayGenerator.GetDelay(_averageDelay);
        }

        public override void StartService() => throw new NotSupportedException();

        public override void FinishService()
        {
            base.FinishService();

            NextTime = _currentTime + _delayGenerator.GetDelay(_averageDelay);
            _nextElement?.StartService();
        }
    }
}
