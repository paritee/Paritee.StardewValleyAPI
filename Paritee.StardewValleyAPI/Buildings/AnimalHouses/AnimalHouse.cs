using StardewValley;

namespace Paritee.StardewValleyAPI.Buildings.AnimalHouses
{
    public class AnimalHouse
    {
        public enum Size
        {
            Small,
            Big,
            Deluxe
        }

        protected GameLocation Location;

        public AnimalHouse(GameLocation location)
        {
            this.Location = location;
        }

        public static string FormatBuilding(string @base, AnimalHouse.Size size)
        {
            return size.ToString() + " " + @base;
        }

        public bool IsSize(AnimalHouse.Size size)
        {
            return size.Equals(AnimalHouse.Size.Small) ? this.IsSmall() : this.Location.Name.Contains(size.ToString());
        }

        public bool IsSmall()
        {
            return !this.IsBig() && !this.IsDeluxe();
        }

        public bool IsBig()
        {
            return this.Location.Name.Contains(AnimalHouse.Size.Big.ToString());
        }

        public bool IsDeluxe()
        {
            return this.Location.Name.Contains(AnimalHouse.Size.Deluxe.ToString());
        }

        public bool CanHaveIncubator()
        {
            return false;
        }
    }
}
