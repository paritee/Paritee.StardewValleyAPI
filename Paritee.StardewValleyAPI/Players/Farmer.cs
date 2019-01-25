using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Quests;
using System.Collections.Generic;
using System.Linq;

namespace Paritee.StardewValleyAPI.Players
{
    public class Player
    {
        public StardewValley.Farmer Me;
        public IModHelper Helper;

        public long MyID
        {
            get
            {
                return this.Me.UniqueMultiplayerID;
            }
        }

        public Player(StardewValley.Farmer farmer, IModHelper helper)
        {
            this.Me = farmer;
            this.Helper = helper;
        }

        public long GetNewID()
        {
            return this.Helper.Multiplayer.GetNewID();
        }

        public bool CanAfford(int cost)
        {
            return this.Me.money >= cost;
        }

        public bool HasSeenEvent(int eventID)
        {
            return this.Me.eventsSeen.Contains(eventID);
        }

        public bool HasCompletedQuest(int questID)
        {
            // Check the quest log
            List<Quest> QuestLog = this.Me.questLog.ToList<Quest>();

            foreach (Quest Quest in QuestLog)
            {
                if (Quest.id == questID && Quest.completed)
                    return true;
            }

            return false;
        }

        public void ContinueCurrentEvent()
        {
            if (Game1.currentLocation.currentEvent != null)
                ++Game1.currentLocation.currentEvent.CurrentCommand;
        }

        public delegate long GetNewMultiplayerIDBehavior();

        public Point GetCursorCoordinates(ButtonPressedEventArgs e)
        {
            int X = (int)e.Cursor.ScreenPixels.X;
            int Y = (int)e.Cursor.ScreenPixels.Y;

            return new Point(X, Y);
        }
    }
}
