using System.Collections.Generic;

namespace Lab2
{
    class Program
    {
        public static void Main(string[] args)
        {
            Model model = CreateschemeModel();
            model.Simulation(100);
        }

        private static Model CreateSingleModel()
        {
            Process process = new Process("Process", null, 10, 1);

            Create create = new Create("Create", process, 5);

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(process);

            return new Model(elements);
        }

        private static Model CreateschemeModel()
        {
            Process process3 = new Process("Process3", null, 5, 1);

            Process process2 = new Process("Process2", process3, 10, 2);

            Process process1 = new Process("Process1", process2, 8, 1);

            Create create = new Create("Create", process1, 5);

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(process1);
            elements.Add(process2);
            elements.Add(process3);

            return new Model(elements);
        }
    }
}
