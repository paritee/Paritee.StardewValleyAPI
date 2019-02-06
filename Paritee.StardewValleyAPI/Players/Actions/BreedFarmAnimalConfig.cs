using Paritee.StardewValleyAPI.FarmAnimals;
using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.Players.Actions
{
    public class BreedFarmAnimalConfig
    {
        public Dictionary<string, List<string>> AvailableFarmAnimals;
        public BlueVariation BlueFarmAnimals;
        public FarmAnimalsData FarmAnimalsData;
        public bool RandomizeNewbornFromCategory;
        public bool RandomizeHatchlingFromCategory;
        public bool IgnoreParentProduceCheck;

        public BreedFarmAnimalConfig(Dictionary<string, List<string>> availableFarmAnimals, BlueVariation blueFarmAnimals, bool randomizeNewbornFromCategory = false, bool randomizeHatchlingFromCategory = false, bool ignoreParentProduceCheck = false)
        {
            this.AvailableFarmAnimals = availableFarmAnimals;
            this.BlueFarmAnimals = blueFarmAnimals;
            this.FarmAnimalsData = new FarmAnimalsData();

            // Default these to false for backwards compatibility
            this.RandomizeNewbornFromCategory = randomizeNewbornFromCategory;
            this.RandomizeHatchlingFromCategory = randomizeHatchlingFromCategory;
            this.IgnoreParentProduceCheck = ignoreParentProduceCheck;
        }
    }
}
