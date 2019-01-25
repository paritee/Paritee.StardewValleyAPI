using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.Buidlings.AnimalShop.FarmAnimals
{
    public class StockConfig
    {
        public Dictionary<Stock.Name, string[]> Available;
        public Blue BlueFarmAnimals;
        public Void VoidFarmAnimals;

        public StockConfig(Dictionary<Stock.Name, string[]> available, Blue blueFarmAnimals, Void voidFarmAnimals)
        {
            this.Available = available;
            this.BlueFarmAnimals = blueFarmAnimals;
            this.VoidFarmAnimals = voidFarmAnimals;
        }
    }
}
