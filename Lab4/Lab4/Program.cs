using System.Collections.Generic;
using System;

using Lab4.Model;
using Lab4.Model.DelayGenerator;
using Lab4.Model.Elements;
using Lab4.Model.NextElementSelector;
using Lab4.Model.Queue;

namespace Lab4
{ 
    class Program
    {
        public static void Main(string[] args)
        {
            Model<DefaultQueueItem> model = createSequentialModel(20);
            model.Simulation(100, true);

            //Model < DefaultQueueItem > model = createBranchesModel(5, 4);
            //model.Simulation(100, true);
        }

        private static Model<DefaultQueueItem> createSequentialModel(int countProcesses)
        {
            List<Element<DefaultQueueItem>> elements = new();

            Create<DefaultQueueItem> create = new Create<DefaultQueueItem>("Create", new ExponentialDelayGenerator(0.5));
            elements.Add(create);

            Element<DefaultQueueItem> prevElement = create;
            for (int i = 0; i < countProcesses; i++)
            {
                Process<DefaultQueueItem> process = new Process<DefaultQueueItem>((i + 1).ToString(), 2, new List<Element<DefaultQueueItem>> {
                    new SimpleProcessor<DefaultQueueItem>((i+1).ToString(), new ExponentialDelayGenerator(0.5))}
                );

                prevElement.NextElementSelector = new NextElementProbabilitySelector<DefaultQueueItem>(
                    new List<(Element<DefaultQueueItem>, double)> { (process, 1.0) }
                );

                elements.Add(process);
                prevElement = process;
            }
            //prevElement.NextElementSelector

            return new Model<DefaultQueueItem>(elements);
        }

        private static Model<DefaultQueueItem> createBranchesModel(int countBranches, int countProcess)
        {
            List<Element<DefaultQueueItem>> elements = new();

            Create<DefaultQueueItem> create = new Create<DefaultQueueItem>("Create", new ExponentialDelayGenerator(0.5));
            elements.Add(create);

            List<(Element<DefaultQueueItem>, double)> startBranchesElements = new List<(Element<DefaultQueueItem>, double)>();

            for (int i = 0; i < countBranches; i++)
            {
                Element<DefaultQueueItem> prevElement = null;
                for (int j = 0; j < countProcess; j++)
                {
                    string name = (i + 1).ToString() + "-" + (j + 1).ToString();
                    Process<DefaultQueueItem> process = new Process<DefaultQueueItem>(name, 2, new List<Element<DefaultQueueItem>> {
                        new SimpleProcessor<DefaultQueueItem>(name, new ExponentialDelayGenerator(0.5))}
                    );

                    if(j != 0)
                    {
                        prevElement.NextElementSelector = new NextElementProbabilitySelector<DefaultQueueItem>(
                            new List<(Element<DefaultQueueItem>, double)> { (process, 1.0) }
                        );
                    }
                    else
                    {
                        startBranchesElements.Add((process, 1.0 / countBranches));
                    }
                    
                    elements.Add(process);
                    prevElement = process;
                }
                //prevElement.NextElementSelector
            }
            create.NextElementSelector = new NextElementProbabilitySelector<DefaultQueueItem>(startBranchesElements);


            return new Model<DefaultQueueItem>(elements);
        }
    }
}
