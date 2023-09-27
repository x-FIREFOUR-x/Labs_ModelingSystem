using System.Collections.Generic;

using Lab2.Model;
using Lab2.Model.DelayGenerator;

namespace Lab2
{ 
    class Program
    {
        public static void Main(string[] args)
        {
            Model.Model model = CreateschemeModel();
            model.Simulation(100);
        }

        private static Model.Model CreateSingleModel()
        {
            Process process = new Process("Process", null, 1, new List<SimpleProcessor> {
                new SimpleProcessor("p3", new ConstantDelayGenerator(10))
            });

            Create create = new Create("Create", process, new ConstantDelayGenerator(5));

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(process);

            return new Model.Model(elements);
        }

        private static Model.Model CreateschemeModel()
        {
            Process process3 = new Process("Process3", null, 1, new List<SimpleProcessor> {
                new SimpleProcessor("p3", new UniformDelayGenerator(7, 10))
            });

            Process process2 = new Process("Process2", process3, 2, new List<SimpleProcessor> {
                new SimpleProcessor("p2", new UniformDelayGenerator(5, 10)) 
            });

            Process process1 = new Process("Process1", process2, 1, new List<SimpleProcessor> {
                new SimpleProcessor("p1", new UniformDelayGenerator(3, 10)) 
            });

            Create create = new Create("Create", process1, new UniformDelayGenerator(3, 10));

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(process1);
            elements.Add(process2);
            elements.Add(process3);

            return new Model.Model(elements);
        }
    }
}
