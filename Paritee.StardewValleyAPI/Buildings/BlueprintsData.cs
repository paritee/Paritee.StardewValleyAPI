namespace Paritee.StardewValleyAPI.Buildings
{
    public class BlueprintsData : Paritee.StardewValleyAPI.Content.Data
    {
        private const string FILE_PATH = "Data\\Blueprints";

        public const byte DISPLAY_NAME = 8;

        public BlueprintsData()
        {
            this.Entries = this.Load();
        }

        public override string GetFilePath()
        {
            return BlueprintsData.FILE_PATH;
        }
    }
}
