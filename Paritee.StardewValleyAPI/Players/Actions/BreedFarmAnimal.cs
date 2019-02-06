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
        private readonly Dictionary<string, List<string>> AvailableFarmAnimals;

        private readonly BlueVariation BlueFarmAnimals;
        private readonly FarmAnimalsData FarmAnimalsData;
        private readonly bool RandomizeNewbornFromCategory;
        private readonly bool RandomizeHatchlingFromCategory;
        private readonly bool IgnoreParentProduceCheck;

        public BreedFarmAnimal(Player farmer, BreedFarmAnimalConfig breedFarmAnimalConfig)
        {
            this.Farmer = farmer;
            this.AvailableFarmAnimals = breedFarmAnimalConfig.AvailableFarmAnimals;
            this.BlueFarmAnimals = breedFarmAnimalConfig.BlueFarmAnimals;
            this.FarmAnimalsData = breedFarmAnimalConfig.FarmAnimalsData;
            this.RandomizeNewbornFromCategory = breedFarmAnimalConfig.RandomizeNewbornFromCategory;
            this.RandomizeHatchlingFromCategory = breedFarmAnimalConfig.RandomizeHatchlingFromCategory;
            this.IgnoreParentProduceCheck = breedFarmAnimalConfig.IgnoreParentProduceCheck;
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
            string type;

            // ex. Bulls don't produce anything, but may still exist in the Dairy Cow category
            if (this.RandomizeNewbornFromCategory)
            {
                List<string> types = this.ConsiderTypesFromCategory(new List<string>() { parent.type }, new string[] { parent.defaultProduceIndex.ToString(), parent.deluxeProduceIndex.ToString() });

                // Check the count in case someone messed up and removed the parent from the config
                type = types.Count > 0 ? types.ElementAt(Game1.random.Next(types.Count)) : parent.type;
            }
            else
            {
                // Don't randomize within the parent's category
                // This means the baby will always match the parent's type
                type = parent.type;
            }

            return this.CreateBaby(name, type, (StardewValley.AnimalHouse)parent.home.indoors, parent.myID);
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

            if (types.Count <= 0)
            {
                // Try to grab anything that produces that item regardless if owned/in the coop
                // ex. Parent was sold before hatchling hatched
                types = this.DetermineTypesFromProduce(incubator.GetIncubatingItemIndex());
            }

            // ex. Roosters don't produce anything, but may still exist in the Chicken category
            if (this.RandomizeHatchlingFromCategory)
            {
                List<string> typesFromCategory = this.ConsiderTypesFromCategory(types, new string[] { heldItemIndex });

                // Check the count in case someone messed up and removed the parent from the config
                if (typesFromCategory.Count > 0)
                {
                    types = typesFromCategory;
                }
            }

            return types;
        }

        private List<string> DetermineTypesFromPossibleParents(string produceIndex, List<StardewValley.FarmAnimal> possibleParents)
        {
            List<string> types = new List<string>();

            // Validate the potential types against what is in the coop
            foreach (StardewValley.FarmAnimal possibleParent in possibleParents)
            {
                // Already has this type
                if (types.Contains(possibleParent.type))
                {
                    continue;
                }

                // Babies cannot be parents
                if (possibleParent.isBaby())
                {
                    continue;
                }

                // Must be an adult and must produce this item
                if (this.FarmAnimalsData.ProducesItem(possibleParent.type, produceIndex))
                {
                    types.Add(possibleParent.type);
                }
            }

            return types;
        }

        private List<string> ConsiderTypesFromCategory(List<string> types, string[] produceIndexes)
        {
            // Grab all the categories that are possible
            types = this.AvailableFarmAnimals.Where(
                    // Use Intersect to check if the two lists have any elements in common
                    kvp => kvp.Value.Intersect(types).Any()
                )
                // Keep the dictionary
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                // We only care about the list of lists
                .Values
                // and we need to flatten it
                .SelectMany(i => i)
                // so that we just get a single flat list of types to randomize later
                .ToList();

            // Any check is to only attempt to remove types if we're checking 
            // on an actual produce item. Bulls would fail this check as they 
            // produce -1,-1 and we have no reason to filter out other animals 
            // in this category
            if (!this.IgnoreParentProduceCheck && produceIndexes.Any(i => !i.Equals(FarmAnimalsData.NO_PRODUCE_ITEM_ID)))
            {
                // We still don't want to include types that explicitly state they 
                // produce a different item.
                // Remove any type that:
                // - produces something and
                // - the something is not the specified item
                types.RemoveAll(i => !this.FarmAnimalsData.ProducesNothing(i) && !this.FarmAnimalsData.ProducesAtLeastOneItem(i, produceIndexes));
            }

            return types;
        }

        private List<string> DetermineTypesFromProduce(string produceIndex)
        {
            return this.FarmAnimalsData.FindTypesByProduce(produceIndex);
        }

        private void CreateRandomBaby(string name, List<string> types, StardewValley.AnimalHouse animalHouse, long parentID = FarmAnimals.FarmAnimal.PARENT_ID_DEFAULT)
        {
            if (types.Count < 1)
            {
                return;
            }

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
            // SelectMany will flatten the list of lists
            return this.AvailableFarmAnimals.Values.SelectMany(i => i).Contains(type);
        }

        public BreedFarmAnimal.NamingEvent DetermineNamingEvent()
        {
            if (this.IsNamingNewlyHatchedFarmAnimal())
            {
                return BreedFarmAnimal.NamingEvent.Hatched;
            }

            if (this.IsNamingNewlyBornFarmAnimal())
            {
                return BreedFarmAnimal.NamingEvent.Birthed;
            }

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
            {
                return false;
            }

            // Could be purchasing an animal, receiving a pet or a farm event
            // We only want to show this on the farm event
            if (this.IsFarmEvent())
            {
                return false;
            }

            QuestionEvent QuestionEvent = Game1.farmEvent as QuestionEvent;

            // Make sure the event was actually set up
            if (QuestionEvent.animal == null)
            {
                return false;
            }

            return true;
        }

        private bool IsFarmEvent()
        {
            return Game1.farmEvent == null || !(Game1.farmEvent is QuestionEvent);
        }
    }
}
