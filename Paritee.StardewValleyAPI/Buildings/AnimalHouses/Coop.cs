using PariteeAnimalHouse = Paritee.StardewValleyAPI.Buildings.AnimalHouses.AnimalHouse;
using StardewValley;
using System;

namespace Paritee.StardewValleyAPI.Buildings.AnimalHouses
{
    public class Coop : PariteeAnimalHouse
    {
        public const string COOP = "Coop";

        public Coop(GameLocation location) : base(location)
        {
            if (!location.Name.Contains(Coop.COOP))
                throw new ArgumentException(String.Format("{0} is not a coop", location.Name), "name");
        }

        public new bool CanHaveIncubator()
        {
            return this.IsBig() || this.IsDeluxe();
        }
    }
}
