using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.FarmAnimals
{
    public class FarmAnimalsData : Paritee.StardewValleyAPI.Content.Data
    {
        private const string FILE_PATH = "Data\\FarmAnimals";

        public const string NO_PRODUCE_ITEM_ID = "-1";
        public const byte DEFAULT_PRODUCE_INDEX = 2;
        public const byte DELUXE_PRODUCE_INDEX = 3;
        public const byte BUILDING_TYPE_I_LIVE_IN_INDEX = 15;
        public const byte DISPLAY_NAME = 25;

        public FarmAnimalsData()
        {
            this.Entries = this.Load();
        }

        public override string GetFilePath()
        {
            return FarmAnimalsData.FILE_PATH;
        }

        public bool ProducesItem(string key, string produceIndex)
        {
            string[] DataArr = this.Split(this.GetEntries()[key]);

            return DataArr[FarmAnimalsData.DEFAULT_PRODUCE_INDEX].Equals(produceIndex) || DataArr[FarmAnimalsData.DELUXE_PRODUCE_INDEX].Equals(produceIndex);
        }

        public bool ProducesAtLeastOneItem(string key, string[] produceIndexes)
        {
            string[] DataArr = this.Split(this.GetEntries()[key]);

            foreach (string produceIndex in produceIndexes)
            {
                if (this.ProducesItem(key, produceIndex))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ProducesNothing(string key)
        {
            string[] DataArr = this.Split(this.GetEntries()[key]);

            // Note the difference between this function and ProducesItem - this 
            // function check that both default AND deluxe produce are "nothing"
            return DataArr[FarmAnimalsData.DEFAULT_PRODUCE_INDEX].Equals(FarmAnimalsData.NO_PRODUCE_ITEM_ID) && DataArr[FarmAnimalsData.DELUXE_PRODUCE_INDEX].Equals(FarmAnimalsData.NO_PRODUCE_ITEM_ID);
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
