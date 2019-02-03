using Paritee.StardewValleyAPI.Buildings.AnimalHouses;
using Paritee.StardewValleyAPI.FarmAnimals;
using Paritee.StardewValleyAPI.FarmAnimals.Buildings.AnimalHouses;
using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using StardewValley;
using StardewValley.Events;
using System.Collections.Generic;
using System.Linq;

namespace Paritee.StardewValleyAPI.Players.Actions
{
    public class BreedFarmAnimal
    {
        public enum NamingEvent
        {
            None,
            Birthed,
            Hatched
        }

        public Player Farmer;
        private List<string> AvailableFarmAnimals;

        private BlueVariation BlueFarmAnimals;
        private FarmAnimalsData FarmAnimalData;

        public BreedFarmAnimal(Player farmer, BreedFarmAnimalConfig breedFarmAnimalConfig)
        {
            this.Farmer = farmer;
            this.AvailableFarmAnimals = breedFarmAnimalConfig.AvailableFarmAnimals;
            this.BlueFarmAnimals = breedFarmAnimalConfig.BlueFarmAnimals;
            this.FarmAnimalData = breedFarmAnimalConfig.FarmAnimalData;
        }

        private List<string> Sanitize(List<string> types)
        {
            // Remove anything that isn't available via the config
            types.RemoveAll(item => !this.IsAvailable(item));

            // Remove Blue <FarmAnimal>s if we need to
            types = this.BlueFarmAnimals.Sanitize(types, Variation.SAFETY_SAFE);

            return types;
        }

        public FarmAnimals.FarmAnimal CreateFromParent(StardewValley.FarmAnimal parent, string name)
        {
            // @TODO: Randomize based on farm animal data/category (ex. Dairy Cow)?
            return this.CreateBaby(name, parent.type, (StardewValley.AnimalHouse)parent.home.indoors, parent.myID);
        }

        public void CreateFromIncubator(StardewValley.AnimalHouse animalHouse, string name)
        {
            Incubator incubator;

            try
            {
                incubator = new Incubator(animalHouse);
            }
            catch
            {
                // Could not find an incubator; do nothing.
                return;
            }

            List<string> types = this.DetermineHatchlingTypes(incubator);

            // Remove types that should not be here
            types = this.Sanitize(types);

            // Create the animal if we could pull the type
            this.CreateRandomBaby(name, types, animalHouse);

            // Reset the incubator regardless if a baby was hatched or not
            incubator.ResetIncubatingItem();
        }

        private List<string> DetermineHatchlingTypes(Incubator incubator)
        {
            // Grab the held item's index #
            string heldItemIndex = incubator.GetIncubatingItemIndex();

            // Randomize an eligible animal type from parents
            List<StardewValley.FarmAnimal> possibleParents = incubator.AnimalHouse.animals.Pairs.ToDictionary(pair => pair.Key, pair => pair.Value).Values.ToList<StardewValley.FarmAnimal>();
            List<string> types = this.DetermineTypesFromPossibleParents(heldItemIndex, possibleParents);

            // @TODO: VERY SMALL random chance to get something you don't own? Leave to random event?
            if (types.Count > 0)
                return types;

            return this.DetermineTypesFromProduce(incubator.GetIncubatingItemIndex());
        }

        private List<string> DetermineTypesFromPossibleParents(string produceIndex, List<StardewValley.FarmAnimal> possibleParents)
        {
            List<string> types = new List<string>();

            // Validate the potential types against what is in the coop
            foreach (StardewValley.FarmAnimal possibleParent in possibleParents)
            {
                // Already has this type
                if (types.Contains(possibleParent.type))
                    continue;

                // Babies cannot be parents
                if (possibleParent.isBaby())
                    continue;

                // Must be an adult and must produce this item
                if (this.FarmAnimalData.ProducesItem(possibleParent.type, produceIndex))
                    types.Add(possibleParent.type);
            }

            return types;
        }

        private List<string> DetermineTypesFromProduce(string produceIndex)
        {
            return this.FarmAnimalData.FindTypesByProduce(produceIndex);
        }

        private void CreateRandomBaby(string name, List<string> types, StardewValley.AnimalHouse animalHouse, long parentID = FarmAnimals.FarmAnimal.PARENT_ID_DEFAULT)
        {
            if (types.Count < 1)
                return;

            // Randomize an eligible animal type
            string type = types.ElementAt(Game1.random.Next(types.Count));

            // Create the baby!
            this.CreateBaby(name, type, animalHouse, parentID);
        }

        private FarmAnimals.FarmAnimal CreateBaby(string name, string type, StardewValley.AnimalHouse animalHouse, long parentID = FarmAnimals.FarmAnimal.PARENT_ID_DEFAULT)
        {
            FarmAnimals.FarmAnimal baby = new FarmAnimals.FarmAnimal(type, this.Farmer.GetNewID(), this.Farmer.MyID);

            baby.SynchronizeNames(name);
            baby.AssignParent(parentID);
            baby.RandomizeLocation(animalHouse);
            baby.AddToAnimalHouse(animalHouse);

            return baby;
        }

        private bool IsAvailable(string type)
        {
            return this.AvailableFarmAnimals.Contains(type);
        }

        public BreedFarmAnimal.NamingEvent DetermineNamingEvent()
        {
            if (this.IsNamingNewlyHatchedFarmAnimal())
                return BreedFarmAnimal.NamingEvent.Hatched;

            if (this.IsNamingNewlyBornFarmAnimal())
                return BreedFarmAnimal.NamingEvent.Birthed;

            return BreedFarmAnimal.NamingEvent.None;
        }

        private bool IsNamingNewlyHatchedFarmAnimal()
        {
            Coop coop;

            try
            {
                coop = new Coop(Game1.currentLocation);
            }
            catch
            {
                // Probably not in a coop
                return false;
            }

            // It comes with the Big Coop and the Deluxe Coop
            return coop.CanHaveIncubator();
        }

        private bool IsNamingNewlyBornFarmAnimal()
        {
            if (this.IsNamingNewlyHatchedFarmAnimal())
                return false;

            // Could be purchasing an animal, receiving a pet or a farm event
            // We only want to show this on the farm event
            if (this.IsFarmEvent())
                return false;

            QuestionEvent QuestionEvent = Game1.farmEvent as QuestionEvent;

            // Make sure the event was actually set up
            if (QuestionEvent.animal == null)
                return false;

            return true;
        }

        private bool IsFarmEvent()
        {
            return Game1.farmEvent == null || !(Game1.farmEvent is QuestionEvent);
        }
    }
}
