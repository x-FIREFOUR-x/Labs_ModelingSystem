using System;
using System.Collections.Generic;

namespace Lab2.Model.Elements
{
    class Process : Element
    {
        private int _queueSize;
        private readonly int _maxQueueSize;

        private int _countFailures;

        private double _averageQueueDividend;

        private List<Element> _processors;


        public Process(string name, int maxQueueSize, List<Element> processors)
            : base(name, null)
        {
            _queueSize = 0;
            _maxQueueSize = maxQueueSize;

            _processors = processors;

            SetNextTime(Double.PositiveInfinity);
        }

        public override void StartService()
        {
            Console.Write(Name);
            if (TryStartService())
            {
                return;
            }

            if(_queueSize < _maxQueueSize)
            {
                Console.WriteLine($": add item in queue, time: {_currentTime}");
                _queueSize++;
                return;
            }

            Console.WriteLine($": failure, time: {_currentTime}");
            _countFailures++;
        }

        public override void FinishService()
        {
            base.FinishService();

            Element nextElement = NextElementSelector.GetNextElement();
            nextElement?.StartService();

            foreach (var finishProcessor in _processors)
            {
                if (Math.Abs(finishProcessor.NextTime() - finishProcessor.CurrentTime) < .0001f)
                {
                    finishProcessor.FinishService();
                    Console.WriteLine($"{Name}.{finishProcessor.Name}: finish service, time: {_currentTime}");

                    if (_queueSize > 0)
                    {
                        Console.Write(Name);

                        _queueSize--;
                        finishProcessor.StartService();
                    }
                    else
                    {
                        finishProcessor.SetNextTime(Double.PositiveInfinity);
                    }
                }
            }
        }

        public override void UpdatedCurrentTime(double currentTime)
        {
            _averageQueueDividend += (currentTime - _currentTime) * _queueSize;
            base.UpdatedCurrentTime(currentTime);

            foreach (var processor in _processors)
            {
                processor.UpdatedCurrentTime(currentTime);
            }
        }

        public override void PrintStats(bool finalStats)
        {
            base.PrintStats(finalStats);

            Console.WriteLine($"\t\tQueue size: {_queueSize}");
            Console.WriteLine($"\t\tFailures: {_countFailures}");
            Console.WriteLine($"\t\tProcessed items: {_countProcessed}");

            if (finalStats)
            {
                Console.WriteLine($"\t\tAverage queue size: {_averageQueueDividend / _currentTime}");
                Console.WriteLine($"\t\tFailure probability: {(float)_countFailures / (_countFailures + _countProcessed)}");
            }
            
        }

        private bool TryStartService()
        {
            foreach(var processor in _processors)
            {
                if(!processor.Processing)
                {
                    processor.StartService();

                    return true;
                }
            }

            return false;
        }

        public override bool TryFinish()
        {
            foreach (var processor in _processors)
            {
                if (Math.Abs(processor.NextTime() - processor.CurrentTime) < .0001f)
                {
                    return true;
                }
            }

            return false;
        }

        public override double NextTime()
        {
            double time = Double.PositiveInfinity;
            foreach (var processor in _processors)
            {
                if (time > processor.NextTime())
                {
                    time = processor.NextTime();
                }
            }

            return time;
        }
    }
}
