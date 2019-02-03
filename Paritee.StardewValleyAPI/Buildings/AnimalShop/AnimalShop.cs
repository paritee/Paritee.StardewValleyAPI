using Paritee.StardewValleyAPI.Buildings.AnimalShop.FarmAnimals;

namespace Paritee.StardewValleyAPI.Buildings.AnimalShop
{
    public class AnimalShop
    {
        public Stock FarmAnimalStock;

        public AnimalShop(Stock stock)
        {
            this.FarmAnimalStock = stock;
        }
    }
}
