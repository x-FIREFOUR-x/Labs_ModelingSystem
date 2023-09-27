using System;

using Lab2.Model.DelayGenerator;

namespace Lab2.Model
{
    public abstract class Element
    {
        public string Name { get; private set; }

        protected bool _processing;

        protected int _countProcessed;

        protected IDelayGenerator _delayGenerator;
        public double NextTime { get; protected set; }
        protected double _currentTime;

        protected Element _nextElement;

        public Element(string name, Element nextElement, IDelayGenerator delayGenerator)
        {
            Name = name;

            _currentTime = 0;

            _delayGenerator = delayGenerator;

            _nextElement = nextElement;
        }

        public virtual void StartService() { }

        public virtual void FinishService() 
        { 
            _countProcessed++;

            Console.WriteLine($"{Name}: finish, time: {_currentTime}");
        }

        public virtual void UpdatedCurrentTime(double currentTime) { _currentTime = currentTime; }

        public virtual void PrintStats(bool finalStats) { Console.WriteLine($"\n\t*{Name}"); }
    }
}
