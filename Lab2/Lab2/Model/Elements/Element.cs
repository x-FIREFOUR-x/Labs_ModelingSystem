using System;

using Lab2.Model.DelayGenerator;

namespace Lab2.Model.Elements
{
    public abstract class Element
    {
        public string Name { get; private set; }

        public bool Processing { get; set; }

        protected int _countProcessed;

        protected IDelayGenerator _delayGenerator;
        private double _nextTime;
        protected double _currentTime;

        public NextElementSelector NextElementSelector { protected get; set; }

        public virtual double NextTime() {return _nextTime;}
        
        public void SetNextTime(double nextTime) => _nextTime = nextTime;
        
        public virtual double CurrentTime => _currentTime; 
       

        public Element(string name, IDelayGenerator delayGenerator)
        {
            Name = name;

            _currentTime = 0;

            _delayGenerator = delayGenerator;
        }

        public virtual void StartService() { }

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

        public virtual void UpdatedCurrentTime(double currentTime) { _currentTime = currentTime; }

        public virtual void PrintStats(bool finalStats) { Console.WriteLine($"\t*{Name}"); }
    }
}
