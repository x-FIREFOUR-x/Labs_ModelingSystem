using Lab4.Model;
using Lab4.Model.Queue;

namespace Lab4
{ 
    class Program
    {
        public static void Main(string[] args)
        {
            Model<DefaultQueueItem> model = ModelsFactory.CreateSequentialModel(20);
            model.Simulation(100, true);

            //Model < DefaultQueueItem > model = ModelsFactory.CreateBranchesModel(5, 4);
            //model.Simulation(100, true);
        }
    }
}
