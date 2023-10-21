using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model.Queue
{
    public class Patient : DefaultQueueItem
    {
        public int TypePatient { get; private set; }

        private double _startTime;
        private double _finishTime;

        public Patient(double startTime)
        {
            _startTime = startTime;

            Random rand = new Random();
            float numb = (float)rand.NextDouble();

            if(numb <= 0.5)
            {
                TypePatient = 1;
            }
            else if (numb <= 0.6)
            {
                TypePatient = 2;
            }
            else
            {
                TypePatient = 3;
            }
        }

        public override int GetIndexGenerator()
        {
            return TypePatient - 1;
        }

        public void Finish(double time)
        {
            _finishTime = time;
        }
    }
}
