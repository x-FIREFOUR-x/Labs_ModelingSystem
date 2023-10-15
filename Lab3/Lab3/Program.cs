using System.Collections.Generic;

using Lab3.Model;
using Lab3.Model.DelayGenerator;
using Lab3.Model.Elements;
using Lab3.Model.NextElementSelector;

namespace Lab3
{ 
    class Program
    {
        public static void Main(string[] args)
        {
            Model.Model model3 = CreateScheme();
            model3.Simulation(100, false);

            //Model.Model model3 = CreateModelCombineProcess();
            //model3.Simulation(100, false);
        }
    
        private static Model.Model CreateScheme()
        {
            Process process2 = new Process("Process2", 4, new List<Element> {
                new SimpleProcessor("p1", new ConstantDelayGenerator(4))
            });
            process2.NextElementSelector = new NextElementPrioritySelector(new List<(Element, double)> ());

            Process process1 = new Process("Process1", 4, new List<Element> {
                new SimpleProcessor("p1", new ConstantDelayGenerator(3)),
                new SimpleProcessor("p2", new ConstantDelayGenerator(3)),
            });
            process1.NextElementSelector = new NextElementPrioritySelector(new List<(Element, double)> { (process1, 1), (process2, 3) });

            Create create = new Create("Create", new ConstantDelayGenerator(2));
            create.NextElementSelector = new NextElementPrioritySelector(new List<(Element, double)> { (process1, 1.0) });

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(process1);
            elements.Add(process2);

            return new Model.Model(elements);
        }

        private static Model.Model CreateModelCombineProcess()
        {
            Process process3 = new Process("Process3", 4, new List<Element> {
                new SimpleProcessor("p1", new ConstantDelayGenerator(5))
            });
            process3.NextElementSelector = new NextElementProbabilitySelector(new List<(Element, double)>());

            Process process2 = new Process("Process2", 4, new List<Element> {
                new SimpleProcessor("p1", new ConstantDelayGenerator(4))
            });
            process2.NextElementSelector = new NextElementProbabilitySelector(new List<(Element, double)> { (process3, 1.0) });

            Process process1 = new Process("Process1", 4, new List<Element> {
                new SimpleProcessor("p1", new ConstantDelayGenerator(3)),
                new SimpleProcessor("p2", new ConstantDelayGenerator(3)),
            });
            process1.NextElementSelector = new NextElementProbabilitySelector(new List<(Element, double)> { (process2, 1.0) });

            Create create = new Create("Create", new ConstantDelayGenerator(2));
            create.NextElementSelector = new NextElementProbabilitySelector(new List<(Element, double)> { (process1, 1.0) });

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(process1);
            elements.Add(process2);
            elements.Add(process3);

            return new Model.Model(elements);
        }
    }
}
