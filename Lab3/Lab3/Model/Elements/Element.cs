using System;

using Lab3.Model.DelayGenerator;
using Lab3.Model.Queue;

namespace Lab3.Model.Elements
{
    public abstract class Element<T> where T : DefaultQueueItem
    {
        public string Name { get; private set; }

        public virtual bool Processing { get; set; }

        protected int _countProcessed;

        protected IDelayGenerator _delayGenerator;
        private double _nextTime;
        protected double _currentTime;

        protected double _timeWorking;

        public NextElementSelector.NextElementSelector<T> NextElementSelector { protected get; set; }

        public virtual double NextTime() {return _nextTime;}
        
        public void SetNextTime(double nextTime) => _nextTime = nextTime;
        
        public virtual double CurrentTime => _currentTime; 
       

        public Element(string name, IDelayGenerator delayGenerator)
        {
            Name = name;

            _currentTime = 0;

            _delayGenerator = delayGenerator;
        }

        public virtual void StartService() { Processing = true; }

        public virtual void FinishService() 
        { 
            _countProcessed++;
        }

        public virtual bool TryFinish() 
        {
            if (Math.Abs(_nextTime - _currentTime) < .0001f)
            {
                return true;
            }

            return false;
        }

        public virtual void UpdatedCurrentTime(double currentTime) {
            if(Processing)
            {
                _timeWorking += currentTime - _currentTime;
            }
            
            _currentTime = currentTime; 
        }

        public virtual void PrintStats(bool finalStats) {
            Console.WriteLine($"\t*{Name}");

            //Console.WriteLine($"\t\tAverage workload: {_timeWorking / _currentTime}");
        }
    }
}
