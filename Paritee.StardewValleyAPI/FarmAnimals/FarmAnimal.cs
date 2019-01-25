using Microsoft.Xna.Framework;
using Netcode;
using StardewValley;
using StardewValley.Buildings;
using System;
using System.Collections.Generic;

namespace Paritee.StardewValleyAPI.FarmAnimals
{
    public class FarmAnimal : StardewValley.FarmAnimal
    {
        public const int HEALTH_DEFAULT = 3;
        public const long PARENT_ID_DEFAULT = -1L;
        public const int FRIENDSHIP_STEP = 200;

        private const string NONE = "none";

        public FarmAnimal(string type, long myID, long ownerID)
        {
            ((NetFieldBase<string, NetString>)this.type).Set(type);
            ((NetFieldBase<long, NetLong>)this.myID).Set(myID);
            ((NetFieldBase<long, NetLong>)this.ownerID).Set(ownerID);
            ((NetFieldBase<int, NetInt>)this.health).Set(FarmAnimal.HEALTH_DEFAULT);

            this.SynchronizeNames(this.RandomizeName());
            this.SetFieldsFromData();
        }

        private string RandomizeName()
        {
            return Dialogue.randomName();
        }

        public string SynchronizeNames(string name)
        {
            this.Name = this.displayName = name;
            return name;
        }

        public void SetFriendshipHearts(int level)
        {
            this.friendshipTowardFarmer.Value = level * FarmAnimal.FRIENDSHIP_STEP;
        }

        public void BecomeAnAdult()
        {
            this.age.Value = this.ageWhenMature.Value;
            this.Sprite = this.DetermineSprite(this.Sprite.SpriteWidth, this.Sprite.SpriteHeight, Paritee.StardewValleyAPI.FarmAnimals.Sprite.STARTING_FRAME);
        }

        public void AssignParent(long parentID)
        {
            this.parentId.Set(parentID);
        }

        private bool SetFieldsFromData()
        {
            Data FarmAnimalData = new Data();

            // Grab the data
            Dictionary<string, string> Data = FarmAnimalData.GetEntries();

            string Value;

            if (!Data.TryGetValue(this.type, out Value))
                return false;

            string[] DataArr = FarmAnimalData.Split(Value);

            ((NetFieldBase<byte, NetByte>)this.daysToLay).Set(Byte.Parse(DataArr[0]));
            ((NetFieldBase<byte, NetByte>)this.ageWhenMature).Set(Byte.Parse(DataArr[1]));
            ((NetFieldBase<int, NetInt>)this.defaultProduceIndex).Set(Int32.Parse(DataArr[Paritee.StardewValleyAPI.FarmAnimals.Data.DEFAULT_PRODUCE_INDEX]));
            ((NetFieldBase<int, NetInt>)this.deluxeProduceIndex).Set(Int32.Parse(DataArr[Paritee.StardewValleyAPI.FarmAnimals.Data.DELUXE_PRODUCE_INDEX]));
            ((NetFieldBase<string, NetString>)this.sound).Set(DataArr[4].Equals(FarmAnimal.NONE) ? (string)null : DataArr[4]);
            ((NetFieldBase<Rectangle, NetRectangle>)this.frontBackBoundingBox).Set(new Rectangle(Int32.Parse(DataArr[5]), Int32.Parse(DataArr[6]), Int32.Parse(DataArr[7]), Int32.Parse(DataArr[8])));
            ((NetFieldBase<Rectangle, NetRectangle>)this.sidewaysBoundingBox).Set(new Rectangle(Int32.Parse(DataArr[9]), Int32.Parse(DataArr[10]), Int32.Parse(DataArr[11]), Int32.Parse(DataArr[12])));
            ((NetFieldBase<byte, NetByte>)this.harvestType).Set(Byte.Parse(DataArr[13]));
            ((NetFieldBase<bool, NetBool>)this.showDifferentTextureWhenReadyForHarvest).Set(Boolean.Parse(DataArr[14]));
            ((NetFieldBase<string, NetString>)this.buildingTypeILiveIn).Set(DataArr[15]);
            this.Sprite = this.DetermineSprite(Int32.Parse(DataArr[16]), Int32.Parse(DataArr[17]), Paritee.StardewValleyAPI.FarmAnimals.Sprite.STARTING_FRAME);
            ((NetFieldBase<Rectangle, NetRectangle>)this.frontBackSourceRect).Set(new Rectangle(0, 0, Int32.Parse(DataArr[16]), Int32.Parse(DataArr[17])));
            ((NetFieldBase<Rectangle, NetRectangle>)this.sidewaysSourceRect).Set(new Rectangle(0, 0, Int32.Parse(DataArr[18]), Int32.Parse(DataArr[19])));
            ((NetFieldBase<byte, NetByte>)this.fullnessDrain).Set(Byte.Parse(DataArr[20]));
            ((NetFieldBase<byte, NetByte>)this.happinessDrain).Set(Byte.Parse(DataArr[21]));
            ((NetFieldBase<byte, NetByte>)this.happiness).Set(byte.MaxValue);
            ((NetFieldBase<byte, NetByte>)this.fullness).Set(byte.MaxValue);
            ((NetFieldBase<string, NetString>)this.toolUsedForHarvest).Set(DataArr[22].Equals(FarmAnimal.NONE) ? "" : DataArr[22]);
            ((NetFieldBase<int, NetInt>)this.meatIndex).Set(Int32.Parse(DataArr[23]));
            ((NetFieldBase<int, NetInt>)this.price).Set(Int32.Parse(DataArr[24]));

            return true;
        }

