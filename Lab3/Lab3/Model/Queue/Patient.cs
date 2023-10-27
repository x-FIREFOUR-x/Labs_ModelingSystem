using System;

namespace Lab3.Model.Queue
{
    public class Patient : DefaultQueueItem
    {
        public int TypePatient { get; set; }
        public int StartTypePatient { get; private set; }

        private double _startTime;
        private double _finishTime;

        public Patient(double startTime)
        {
            _startTime = startTime;
            _finishTime = double.NaN;

            Random rand = new Random();
            float numb = (float)rand.NextDouble();

            int type;
            if(numb <= 0.5)
            {
                type = 1;
            }
            else if (numb <= 0.6)
            {
                type = 2;
            }
            else
            {
                type = 3;
            }

            StartTypePatient = type;
            TypePatient = type;
        }

        public override int GetIndexGenerator()
        {
            return TypePatient - 1;
        }

        public void Finish(double time)
        {
            _finishTime = time;
        }

        public override void PrintStats()
        {
            Console.WriteLine($"\t\t\t{StartTypePatient}       {Math.Round(_startTime, 2)}      {Math.Round(_finishTime, 2)}");
        }
    }
}
