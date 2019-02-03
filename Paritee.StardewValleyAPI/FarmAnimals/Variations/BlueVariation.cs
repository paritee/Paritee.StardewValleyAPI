using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.FarmAnimals.Variations
{
    public class BlueVariation : Variation
    {
        public const string BLUE = "Blue";
        public const double CHANCE_VALUE = 0.25;
        public const int EVENT_ID = 3900074;

        public override string Prefix
        {
            get { return BlueVariation.BLUE; }
        }

        private BlueConfig Config;

        public BlueVariation(BlueConfig config)
        {
            this.Config = config;
        }

        public bool AreAvailableToPlayer()
        {
            if (!this.Config.HasSeenEvent)
                return false;

            return this.RollChance(BlueVariation.CHANCE_VALUE);
        }

        public List<string> Sanitize(List<string> types, byte safety = Variation.SAFETY_UNSAFE)
        {
            List<string> ClonedTypes = new List<string>(types);

            // Make sure we account for the blue chicken rarity
            if (!this.AreAvailableToPlayer())
                types = this.RemoveSpecialTypesFromList(types);

            // Make sure we didn't remove everything if the safety is on
            // If we did, pretend the sanitize didn't happen!
            // Scenario: types only contained Blue Chicken and the chance failed
            if (safety == BlueVariation.SAFETY_SAFE && types.Count < 1)
                types = ClonedTypes;

            return types;
        }
    }
}
