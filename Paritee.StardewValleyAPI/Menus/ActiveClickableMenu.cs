using StardewValley;
using StardewValley.Menus;
using System.Reflection;

namespace Paritee.StardewValleyAPI.Menus
{
    public class ActiveClickableMenu : Menu
    {
        private IClickableMenu ClickableMenu;

        public ActiveClickableMenu(IClickableMenu clickableMenu = null)
        {
            this.ClickableMenu = clickableMenu ?? Game1.activeClickableMenu;
        }

        public IClickableMenu GetMenu()
        {
            return this.ClickableMenu;
        }

        public bool IsOpen()
        {
            return this.ClickableMenu != null;
        }

        public void Exit()
        {
            if (!this.IsOpen())
                return;

            // Does the following: Game1.activeClickableMenu = (IClickableMenu)null;
            Game1.exitActiveMenu();
            this.ClickableMenu = null;
        }

        public void ExitWithSound(string sound)
        {
            this.Exit();
            this.PlaySound(sound);
        }

        private void PlaySound(string sound)
        {
            Game1.playSound(sound);
        }

        public void ExitWithoutSound()
        {
            if (this.IsOpen())
                return;

            this.ClickableMenu.exitThisMenuNoSound();
        }

        private FieldInfo GetField(string field)
        {
            return this.ClickableMenu.GetType().GetField(field, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public T GetValue<T>(string field)
        {
            return (T)this.GetField(field).GetValue(this.ClickableMenu);
        }

        public void SetValue<T>(string field, T value)
        {
            this.GetField(field).SetValue(this.ClickableMenu, value);
        }
    }
}
