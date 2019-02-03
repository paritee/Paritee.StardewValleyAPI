using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paritee.StardewValleyAPI.FarmAnimals.Variations;
using StardewValley.Menus;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Paritee.StardewValleyAPI.Buildings.AnimalShop.FarmAnimals
{
    public class Stock
    {
        public const byte SANITIZE_KEEP = 0;
        public const byte SANITIZE_REMOVE = 1;

        public enum VanillaName
        {
            [EnumMember(Value = "Dairy Cow")]
            DairyCow,
            Sheep,
            Goat,
            Pig,
            Chicken,
            Duck,
            Rabbit
        }

        public BlueVariation BlueFarmAnimals;
        public VoidVariation VoidFarmAnimals;
        public List<FarmAnimalForPurchase> FarmAnimalsForPurchase;

        public Stock(StockConfig stockConfig)
        {
            this.BlueFarmAnimals = stockConfig.BlueFarmAnimals;
            this.VoidFarmAnimals = stockConfig.VoidFarmAnimals;

            // Use the lists because the meat indices can't be relied upon (ex. mutton)
            this.FarmAnimalsForPurchase = stockConfig.FarmAnimalsForPurchase;
        }

        public List<ClickableTextureComponent> DetermineClickableComponents(PurchaseAnimalsMenu menu, Dictionary<string, Texture2D> textures)
        {
            List<ClickableTextureComponent> animalsToPurchase = new List<ClickableTextureComponent>();

            for (int index = 0; index < this.FarmAnimalsForPurchase.Count; ++index)
            {
                FarmAnimalForPurchase farmAnimalForPurchase = this.FarmAnimalsForPurchase[index];

                string name = farmAnimalForPurchase.salePrice().ToString();
                string label = (string)null;
                string hoverText = farmAnimalForPurchase.displayName + "_" + farmAnimalForPurchase.getDescription();

                Rectangle bounds = new Microsoft.Xna.Framework.Rectangle(menu.xPositionOnScreen + IClickableMenu.borderWidth + index % 3 * 64 * 2, menu.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2 + index / 3 * 85, 128, 64);
                Texture2D texture = textures[farmAnimalForPurchase.Name];
                Rectangle sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
                float scale = 4f;
                bool drawShadow = farmAnimalForPurchase.Type == null;

                ClickableTextureComponent textureComponent = new ClickableTextureComponent(name, bounds, label, hoverText, texture, sourceRect, scale, drawShadow)
                {
                    item = farmAnimalForPurchase,
                    myID = index,
                    rightNeighborID = index % 3 == 2 ? -1 : index + 1,
                    leftNeighborID = index % 3 == 0 ? -1 : index - 1,
                    downNeighborID = index + 3,
                    upNeighborID = index - 3
                };

                animalsToPurchase.Add(textureComponent);
            }

            return animalsToPurchase;
        }

        public string DetermineNameFromType(string type)
        {
            foreach (FarmAnimalForPurchase farmAnimalForPurchase in this.FarmAnimalsForPurchase)
            {
                if (farmAnimalForPurchase.FarmAnimalTypes.Contains(type))
                {
                    return farmAnimalForPurchase.Name;
                }
            }

            throw new StockDoesNotExistException();
        }

        public List<string> GetAvailableTypes(string name, byte sanitize = Stock.SANITIZE_KEEP)
        {
            List<string> Types = this.FarmAnimalsForPurchase.First<FarmAnimalForPurchase>(k => k.Name == name).FarmAnimalTypes;

            if (sanitize == Stock.SANITIZE_REMOVE)
            {
                // Make sure we account for the Blue <FarmAnimal> rarity
                Types = this.BlueFarmAnimals.Sanitize(Types);

                // We also need to make sure we're not including Void <FarmAnimal>s in the shop if they don't want them
                Types = this.VoidFarmAnimals.SanitizeForShop(Types, Variation.SAFETY_SAFE);
            }

            return Types;
        }
    }
}
