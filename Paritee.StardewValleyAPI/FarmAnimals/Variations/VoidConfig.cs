namespace Paritee.StardewValleyAPI.FarmAnimals.Variations
{
    public class VoidConfig
    {
        public enum InShop
        {
            Never,
            QuestOnly,
            Always
        }

        public InShop AllowInShop;
        public bool HasCompletedQuest;

        public VoidConfig(InShop AllowInShop, bool hasCompletedQuest)
        {
            this.AllowInShop = AllowInShop;
            this.HasCompletedQuest = hasCompletedQuest;
        }
    }
}