        public AnimatedSprite DetermineSprite(int width, int height, int frame = Paritee.StardewValleyAPI.FarmAnimals.Sprite.STARTING_FRAME)
        {
            return new AnimatedSprite(this.DetermineSpriteFilePath(), frame, width, height);
        }

        private string DetermineSpriteFilePath()
        {
            return (new Sprite(this)).DetermineFilePath();
        }

        public FarmAnimal RandomizeLocation(AnimalHouse animalHouse)
        {
            Building Building = animalHouse.getBuilding();

            this.home = Building;
            this.homeLocation.Set(new Vector2((float)Building.tileX, (float)Building.tileY));
            this.setRandomPosition(this.home.indoors);

            return this;
        }

        public StardewValley.FarmAnimal AddToAnimalHouse(AnimalHouse animalHouse)
        {
            // !!! @WARNING
            // !!! We have to convert to a base Farm Animal due to exceptions 
            // !!! thrown by the day's save XML functions
            StardewValley.FarmAnimal BaseFarmAnimal = this.ToFarmAnimal();

            animalHouse.animals.Add(BaseFarmAnimal.myID, BaseFarmAnimal);
            animalHouse.animalsThatLiveHere.Add(BaseFarmAnimal.myID);

            return BaseFarmAnimal;
        }

