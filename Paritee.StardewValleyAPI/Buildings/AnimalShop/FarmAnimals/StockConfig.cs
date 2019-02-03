using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.Buildings.AnimalShop.FarmAnimals
{
    public class StockConfig
    {
        public List<FarmAnimalForPurchase> FarmAnimalsForPurchase;
        public BlueVariation BlueFarmAnimals;
        public VoidVariation VoidFarmAnimals;

        public StockConfig(List<FarmAnimalForPurchase> farmAnimalsForPurchase, BlueVariation blueFarmAnimals, VoidVariation voidFarmAnimals)
        {
            this.FarmAnimalsForPurchase = farmAnimalsForPurchase;
            this.BlueFarmAnimals = blueFarmAnimals;
            this.VoidFarmAnimals = voidFarmAnimals;
        }
    }
}
