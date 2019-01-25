using StardewValley;
using System;
using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.FarmAnimals.Variations
{
    public abstract class Variation
    {
        public const byte SAFETY_UNSAFE = 0;
        public const byte SAFETY_SAFE = 1;
        public abstract string Prefix { get; }

        public Variation() { }

        public bool RollChance(double chance)
        {
            return Game1.random.NextDouble() >= chance;
        }

        public string ApplyPrefix(string type)
        {
            return this.Prefix + " " + type;
        }

        private List<string> DetermineSpecialTypes()
        {
            List<string> baseVariations = new List<string>();
            Array values = Enum.GetValues(typeof(Type.Base));

            foreach (Type.Base typeBase in values)
                baseVariations.Add(this.ApplyPrefix(Type.ConvertBaseToString(typeBase)));

            return baseVariations;
        }

        public List<string> RemoveSpecialTypesFromList(List<string> types)
        {
            List<string> toRemove = this.DetermineSpecialTypes();

            // Didn't pass the logic test so we should remove it from the list of items if it was there
            types.RemoveAll(item => toRemove.Contains(item));

            return types;
        }
    }
}
