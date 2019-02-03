using Paritee.StardewValleyAPI.Buildings.AnimalHouses;
using StardewValley;
using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.Buildings.AnimalShop.FarmAnimals
{
    public class FarmAnimalForPurchase : StardewValley.Object
    {
        public List<string> FarmAnimalTypes;
        private readonly string Description;

        public override string DisplayName
        {
            get
            {
                return this.displayName;
            }
            set
            {
                this.displayName = value;
            }
        }

        public FarmAnimalForPurchase(string name, string displayName, string description, int price, List<string> buildingsILiveIn, List<string> types) : base(100, 1, false, price, 0)
        {
            this.Name = name;
            this.displayName = displayName;
            this.Description = description;
            this.Type = this.DetermineType(buildingsILiveIn);
            this.FarmAnimalTypes = types;
        }

        public override string getDescription()
        {
            return this.Description;
        }

        private string DetermineType(List<string> buildingsILiveIn)
        {
            Farm farm = Game1.getFarm();
            bool hasBuilding = false;
            string type = (string)null;

            foreach (string building in buildingsILiveIn)
            {
                if (farm.isBuildingConstructed(building))
                {
                    hasBuilding = true;
                    break;
                }
            }

            if (!hasBuilding)
            {
                BlueprintsData bluePrintsData = new BlueprintsData();
                Dictionary<string, string> entries = bluePrintsData.GetEntries();

                // Grab the actual name of the building
                string[] values = bluePrintsData.Split(entries[buildingsILiveIn[0]]);
                string buildingName = values[BlueprintsData.DISPLAY_NAME];

                // Grab the requires Coop string so we can replace "Coop" with the building's name
                string requiresBuilding = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5926");
                type = requiresBuilding.Replace(Coop.COOP, buildingName);
            }

            return type;
        }
    }
}
