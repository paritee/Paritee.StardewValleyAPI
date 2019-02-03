using PariteeAnimalHouse = Paritee.StardewValleyAPI.Buildings.AnimalHouses.AnimalHouse;
using StardewValley;
using System;

namespace Paritee.StardewValleyAPI.Buildings.AnimalHouses
{
    public class Barn : PariteeAnimalHouse
    {
        public const string BARN = "Barn";

        public Barn(GameLocation location) : base(location)
        {
            if (!location.Name.Contains(Barn.BARN))
                throw new ArgumentException(String.Format("{0} is not a barn", location.Name), "name");
        }
    }
}