        public StardewValley.FarmAnimal ToFarmAnimal()
        {
            // Create a new resource
            StardewValley.FarmAnimal BaseFarmAnimal = new StardewValley.FarmAnimal(this.type, this.myID, this.ownerID);

            // Copy the identification
            ((NetFieldBase<string, NetString>)BaseFarmAnimal.type).Set(this.type);
            ((NetFieldBase<long, NetLong>)BaseFarmAnimal.myID).Set(this.myID);
            ((NetFieldBase<long, NetLong>)BaseFarmAnimal.ownerID).Set(this.ownerID);

            // Copy the name
            BaseFarmAnimal.Name = this.Name;
            BaseFarmAnimal.displayName = this.displayName;

            // Copy the biology
            ((NetFieldBase<int, NetInt>)BaseFarmAnimal.health).Set(this.health);
            ((NetFieldBase<long, NetLong>)BaseFarmAnimal.parentId).Set(this.parentId);

            // Copy the data
            ((NetFieldBase<byte, NetByte>)BaseFarmAnimal.daysToLay).Set(this.daysToLay);
            ((NetFieldBase<byte, NetByte>)BaseFarmAnimal.ageWhenMature).Set(this.ageWhenMature);
            ((NetFieldBase<int, NetInt>)BaseFarmAnimal.defaultProduceIndex).Set(this.defaultProduceIndex);
            ((NetFieldBase<int, NetInt>)BaseFarmAnimal.deluxeProduceIndex).Set(this.deluxeProduceIndex);
            ((NetFieldBase<string, NetString>)BaseFarmAnimal.sound).Set(this.sound);
            ((NetFieldBase<Rectangle, NetRectangle>)BaseFarmAnimal.frontBackBoundingBox).Set(this.frontBackBoundingBox);
            ((NetFieldBase<Rectangle, NetRectangle>)BaseFarmAnimal.sidewaysBoundingBox).Set(this.sidewaysBoundingBox);
            ((NetFieldBase<byte, NetByte>)BaseFarmAnimal.harvestType).Set(this.harvestType);
            ((NetFieldBase<bool, NetBool>)BaseFarmAnimal.showDifferentTextureWhenReadyForHarvest).Set(this.showDifferentTextureWhenReadyForHarvest);
            ((NetFieldBase<string, NetString>)BaseFarmAnimal.buildingTypeILiveIn).Set(this.buildingTypeILiveIn);
            BaseFarmAnimal.Sprite = this.Sprite;
            ((NetFieldBase<Rectangle, NetRectangle>)BaseFarmAnimal.frontBackSourceRect).Set(this.frontBackSourceRect);
            ((NetFieldBase<Rectangle, NetRectangle>)BaseFarmAnimal.sidewaysSourceRect).Set(this.sidewaysSourceRect);
            ((NetFieldBase<byte, NetByte>)BaseFarmAnimal.fullnessDrain).Set(this.fullnessDrain);
            ((NetFieldBase<byte, NetByte>)BaseFarmAnimal.happinessDrain).Set(this.happinessDrain);
            ((NetFieldBase<byte, NetByte>)BaseFarmAnimal.happiness).Set(this.happiness);
            ((NetFieldBase<byte, NetByte>)BaseFarmAnimal.fullness).Set(this.fullness);
            ((NetFieldBase<string, NetString>)BaseFarmAnimal.toolUsedForHarvest).Set(this.toolUsedForHarvest);
            ((NetFieldBase<int, NetInt>)BaseFarmAnimal.meatIndex).Set(this.meatIndex);
            ((NetFieldBase<int, NetInt>)BaseFarmAnimal.price).Set(this.price);

            // Copy the location information
            BaseFarmAnimal.home = this.home;
            BaseFarmAnimal.homeLocation.Set(this.homeLocation);

            if (this.home != null)
            {
                // Reset their position
                // !!! They will get stuck not moving if this is just copied over
                BaseFarmAnimal.setRandomPosition(this.home.indoors);
            }

            // Copy the misc information
            ((NetFieldBase<int, NetInt>)BaseFarmAnimal.friendshipTowardFarmer).Set(this.friendshipTowardFarmer);
            ((NetFieldBase<int, NetInt>)BaseFarmAnimal.daysSinceLastFed).Set(this.daysSinceLastFed);
            BaseFarmAnimal.uniqueFrameAccumulator = this.uniqueFrameAccumulator;
            ((NetFieldBase<int, NetInt>)BaseFarmAnimal.age).Set(this.age);
            ((NetFieldBase<int, NetInt>)BaseFarmAnimal.produceQuality).Set(this.produceQuality);
            ((NetFieldBase<byte, NetByte>)BaseFarmAnimal.daysSinceLastLay).Set(this.daysSinceLastLay);
            ((NetFieldBase<bool, NetBool>)BaseFarmAnimal.wasPet).Set(this.wasPet);
            ((NetFieldBase<bool, NetBool>)BaseFarmAnimal.allowReproduction).Set(this.allowReproduction);
            ((NetFieldBase<int, NetInt>)BaseFarmAnimal.moodMessage).Set(this.moodMessage);

            return BaseFarmAnimal;
        }

    }
}
