using Paritee.StardewValleyAPI.Buildings.AnimalShop;
using Paritee.StardewValleyAPI.Buildings.AnimalShop.FarmAnimals;
using StardewValley;
using System.Collections.Generic;
using System.Linq;

namespace Paritee.StardewValleyAPI.Players.Actions
{
    public class PurchaseFarmAnimal
    {
        public Player Farmer;
        public AnimalShop AnimalShop;

        public PurchaseFarmAnimal(Player farmer, AnimalShop animalShop)
        {
            this.Farmer = farmer;
            this.AnimalShop = animalShop;
        }

        public FarmAnimals.FarmAnimal RandomizeFarmAnimal(string name)
        {
            // We need to randomize after the FarmAnimal is created to avoid restrictions in FarmAnimal.cs code (ex. "Blue Cow")
            List<string> Types = this.AnimalShop.FarmAnimalStock.GetAvailableTypes(name, Stock.SANITIZE_REMOVE);

            if (Types.Count < 1)
                return null;

            // Randomly select an eligible type
            string Type = Types.ElementAt(Game1.random.Next(Types.Count));

            return new FarmAnimals.FarmAnimal(Type, this.Farmer.GetNewID(), this.Farmer.MyID);
        }

        public bool CanAfford(Item stockAnimal)
        {
            return this.Farmer.CanAfford(stockAnimal.salePrice());
        }
    }
}
