using System.Collections.Generic;
using System;

using Lab3.Model;
using Lab3.Model.DelayGenerator;
using Lab3.Model.Elements;
using Lab3.Model.NextElementSelector;
using Lab3.Model.Queue;

namespace Lab3
{ 
    class Program
    {
        public static void Main(string[] args)
        {
            //Model<DefaultQueueItem> modelBank = CreateBankModel();
            //modelBank.Simulation(100, false);

            Model<Patient> modelHospital = CreateHospitalModel();
            modelHospital.Simulation(1000, false);
        }

        private static Model<DefaultQueueItem> CreateBankModel()
        {
            Process<DefaultQueueItem> cashier1 = new Process<DefaultQueueItem>("Cashier1", 3, new List<Element<DefaultQueueItem>> {
                new SimpleProcessor<DefaultQueueItem>("cashier", new NormalDelayGenerator(0.3, 1), new ExponentialDelayGenerator(0.3)),
            }, 2);

            Process<DefaultQueueItem> cashier2 = new Process<DefaultQueueItem>("Cashier2", 3, new List<Element<DefaultQueueItem>> {
                new SimpleProcessor<DefaultQueueItem>("cashier", new NormalDelayGenerator(0.3, 1), new ExponentialDelayGenerator(0.3)),
            }, 2);

            Create<DefaultQueueItem> create = new Create<DefaultQueueItem>("Create", new ExponentialDelayGenerator(0.5));
            create.NextElementSelector = new NextElementQueuePrioritySelector<DefaultQueueItem>(
                new List<(Element<DefaultQueueItem>, double)> { (cashier1, 2), (cashier2, 1)}
                );
            create.SetNextTime(0.1);

            List<Element<DefaultQueueItem>> elements = new();
            elements.Add(create);
            elements.Add(cashier1);
            elements.Add(cashier2);

            Action<List<Element<DefaultQueueItem>>> ActionChangeQueue = (elements) =>
            {
                foreach (var element in elements)
                {
                    if (element is Process<DefaultQueueItem> process1 && process1.QueueSize == process1.QueueMaxSize)
                    {
                        foreach (var element2 in elements)
                        {
                            if (element2 is Process<DefaultQueueItem> process2 && process1.QueueSize - process2.QueueSize >= 2)
                            {
                                DefaultQueueItem item = process1.GetItemWithQueue();

                                if (process2.Processing)
                                {
                                    process2.PutItemToQueue(item);
                                }
                                else
                                {
                                    process2.StartService(item);
                                }
                            }
                        }
                    }
                }
            };
            
            return new Model<DefaultQueueItem>(elements, ActionChangeQueue);
        }
        
        
        private static Model<Patient> CreateHospitalModel()
        {
            Process<Patient> receptionDepartment = new Process<Patient>("ReceptionDepartment", 100, new List<Element<Patient>> {
                new SimpleProcessor<Patient>("Doctor1", new List<IDelayGenerator>{
                                                            new ExponentialDelayGenerator(15),
                                                            new ExponentialDelayGenerator(40),
                                                            new ExponentialDelayGenerator(30)}),
                new SimpleProcessor<Patient>("Doctor2", new List<IDelayGenerator>{
                                                            new ExponentialDelayGenerator(15),
                                                            new ExponentialDelayGenerator(40),
                                                            new ExponentialDelayGenerator(30)}),
            });

            Process<Patient> wards = new Process<Patient>("Wards", 100, new List<Element<Patient>> {
                new SimpleProcessor<Patient>("Accompanying1", new UniformDelayGenerator(3, 8)),
                new SimpleProcessor<Patient>("Accompanying2", new UniformDelayGenerator(3, 8)),
                new SimpleProcessor<Patient>("Accompanying3", new UniformDelayGenerator(3, 8)),
            });

            Process<Patient> pathToLab = new Process<Patient>("PathToLab", 0, new List<Element<Patient>> {
                new SimpleProcessor<Patient>("Go1", new UniformDelayGenerator(2, 5)),
                new SimpleProcessor<Patient>("Go2", new UniformDelayGenerator(2, 5)),
                new SimpleProcessor<Patient>("Go3", new UniformDelayGenerator(2, 5)),
                new SimpleProcessor<Patient>("Go4", new UniformDelayGenerator(2, 5)),
                new SimpleProcessor<Patient>("Go5", new UniformDelayGenerator(2, 5)),
                new SimpleProcessor<Patient>("Go6", new UniformDelayGenerator(2, 5)),
                new SimpleProcessor<Patient>("Go7", new UniformDelayGenerator(2, 5)),
            });

            Process<Patient> registryLab = new Process<Patient>("RegistryLab", 100, new List<Element<Patient>> {
                new SimpleProcessor<Patient>("register", new ErlangDelayGenerator(4.5, 3)),
            });


            Action<Patient> ActionChangeTypePatientAfterLab = (item) =>
            {
                if (item.TypePatient == 2)
                    item.TypePatient = 1;
            };
            Process<Patient> lab = new Process<Patient>("Laboratory", 100, new List<Element<Patient>> {
                new SimpleProcessor<Patient>("lab1", new ErlangDelayGenerator(4, 2), ActionChangeTypePatientAfterLab),
                new SimpleProcessor<Patient>("lab2", new ErlangDelayGenerator(4, 2), ActionChangeTypePatientAfterLab)
            });
            lab.PrintTimesIncome = true;

            Dispose<Patient> dispose = new Dispose<Patient>("Exit");

            receptionDepartment.NextElementSelector = new NextElementItemTypeSelector<Patient>(
                new List<(Element<Patient>, double)> { (wards, 1), (pathToLab, 2), (pathToLab, 3) });

            wards.NextElementSelector = new NextElementProbabilitySelector<Patient> (
                new List<(Element<Patient>, double)> { (dispose, 1), });

            pathToLab.NextElementSelector = new NextElementProbabilitySelector<Patient>(
                new List<(Element<Patient>, double)> { (registryLab, 1.0) });

            registryLab.NextElementSelector = new NextElementProbabilitySelector<Patient>(
                new List<(Element<Patient>, double)> { (lab, 1.0) });

            lab.NextElementSelector = new NextElementItemTypeSelector<Patient>(
                new List<(Element<Patient>, double)> { (receptionDepartment, 1), (dispose, 3) });

            Create<Patient> create = new Create<Patient>("Create", new ExponentialDelayGenerator(15));
            create.NextElementSelector = new NextElementPrioritySelector<Patient>(new List<(Element<Patient>, double)>{(receptionDepartment, 1)});

            List <Element<Patient>> elements = new();
            elements.Add(create);
            elements.Add(receptionDepartment);
            elements.Add(wards);
            elements.Add(pathToLab);
            elements.Add(registryLab);
            elements.Add(lab);
            elements.Add(dispose);
            
            return new Model<Patient>(elements);
        }
        
    }
}
