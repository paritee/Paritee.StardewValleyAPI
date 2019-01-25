using Paritee.StardewValleyAPI.Players.Actions;
using StardewValley;
using StardewValley.Events;
using StardewValley.Menus;

namespace Paritee.StardewValleyAPI.Menus
{
    public class NameFarmAnimalMenu : Menu
    {
        private BreedFarmAnimal BreedFarmAnimal;
        private StardewValley.Menus.NamingMenu NamingMenu;

        private string Title
        {
            get
            {
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestionEvent.cs.6692");
            }
        }

        public NameFarmAnimalMenu(StardewValley.Menus.NamingMenu namingMenu, BreedFarmAnimal breedFarmAnimal)
        {
            this.NamingMenu = namingMenu;
            this.BreedFarmAnimal = breedFarmAnimal;
        }

        public bool IsCustomerNamingMenuOpen()
        {
            return this.NamingMenu.GetType() == typeof(NamingMenu);
        }

        public void HandleChange()
        {
            if (this.IsCustomerNamingMenuOpen())
                return;

            NamingMenu.doneNamingBehavior DoneNaming = this.DetermineDoneNamingBehavior(this.BreedFarmAnimal.DetermineNamingEvent());

            (new ActiveClickableMenu()).ExitWithoutSound();

            if (DoneNaming != null)
                this.OpenCustomNamingMenu(DoneNaming, this.Title);
        }

        public NamingMenu.doneNamingBehavior DetermineDoneNamingBehavior(BreedFarmAnimal.NamingEvent namingEvent)
        {
            switch (namingEvent)
            {
                case BreedFarmAnimal.NamingEvent.Birthed:
                    return new NamingMenu.doneNamingBehavior(this.HandleFarmAnimalBirth);
                case BreedFarmAnimal.NamingEvent.Hatched:
                    return new NamingMenu.doneNamingBehavior(this.HandleFarmAnimalHatched);
                default:
                    return null;
            }
        }

        public void OpenCustomNamingMenu(NamingMenu.doneNamingBehavior doneNaming, string title, string defaultName = null)
        {
            // Launch the custom one
            NamingMenu BetterNamingMenu = new NamingMenu(doneNaming, title, defaultName);

            this.NamingMenu = BetterNamingMenu;

            Game1.activeClickableMenu = (IClickableMenu)this.NamingMenu;
        }

        private void HandleFarmAnimalHatched(string name)
        {
            // Create the baby animal
            this.BreedFarmAnimal.CreateFromIncubator(Game1.currentLocation as AnimalHouse, name);
            this.HandleAfterFarmAnimalIsCreated();
        }

        private void HandleFarmAnimalBirth(string name)
        {
            QuestionEvent QuestionEvent = Game1.farmEvent as QuestionEvent;

            // Create the baby animal
            this.BreedFarmAnimal.CreateFromParent(QuestionEvent.animal, name);

            // Specific for this animal event
            QuestionEvent.forceProceed = true;

            this.HandleAfterFarmAnimalIsCreated();
        }

        private void HandleAfterFarmAnimalIsCreated()
        {
            // Continue the current event if there is one
            this.BreedFarmAnimal.Farmer.ContinueCurrentEvent();

            // Exit the active menu
            (new ActiveClickableMenu()).Exit();
            this.NamingMenu = (NamingMenu)null;
        }
    }
}
