using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.FarmAnimals
{
    public class Data : Paritee.StardewValleyAPI.Content.Data
    {
        private const string FILE_PATH = "Data\\FarmAnimals";

        public const byte DEFAULT_PRODUCE_INDEX = 2;
        public const byte DELUXE_PRODUCE_INDEX = 3;
        public const byte DISPLAY_NAME = 25;

        public Data()
        {
            this.Entries = this.Load();
        }

        public override string GetFilePath()
        {
            return Data.FILE_PATH;
        }

        public bool ProducesItem(string key, string produceIndex)
        {
            string[] DataArr = this.Split(this.GetEntries()[key]);

            return produceIndex.Equals(DataArr[Data.DEFAULT_PRODUCE_INDEX]) || produceIndex.Equals(DataArr[Data.DELUXE_PRODUCE_INDEX]);
        }

        public List<string> FindTypesByProduce(string produceIndex)
        {
            List<string> Types = new List<string>();
            Dictionary<string, string> Entries = this.GetEntries();

            // Grab the animal based on what they produce! Do this to not mix up White Chickens and Void Chickens, etc.
            foreach (KeyValuePair<string, string> entry in Entries)
            {
                // Check against default and deluxe produce
                if (this.ProducesItem(entry.Key, produceIndex))
                    Types.Add(entry.Key);
            }

            return Types;
        }
    }
}
