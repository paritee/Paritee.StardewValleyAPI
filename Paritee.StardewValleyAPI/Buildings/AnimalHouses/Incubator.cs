using StardewValley;
using System;

namespace Paritee.StardewValleyAPI.FarmAnimals.Buildings.AnimalHouses
{
    public class Incubator
    {
        private const string INCUBATOR = "Incubator";

        private const StardewValley.Object HELD_OBJECT_DEFAULT = null;
        private const int ITEM_INDEX_DEFAULT = 101;
        private const int INCUBATING_EGG_X_DEFAULT = 0;
        private const int INCUBATING_EGG_Y_DEFAULT = -1;

        private StardewValley.Object Self;
        public AnimalHouse AnimalHouse;

        public Incubator(StardewValley.Object @object, AnimalHouse animalHouse)
        {
            this.Self = @object;
            this.AnimalHouse = animalHouse;
        }

        public Incubator(AnimalHouse animalHouse)
        {
            StardewValley.Object @object = this.FindInAnimalHouse(animalHouse);

            if (@object == null)
                throw new ArgumentException("Could not find an incubator", "animalHouse");

            this.Self = @object;
            this.AnimalHouse = animalHouse;
        }

        private StardewValley.Object FindInAnimalHouse(AnimalHouse animalHouse)
        {
            // Try to get the reference for the incubator object
            foreach (StardewValley.Object @object in animalHouse.objects.Values)
            {
                if (@object.bigCraftable && @object.Name.Contains(Incubator.INCUBATOR) && @object.heldObject != null)
                    return @object;
            }

            return null;
        }

        public void ResetIncubatingItem()
        {
            this.ResetSelfIncubatingItem();
            this.ResetAnimalHouseIncubatingItem();
        }

        private void ResetSelfIncubatingItem()
        {
            // Remove the item from the Incubator
            this.Self.heldObject.Set(Incubator.HELD_OBJECT_DEFAULT);
            this.Self.ParentSheetIndex = Incubator.ITEM_INDEX_DEFAULT;
        }

        private void ResetAnimalHouseIncubatingItem()
        {
            // DEFAULT the animal house
            this.AnimalHouse.incubatingEgg.X = Incubator.INCUBATING_EGG_X_DEFAULT;
            this.AnimalHouse.incubatingEgg.Y = Incubator.INCUBATING_EGG_Y_DEFAULT;
        }

        public string GetIncubatingItemIndex()
        {
            return this.Self.heldObject.Value.parentSheetIndex.ToString();
        }
    }
}
