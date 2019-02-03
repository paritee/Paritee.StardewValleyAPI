using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.FarmAnimals.Variations
{
    public class VoidVariation : Variation
    {
        public const string VOID = "Void";
        public const int QUEST_ID = 27; // Goblin Problem - end game quest as of SDV v.1.3.27

        public override string Prefix
        {
            get { return VoidVariation.VOID; }
        }

        private VoidConfig Config;

        public VoidVariation(VoidConfig config)
        {
            this.Config = config;
        }

        public bool IsInShop(VoidConfig.InShop condition)
        {
            return this.Config.AllowInShop == condition;
        }

        public bool IsAlwaysInShop()
        {
            return this.IsInShop(VoidConfig.InShop.Always);
        }

        public bool IsNeverInShop()
        {
            return this.IsInShop(VoidConfig.InShop.Never);
        }

        public bool CanPurchaseFromShop()
        {
            if (this.IsAlwaysInShop())
                return true;

            if (this.IsNeverInShop())
                return false;

            if (!this.Config.HasCompletedQuest)
                return false;

            // Use the same chance used for the Vanilla Blue Chickens
            return this.RollChance(BlueVariation.CHANCE_VALUE);
        }

        public List<string> SanitizeForShop(List<string> types, byte safety = Variation.SAFETY_UNSAFE)
        {
            List<string> ClonedTypes = new List<string>(types);

            // Make sure we account for the Void Chicken rarity
            if (!this.CanPurchaseFromShop())
                types = this.RemoveSpecialTypesFromList(types);

            // Make sure we didn't remove everything if the safety is on
            // If we did, pretend the sanitize didn't happen!
            // Scenario: types only contained Void Chicken and the config does not allow them in the shop
            if (safety == Variation.SAFETY_UNSAFE && types.Count < 1)
                types = ClonedTypes;

            return types;
        }
    }
}
