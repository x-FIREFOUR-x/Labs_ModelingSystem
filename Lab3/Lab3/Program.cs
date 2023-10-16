using System.Collections.Generic;
using System;

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
            Model.Model model3 = CreateBankModel();
            model3.Simulation(100, false);

            //Model.Model model3 = CreateScheme2();
            //model3.Simulation(100, true);

            //Model.Model model3 = CreateModelCombineProcess();
            //model3.Simulation(100, false);
        }

        private static Model.Model CreateBankModel()
        {
            Process cashier1 = new Process("Cashier1", 3, new List<Element> {
                new SimpleProcessor("cashier", new NormalDelayGenerator(0.3, 1), new ExponentialDelayGenerator(0.3)),
            }, 2);

            Process cashier2 = new Process("Cashier2", 3, new List<Element> {
                new SimpleProcessor("cashier", new NormalDelayGenerator(0.3, 1), new ExponentialDelayGenerator(0.3)),
            }, 2);

            Create create = new Create("Create", new ExponentialDelayGenerator(0.5));
            create.NextElementSelector = new NextElementQueuePrioritySelector(new List<(Element, double)> { (cashier1, 2), (cashier2, 1)});
            create.SetNextTime(0.1);

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(cashier1);
            elements.Add(cashier2);

            Action<List<Element>> ActionChangeQueue = (elements) =>
            {
                foreach (var element in elements)
                {
                    if (element is Process process1 && process1.QueueSize == process1.MaxQueueSize)
                    {
                        foreach (var element2 in elements)
                        {
                            if (element2 is Process process2 && process1.QueueSize - process2.QueueSize >= 2)
                            {
                                process1.QueueSize--;

                                if (process2.Processing)
                                {
                                    process2.QueueSize++;
                                }
                                else
                                {
                                    process2.StartService();
                                }
                            }
                            
                        }
                    }
                }
            };

            return new Model.Model(elements, ActionChangeQueue);
        }

        private static Model.Model CreateScheme()
        {
            Process process1 = new Process("Process1", 4, new List<Element> {
                new SimpleProcessor("p1", new ConstantDelayGenerator(5)),
            });
            process1.NextElementSelector = new NextElementPrioritySelector(new List<(Element, double)> ());

            Create create = new Create("Create", new ConstantDelayGenerator(5));
            create.NextElementSelector = new NextElementPrioritySelector(new List<(Element, double)> { (process1, 1.0) });

            List<Element> elements = new();
            elements.Add(create);
            elements.Add(process1);

            return new Model.Model(elements);
        }


        private static Model.Model CreateScheme2()
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
