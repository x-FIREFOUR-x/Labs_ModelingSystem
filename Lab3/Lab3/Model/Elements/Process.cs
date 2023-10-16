using System;
using System.Collections.Generic;

using Lab3.Model.Queue;

namespace Lab3.Model.Elements
{
    class Process<T>: Element<T> where T : DefaultQueueItem
    {
        private ProcessQueue<T> _queue;

        private int _countFailures;

        private double _averageQueueDividend;

        private List<Element<T>> _processors;

        public int QueueSize { get => _queue.Size; }
        public int QueueMaxSize { get => _queue.MaxSize; }

        public T GetItemWithQueue() { return _queue.GetItem(); }
        public void PutItemToQueue(T item) { _queue.PutItem(item); }

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

        public Process(string name, int maxQueueSize, List<Element<T>> processors, int startQueueSize = 0)
            : base(name, null)
        {
            _processors = processors;
            SetNextTime(Double.PositiveInfinity);

            if(startQueueSize == 0)
            {
                _queue = new ProcessQueue<T>(maxQueueSize);
            }
            else
            {
                _queue = new ProcessQueue<T>(maxQueueSize, startQueueSize);
            }
        }

        public override void StartService()
        {
            Console.Write(Name);
            if (TryStartService())
            {
                return;
            }
            
            if (QueueSize < QueueMaxSize)
            {
                Console.WriteLine($": add item in queue, time: {_currentTime}");

                _queue.PutItem(null);
                return;
            }

            Console.WriteLine($": failure, time: {_currentTime}");
            _countFailures++;
        }

        public override void FinishService()
        {
            base.FinishService();

            Element<T> nextElement = NextElementSelector?.GetNextElement();
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
                        _queue.GetItem();
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
            _averageQueueDividend += (currentTime - _currentTime) * _queue.Size;
            base.UpdatedCurrentTime(currentTime);

            foreach (var processor in _processors)
            {
                processor.UpdatedCurrentTime(currentTime);
            }
        }

        public override void PrintStats(bool finalStats)
        {
            base.PrintStats(finalStats);

            Console.WriteLine($"\t\tQueue size: {_queue.Size}");
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
