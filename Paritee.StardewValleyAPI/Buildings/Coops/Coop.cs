using StardewValley;
using System;

namespace Paritee.StardewValleyAPI.Buildings.Coops
{
    public class Coop
    {
        public enum Size
        {
            Small, 
            Big, 
            Deluxe
        }

        public const string COOP = "Coop";

        private GameLocation Location;

        public Coop(GameLocation location)
        {
            if (!location.Name.Contains(Coop.COOP))
                throw new ArgumentException(String.Format("{0} is not a coop", location.Name), "name");

            this.Location = location;
        }

        public bool IsSmall()
        {
            return !this.IsBig() && !this.IsDeluxe();
        }

        public bool IsBig()
        {
            return this.Location.Name.Contains(Coop.Size.Big.ToString());
        }

        public bool IsDeluxe()
        {
            return this.Location.Name.Contains(Coop.Size.Deluxe.ToString());
        }

        public bool IsSize(Coop.Size size)
        {
            return size.Equals(Coop.Size.Small) ? this.IsSmall() : this.Location.Name.Contains(size.ToString());
        }

        public bool CanHaveIncubator()
        {
            return this.IsBig() || this.IsDeluxe();
        }
    }
}
