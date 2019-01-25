using Paritee.StardewValleyAPI.Utilities;
using System.Runtime.Serialization;

namespace Paritee.StardewValleyAPI.FarmAnimals
{
    public class Type
    {
        // Bases including extension for male animals
        public enum Base
        {
            Cow,
            Bull,
            Ram,
            [EnumMember(Value = "Billy Goat")]
            BillyGoat,
            Chicken,
            Rooster,
            Drake,
            Goat,
            Duck,
            Rabbit,
            Dinosaur,
            Sheep,
            Pig,
            Hog
        };

        public static string ConvertBaseToString(Type.Base typeBase)
        {
            return Enums.GetValue(typeBase);
        }
    }
}
