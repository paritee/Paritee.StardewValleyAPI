using Paritee.StardewValleyAPI.Buidlings.AnimalShop.FarmAnimals;
using Paritee.StardewValleyAPI.Players.Actions;
using StardewValley.Menus;
using StardewValley;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;

namespace Paritee.StardewValleyAPI.Menus
{
    public class PurchaseFarmAnimalMenu : Menu
    {
        private PurchaseFarmAnimal PurchaseFarmAnimal;
        private PurchaseAnimalsMenu PurchaseAnimalsMenu;

        public PurchaseFarmAnimalMenu(PurchaseAnimalsMenu purchaseAnimalsMenu, PurchaseFarmAnimal purchaseFarmAnimal)
        {
            this.PurchaseAnimalsMenu = purchaseAnimalsMenu;
            this.PurchaseFarmAnimal = purchaseFarmAnimal;

            // @TODO: Add in more configuration for price (or hide it? or range possibilities from types?)
            // Class:StardewValley.Menus.PurchaseAnimalsMenu
            // ---
            //string animalTitle = PurchaseAnimalsMenu.getAnimalTitle(this.hovered.hoverText);
            //SpriteText.drawStringWithScrollBackground(b, animalTitle, this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + 64, this.yPositionOnScreen + this.height - 32 + IClickableMenu.spaceToClearTopBorder / 2 + 8, "Truffle Pig", 1f, -1);
            //SpriteText.drawStringWithScrollBackground(b, "$" + Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object)this.hovered.item.salePrice()), this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + 128, this.yPositionOnScreen + this.height + 64 + IClickableMenu.spaceToClearTopBorder / 2 + 8, "$99999999g", Game1.player.Money >= this.hovered.item.salePrice() ? 1f : 0.5f, -1);
            //string animalDescription = PurchaseAnimalsMenu.getAnimalDescription(this.hovered.hoverText);
            //IClickableMenu.drawHoverText(b, Game1.parseText(animalDescription, Game1.smallFont, 320), Game1.smallFont, 0, 0, -1, animalTitle, -1, (string[])null, (Item)null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe)null);
        }

        private void HandleManualClose()
        {
            (new ActiveClickableMenu()).ExitWithSound("bigDeSelect");
        }

        private bool HasOkButton()
        {
            return this.PurchaseAnimalsMenu.okButton != null;
        }

        private bool TappedOnOkButton(int x, int y)
        {
            return this.HasOkButton() && this.PurchaseAnimalsMenu.okButton.containsPoint(x, y);
        }

        private bool IsReadyToClose()
        {
            return this.PurchaseAnimalsMenu.readyToClose();
        }

        private bool IsOnStockSelectionMenu()
        {
            // Grab from parent menu
            ActiveClickableMenu ActiveClickableMenu = new ActiveClickableMenu();

            bool Freeze = ActiveClickableMenu.GetValue<bool>("freeze");

            if (Game1.globalFade || Freeze)
                return false;

            // Grab from parent menu
            bool IsOnFarm = ActiveClickableMenu.GetValue<bool>("onFarm");

            if (IsOnFarm)
                return false;

            return true;
        }

        public void HandleTap(ButtonPressedEventArgs e)
        {
            if (!this.IsOnStockSelectionMenu())
                return;

            this.PurchaseFarmAnimal.Farmer.Helper.Input.Suppress(e.Button);

            // Get the clicked screen pixels
            Point Coords = this.PurchaseFarmAnimal.Farmer.GetCursorCoordinates(e);

            // Close menu
            if (this.TappedOnOkButton(Coords.X, Coords.Y) && this.IsReadyToClose())
            {
                this.HandleManualClose();
                return;
            }

            Item Stock = this.DetermineSelectedStock(Coords.X, Coords.Y);

            if (Stock == null)
                return;

            this.HandleStockSelection(Stock);
        }

        private StardewValley.Item DetermineSelectedStock(int x, int y)
        {
            foreach (ClickableTextureComponent StockComponent in this.PurchaseAnimalsMenu.animalsToPurchase)
            {
                if (this.StockContainsTap(StockComponent, x, y))
                    return StockComponent.item;
            }

            return (StardewValley.Item)null;
        }

        private bool StockContainsTap(ClickableTextureComponent stockComponent, int x, int y)
        {
            return stockComponent.containsPoint(x, y) && (stockComponent.item as StardewValley.Object).Type == null;
        }

        private void HandleStockSelection(Item stock)
        {
            if (!this.PurchaseFarmAnimal.CanAfford(stock))
                return;

            // Since we're not propogating the click, make sure we change the relevant parent info
            this.SetupForAfterFade();
            this.PropogateStockSelection(stock);
        }

        private void SetupForAfterFade()
        {
            Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.PurchaseAnimalsMenu.setUpForAnimalPlacement), 0.02f);
            Game1.playSound("smallSelect");
        }

        private void PropogateStockSelection(Item stock)
        {
            ActiveClickableMenu ActiveClickableMenu = new ActiveClickableMenu();

            ActiveClickableMenu.SetValue<bool>("onFarm", true);
            ActiveClickableMenu.SetValue<int>("priceOfAnimal", stock.salePrice());

            // PurchaseAnimalsMenu.cs: public PurchaseAnimalsMenu(List<StardewValley.Object> stock)
            Stock.Name name = this.PurchaseFarmAnimal.AnimalShop.FarmAnimalStock.StringToName(stock.Name);
            FarmAnimals.FarmAnimal AnimalBeingPurchased = this.PurchaseFarmAnimal.RandomizeFarmAnimal(name);

            // Update the animalBeingPurchased
            // !!! We have to convert to a base Farm Animal due to exceptions thrown by the day's save XML functions
            ActiveClickableMenu.SetValue<StardewValley.FarmAnimal>("animalBeingPurchased", AnimalBeingPurchased.ToFarmAnimal());
        }
    }
}
