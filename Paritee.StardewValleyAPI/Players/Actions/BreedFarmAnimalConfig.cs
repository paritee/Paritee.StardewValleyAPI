using Paritee.StardewValleyAPI.FarmAnimals;
using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.Players.Actions
{
    public class BreedFarmAnimalConfig
    {
        public List<string> AvailableFarmAnimals;
        public Blue BlueFarmAnimals;
        public Data FarmAnimalData;

        public BreedFarmAnimalConfig(List<string> availableFarmAnimals, Blue blueFarmAnimals)
        {
            this.AvailableFarmAnimals = availableFarmAnimals;
            this.BlueFarmAnimals = blueFarmAnimals;
            this.FarmAnimalData = new Data();

        }
    }
}
