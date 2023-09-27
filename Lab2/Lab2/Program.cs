using System.Collections.Generic;

namespace Lab2
{
    class Program
    {
        public static void Main(string[] args)
        {
            Model model = CreateSingleModel();
            model.Simulation(20);
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
    }
}
