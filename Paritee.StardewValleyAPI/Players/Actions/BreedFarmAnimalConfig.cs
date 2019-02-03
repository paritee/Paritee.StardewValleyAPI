using Paritee.StardewValleyAPI.FarmAnimals;
using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.Players.Actions
{
    public class BreedFarmAnimalConfig
    {
        public List<string> AvailableFarmAnimals;
        public BlueVariation BlueFarmAnimals;
        public FarmAnimalsData FarmAnimalData;

        public BreedFarmAnimalConfig(List<string> availableFarmAnimals, BlueVariation blueFarmAnimals)
        {
            this.AvailableFarmAnimals = availableFarmAnimals;
            this.BlueFarmAnimals = blueFarmAnimals;
            this.FarmAnimalData = new FarmAnimalsData();

        }
    }
}
