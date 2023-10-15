using System.Collections.Generic;

using Lab3.Model;
using Lab3.Model.DelayGenerator;
using Lab3.Model.Elements;

namespace Lab3
{ 
    class Program
    {
        public static void Main(string[] args)
        {
            //Model.Model model = CreateSingleModel();
            //model.Simulation(100, true);

            //Model.Model model1 = CreateSchemeModel();
            //model1.Simulation(100, true);

            //Model.Model model2 = CreateSchemeModel2();
            //model2.Simulation(100);

            //Model.Model model3 = CreateModelCombineProcess();
            //model3.Simulation(100);

            Model.Model model4 = CreateModelProcessSomeExits();
            model4.Simulation(100, true);
        }

        private static Model.Model CreateSingleModel()
        {
            Process process = new Process("Process", 4, new List<Element> {
                new SimpleProcessor("p3", new ConstantDelayGenerator(8))
            });
            process.NextElementSelector = new NextElementSelector(new List<(Element, double)>());

            Create create = new Create("Create", new ConstantDelayGenerator(5));
            create.NextElementSelector = new NextElementSelector(new List<(Element, double)>{ (process, 1.0) });

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(process);

            return new Model.Model(elements);
        }

        private static Model.Model CreateSchemeModel()
        {
            Process process3 = new Process("Process3", 1, new List<Element> {
                new SimpleProcessor("p3", new UniformDelayGenerator(7, 10))
            });
            process3.NextElementSelector = new NextElementSelector(new List<(Element, double)>());

            Process process2 = new Process("Process2", 2, new List<Element> {
                new SimpleProcessor("p2", new UniformDelayGenerator(5, 10)) 
            });
            process2.NextElementSelector = new NextElementSelector(new List<(Element, double)> { (process3, 1.0) });

            Process process1 = new Process("Process1", 1, new List<Element> {
                new SimpleProcessor("p1", new UniformDelayGenerator(3, 10)) 
            });
            process1.NextElementSelector = new NextElementSelector(new List<(Element, double)> { (process2, 1.0) });

            Create create = new Create("Create", new UniformDelayGenerator(3, 10));
            create.NextElementSelector = new NextElementSelector(new List<(Element, double)> { (process1, 1.0) });

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(process1);
            elements.Add(process2);
            elements.Add(process3);

            return new Model.Model(elements);
        }

        private static Model.Model CreateSchemeModel2()
        {
            Process process3 = new Process("Process3", 4, new List<Element> {
                new SimpleProcessor("p3", new ConstantDelayGenerator(5))
            });
            process3.NextElementSelector = new NextElementSelector(new List<(Element, double)>());

            Process process2 = new Process("Process2", 4, new List<Element> {
                new SimpleProcessor("p2", new ConstantDelayGenerator(4))
            });
            process2.NextElementSelector = new NextElementSelector(new List<(Element, double)> { (process3, 1.0) });

            Process process1 = new Process("Process1", 4, new List<Element> {
                new SimpleProcessor("p1", new ConstantDelayGenerator(3))
            });
            process1.NextElementSelector = new NextElementSelector(new List<(Element, double)> { (process2, 1.0) });

            Create create = new Create("Create", new ConstantDelayGenerator(2));
            create.NextElementSelector = new NextElementSelector(new List<(Element, double)> { (process1, 1.0) });

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(process1);
            elements.Add(process2);
            elements.Add(process3);

            return new Model.Model(elements);
        }
    

        private static Model.Model CreateModelCombineProcess()
        {
            Process process3 = new Process("Process3", 4, new List<Element> {
                new SimpleProcessor("p1", new ConstantDelayGenerator(5))
            });
            process3.NextElementSelector = new NextElementSelector(new List<(Element, double)>());

            Process process2 = new Process("Process2", 4, new List<Element> {
                new SimpleProcessor("p1", new ConstantDelayGenerator(4))
            });
            process2.NextElementSelector = new NextElementSelector(new List<(Element, double)> { (process3, 1.0) });

            Process process1 = new Process("Process1", 4, new List<Element> {
                new SimpleProcessor("p1", new ConstantDelayGenerator(3)),
                new SimpleProcessor("p2", new ConstantDelayGenerator(3)),
            });
            process1.NextElementSelector = new NextElementSelector(new List<(Element, double)> { (process2, 1.0) });

            Create create = new Create("Create", new ConstantDelayGenerator(2));
            create.NextElementSelector = new NextElementSelector(new List<(Element, double)> { (process1, 1.0) });

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(process1);
            elements.Add(process2);
            elements.Add(process3);

            return new Model.Model(elements);
        }

        private static Model.Model CreateModelProcessSomeExits()
        {

            Process process2 = new Process("Process2", 1, new List<Element> {
                new SimpleProcessor("p2", new ConstantDelayGenerator(4))
            });
            process2.NextElementSelector = new NextElementSelector(new List<(Element, double)> ());

            Process process1 = new Process("Process1", 1, new List<Element> {
                new SimpleProcessor("p1", new ConstantDelayGenerator(3)),
            });
            process1.NextElementSelector = new NextElementSelector(new List<(Element, double)> { (process1, 0.25), (process2, 0.75) });

            Create create = new Create("Create", new ConstantDelayGenerator(2));
            create.NextElementSelector = new NextElementSelector(new List<(Element, double)> { (process1, 1.0) });

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(process1);
            elements.Add(process2);

            return new Model.Model(elements);
        }
    }
}
