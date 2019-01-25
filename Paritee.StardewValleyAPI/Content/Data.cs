using StardewValley;
using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.Content
{
    public abstract class Data
    {
        protected const char DELIMITER = '/';

        protected Dictionary<string, string> Entries;

        public abstract string GetFilePath();

        public string[] Split(string value)
        {
            return value.Split(Data.DELIMITER);
        }

        protected Dictionary<string, string> Load()
        {
            return Game1.content.Load<Dictionary<string, string>>(this.GetFilePath());
        }

        public Dictionary<string, string> GetEntries()
        {
            return this.Entries;
        }
    }
}
