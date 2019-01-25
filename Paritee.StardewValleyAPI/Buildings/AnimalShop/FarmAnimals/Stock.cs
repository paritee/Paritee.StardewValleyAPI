using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using Paritee.StardewValleyAPI.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Paritee.StardewValleyAPI.Buidlings.AnimalShop.FarmAnimals
{
    public class Stock
    {
        public const byte SANITIZE_KEEP = 0;
        public const byte SANITIZE_REMOVE = 1;

        public enum Name
        {
            [EnumMember(Value = "Dairy Cow")]
            DairyCow,
            Sheep,
            Goat,
            Pig,
            Chicken,
            Duck,
            Rabbit
        }

        private Blue BlueFarmAnimals;
        private Paritee.StardewValleyAPI.FarmAnimals.Variations.Void VoidFarmAnimals;
        private Dictionary<Stock.Name, string[]> Available;

        public Stock(StockConfig stockConfig)
        {
            this.BlueFarmAnimals = stockConfig.BlueFarmAnimals;
            this.VoidFarmAnimals = stockConfig.VoidFarmAnimals;

            // Use the lists because the meat indices can't be relied upon (ex. mutton)
            this.Available = stockConfig.Available;
        }

        public Stock.Name StringToName(string name)
        {
            foreach (KeyValuePair<Stock.Name, string[]> entry in this.Available)
            {
                if (name.Equals(Enums.GetValue(entry.Key)))
                    return entry.Key;
            }

            throw new StockDoesNotExistException();
        }

        public Stock.Name DetermineNameFromType(string type)
        {
            foreach (KeyValuePair<Stock.Name, string[]> entry in this.Available)
            {
                foreach (string stockType in entry.Value)
                {
                    if (type == stockType)
                        return entry.Key;
                }
            }

            throw new StockDoesNotExistException();
        }

        public List<string> GetAvailableTypes(Stock.Name name, byte sanitize = Stock.SANITIZE_KEEP)
        {
            List<string> Types = this.Available[name].ToList();

            if (sanitize == Stock.SANITIZE_REMOVE)
            {
                // Make sure we account for the Blue <FarmAnimal> rarity
                Types = this.BlueFarmAnimals.Sanitize(Types);

                // We also need to make sure we're not including Void <FarmAnimal>s in the shop if they don't want them
                Types = this.VoidFarmAnimals.SanitizeForShop(Types, Variation.SAFETY_SAFE);
            }

            return Types;
        }
    }
}
