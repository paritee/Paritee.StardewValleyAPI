using FarmAnimalsData = Paritee.StardewValleyAPI.FarmAnimals.Data;
using FarmAnimalsType = Paritee.StardewValleyAPI.FarmAnimals.Type;
using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using Paritee.StardewValleyAPI.Utilities;
using System.Collections.Generic;
using System;
using System.Linq;
using VariationsVoid = Paritee.StardewValleyAPI.FarmAnimals.Variations.Void;

namespace Paritee.StardewValleyAPI.Buidlings.AnimalShop.FarmAnimals
{
    public class StockConfig
    {
        public Dictionary<Stock.Name, string[]> Available;
        public Blue BlueFarmAnimals;
        public VariationsVoid VoidFarmAnimals;

        public StockConfig(Dictionary<Stock.Name, string[]> available, Blue blueFarmAnimals, VariationsVoid voidFarmAnimals)
        {
            this.Available = available;
            this.BlueFarmAnimals = blueFarmAnimals;
            this.VoidFarmAnimals = voidFarmAnimals;
        }

        public StockConfig(Blue blueFarmAnimals, VariationsVoid voidFarmAnimals)
        {
            this.Available = this.GetDefaultAvailable();
            this.BlueFarmAnimals = blueFarmAnimals;
            this.VoidFarmAnimals = voidFarmAnimals;
        }

        public Dictionary<Stock.Name, string[]> GetDefaultAvailable()
        {
            FarmAnimalsData data = new FarmAnimalsData();
            Dictionary<string, string> entries = data.GetEntries();
            Dictionary<Stock.Name, List<string>> farmAnimals = new Dictionary<Stock.Name, List<string>>();

            foreach (KeyValuePair<string, string> entry in entries)
            {
                try
                {
                    string baseType = entry.Key.Split(' ').Last();

                    Stock.Name stockName = baseType.Equals(Enums.GetValue(FarmAnimalsType.Base.Cow))
                        ? Stock.Name.DairyCow
                        : (Stock.Name)Enum.Parse(typeof(Stock.Name), baseType);

                    if (!farmAnimals.ContainsKey(stockName))
                        farmAnimals.Add(stockName, new List<string>());

                    farmAnimals[stockName].Add(entry.Key);
                }
                catch
                {
                    // Do nothing, catch animals like "Dinosaur" that don't belong in the shop
                }
            }

            return farmAnimals.ToDictionary(k => k.Key, k => k.Value.ToArray<string>());
        }
    }
}
