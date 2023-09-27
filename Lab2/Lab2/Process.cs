using System;

namespace Lab2
{
    class Process : Element
    {
        private int _queueSize;
        private readonly int _maxQueueSize;

        private int _countFailures;

        

        public Process(string name, Element nextElement, double averageDelay, int maxQueueSize = 0, Distribution distribution = Distribution.Constant)
            : base(name, nextElement, averageDelay, distribution)
        {
            _queueSize = 0;
            _maxQueueSize = maxQueueSize;

            _averageDelay = averageDelay;

            _processing = false;

            NextTime = Double.PositiveInfinity;
        }

        public override void StartService()
        {
            if (!_processing)
            {
                _processing = true;
                NextTime = _currentTime + _delayGenerator.GetDelay(_averageDelay);

                Console.WriteLine($"{Name}: start service, time: {_currentTime}");
                return;
            }

            if(_queueSize < _maxQueueSize)
            {
                Console.WriteLine($"{Name}: add item in queue, time: {_currentTime}");
                _queueSize++;
                return;
            }

            Console.WriteLine($"{Name}: failure, time: {_currentTime}");
            _countFailures++;
        }


        public override void FinishService()
        {
            base.FinishService();

            _processing = false;

            _nextElement?.StartService();

            if(_queueSize > 0)
            {
                _queueSize--;
                _processing = true;
                NextTime = _currentTime + _delayGenerator.GetDelay(_averageDelay);
            }
            else
            {
                NextTime = Double.PositiveInfinity;
            }
        }
    }
}
