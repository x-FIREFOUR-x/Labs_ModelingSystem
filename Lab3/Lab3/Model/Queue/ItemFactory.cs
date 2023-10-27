using System;

namespace Lab3.Model.Queue
{
    public class ItemFactory<T> where T: DefaultQueueItem
    {
        public T CreateItem(double currentTime)
        {
            if (typeof(T) == typeof(Patient))
            {
                DefaultQueueItem patient = new Patient(currentTime);
                return (T)patient;
            }
            else if (typeof(T) == typeof(DefaultQueueItem))
            {
                return null;
            }
            else
            {
                throw new Exception($"Not realize for type{typeof(T)}");
            }
        }

        
    }
}
