using System;

using Lab3.Model.DelayGenerator;
using Lab3.Model.Queue;

namespace Lab3.Model.Elements
{
    public class Create<T> : Element<T> where T : DefaultQueueItem
    {
        private ItemFactory<T> _itemFactory;

        public Create(string name, IDelayGenerator delayGenerator)
           : base(name, delayGenerator)
        {
            Processing = true;
            _currentTime = 0;
            SetNextTime(_currentTime + _delayGenerators[0].GetDelay());

            _itemFactory = new ItemFactory<T>();
        }

        public override void StartService(T item) => throw new NotSupportedException();

        public override void FinishService()
        {
            base.FinishService();

            Console.WriteLine($"{Name}: finish, time: {_currentTime}");

            SetNextTime(_currentTime + _delayGenerators[0].GetDelay());

            T item = _itemFactory.CreateItem(_currentTime);

            Element<T> nextElement = NextElementSelector.GetNextElement(item);
            nextElement?.StartService(_itemFactory.CreateItem(_currentTime));
        }

        public override void PrintStats(bool finalStats)
        {
            base.PrintStats(finalStats);
            Console.WriteLine($"\t\tCreated items: {_countProcessed}");
        }
    }
}
