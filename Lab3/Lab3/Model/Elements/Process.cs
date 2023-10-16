using System;
using System.Collections.Generic;

namespace Lab3.Model.Elements
{
    class Process : Element
    {
        public int QueueSize { get; set; }
        public readonly int MaxQueueSize;

        private int _countFailures;

        private double _averageQueueDividend;

        private List<Element> _processors;

        public override bool Processing
        {
            get {
                bool isWorking = false;
                foreach (var processors in _processors)
                {
                    if (processors.Processing)
                    {
                        isWorking = true;
                        break;
                    }
                }
                base.Processing = isWorking;
                return base.Processing;
            }
            set => base.Processing = value;
        }

        public Process(string name, int maxQueueSize, List<Element> processors, int startQueueSize = 0)
            : base(name, null)
        {
            QueueSize = 0;
            MaxQueueSize = maxQueueSize;

            _processors = processors;

            SetNextTime(Double.PositiveInfinity);

            QueueSize = startQueueSize;
        }

        public override void StartService()
        {
            Console.Write(Name);
            if (TryStartService())
            {
                return;
            }

            if(QueueSize < MaxQueueSize)
            {
                Console.WriteLine($": add item in queue, time: {_currentTime}");
                QueueSize++;
                return;
            }

            Console.WriteLine($": failure, time: {_currentTime}");
            _countFailures++;
        }

        public override void FinishService()
        {
            base.FinishService();

            Element nextElement = NextElementSelector?.GetNextElement();
            nextElement?.StartService();

            foreach (var finishProcessor in _processors)
            {
                if (Math.Abs(finishProcessor.NextTime() - finishProcessor.CurrentTime) < .0001f)
                {
                    finishProcessor.FinishService();
                    Console.WriteLine($"{Name}.{finishProcessor.Name}: finish service, time: {_currentTime}");

                    if (QueueSize > 0)
                    {
                        Console.Write(Name);

                        QueueSize--;
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
            _averageQueueDividend += (currentTime - _currentTime) * QueueSize;
            base.UpdatedCurrentTime(currentTime);

            foreach (var processor in _processors)
            {
                processor.UpdatedCurrentTime(currentTime);
            }
        }

        public override void PrintStats(bool finalStats)
        {
            base.PrintStats(finalStats);

            Console.WriteLine($"\t\tQueue size: {QueueSize}");
            Console.WriteLine($"\t\tFailures: {_countFailures}");
            Console.WriteLine($"\t\tProcessed items: {_countProcessed}");

            if (finalStats)
            {
                Console.WriteLine($"\t\tAverage queue size: {_averageQueueDividend / _currentTime}");
                Console.WriteLine($"\t\tFailure probability: {(float)_countFailures / (_countFailures + _countProcessed)}");
                Console.WriteLine($"\t\tAverage workload: {_timeWorking / _currentTime}");
            }
            
        }

        private bool TryStartService()
        {
            foreach(var processor in _processors)
            {
                if(!processor.Processing)
                {
                    base.StartService();
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
