using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Manic_Shooter.Interfaces;

namespace Manic_Shooter.Classes
{
    public enum SpawnerMode
    {
        None,
        Constant
    }
    public class MobSpawner
    {
        private static MobSpawner _instance;
        private List<MobSet> mobSetList;
        private List<MobSet> activeMobSets;
        private SpawnerMode currentMode;

        public List<MobSet> MobSetList
        {
            get{return mobSetList;}
        }

        public static MobSpawner Instance
        {
            get
            {
                if (_instance == null) _instance = new MobSpawner();
                return _instance;
            }
        }

        //Reads in XML file for mobsets
        /*
         *  <Mobset>
         *      <Mob type="Slider" health="3" spawnPositionX="-50" spawnPositionX="-50" entryPointX="0" entryPoint="0">
         *              //Required, Either "Slider","Popper", or "Hunter"
         *          <Type>(Slider|Popper|Hunter)</Type>
         *              //Required
         *          <Health></Health>
         *              //Optional - Default 0 if only one is provided
         *          <SpawnPositionX></SpawnPositionX>
         *              //Optional - Default 0 if only one is provided
         *          <SpawnPositionY></SpawnPositionY>
         *              //Optional - Default random if only one is provided
         *          <EntryPointX></EntryPointX>
         *              //Optional - Default random if only one is provided
         *          <EntryPointY></EntryPointY>
         *      </Mob>
         *  </Mobset>
         */
        public MobSpawner()
        {
            mobSetList = new List<MobSet>();
            activeMobSets = new List<MobSet>();
            currentMode = SpawnerMode.None;

            XmlDocument doc = new XmlDocument();
            doc.Load("Content/Mobset.xml");

            string type = "";
            Random rng = new Random();
            int health, spawnPositionX = -50, spawnPositionY = -50;
            int entryPointX = rng.Next(ManicShooter.ScreenSize.Left, ManicShooter.ScreenSize.Right);
            int entryPointY = rng.Next(-50, ManicShooter.ScreenSize.Top);
            bool hasEntryPoint = false;
            Vector2 spawnPosition, entryPoint = Vector2.Zero;

            foreach(XmlNode mobSet in doc.DocumentElement.ChildNodes)
            {
                MobSet newMobSet = new MobSet();
                foreach (XmlNode mob in mobSet.ChildNodes)
                {
                    try
                    {
                        type = mob.Attributes["type"].InnerText;
                        health = Int32.Parse(mob.Attributes["health"].InnerText);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("XML data not formatted correctly, please revise.");
                    }

                    if (mob.Attributes["spawnPositionX"] != null)
                    {
                        spawnPositionX = Int32.Parse(mob.Attributes["spawnPositionX"].InnerText);
                    }

                    if (mob.Attributes["spawnPositionY"] != null)
                    {
                        spawnPositionY = Int32.Parse(mob.Attributes["spawnPositionY"].InnerText);
                    }

                    if (mob.Attributes["entryPointX"] != null)
                    {
                        hasEntryPoint = true;
                        entryPointX = Int32.Parse(mob.Attributes["entryPointX"].InnerText);
                    }

                    if (mob.Attributes["entryPointY"] != null)
                    {
                        hasEntryPoint = true;
                        entryPointY = Int32.Parse(mob.Attributes["entryPointY"].InnerText);
                    }

                    spawnPosition = new Vector2(spawnPositionX, spawnPositionY);
                    if (hasEntryPoint)
                    {
                        entryPoint = new Vector2(entryPointX, entryPointY);
                        newMobSet.AddEnemy(type, spawnPosition, health, entryPoint);
                    }
                    else
                    {
                        newMobSet.AddEnemy(type, spawnPosition, health);
                    }
                }
                mobSetList.Add(newMobSet);
            }
        }

        //Optional: link list with key numbers for testing
        //Constant mode: automatically spawns a new random mob when the current mob dies

        public void SetSpawnerMode(SpawnerMode newMode)
        {
            currentMode = newMode;
        }

        public MobSet SpawnMobSet(int index)
        {
            index = MathHelper.Clamp(index, 0, mobSetList.Count-1);

            activeMobSets.Add(mobSetList[index]);
            mobSetList[index].Spawn();

            return mobSetList[index];
        }

        public void Update(GameTime gameTime)
        {
            switch(currentMode)
            {
                case SpawnerMode.None:
                    {
                        break;
                    }
                case SpawnerMode.Constant:
                    {
                        if(activeMobSets.Count == 1)
                        {
                            if(activeMobSets[0].IsDead())
                            {
                                activeMobSets.Clear();
                            }
                        }
                        
                        if(activeMobSets.Count == 0)
                        {
                            Random rng = new Random();
                            int newMobSetIndex = rng.Next(mobSetList.Count);

                            this.SpawnMobSet(newMobSetIndex);
                        }

                        break;
                    }
            }
            
            for(int i = 0; i < activeMobSets.Count; i++)
            {
                if(activeMobSets[i].IsDead())
                    activeMobSets.RemoveAt(i--);
            }
        }
        
        public void Reset()
        {
            for (int i = 0; i < activeMobSets.Count; i++)
            {
                activeMobSets[i].Reset();
            }
        }

    }

    public class MobSet
    {
        private List<MobDefinition> mobList;
        private List<IEnemy> spawnList;

        public MobSet()
        {
            mobList = new List<MobDefinition>();
            spawnList = new List<IEnemy>();
        }

        public void AddEnemy(MobDefinition newEnemy)
        {
            mobList.Add(newEnemy);
        }

        public void AddEnemy(String enemyType, Vector2 spawnPosition, int health)
        {
            AddEnemy(new MobDefinition(enemyType, spawnPosition, health));
        }
        public void AddEnemy(String enemyType, Vector2 spawnPosition, int health, Vector2 entryPosition)
        {
            AddEnemy(new MobDefinition(enemyType, spawnPosition, health, entryPosition));
        }

        public void Spawn()
        {
            foreach(MobDefinition md in mobList)
            {
                IEnemy enemy = md.Spawn();
                spawnList.Add(enemy);
                ResourceManager.Instance.AddEnemy(enemy);
            }
        }

        public bool IsDead()
        {
            foreach(IEnemy e in spawnList)
            {
                if(e != null && e.Health > 0)
                    return false;
            }

            return true;
        }

        public void RemoveEnemy(MobDefinition enemy)
        {
            if (!mobList.Contains(enemy))
                mobList.Remove(enemy);
        }

        public void Reset()
        {
            spawnList.Clear();
        }
    }

    public class MobDefinition
    {
        public String Type;
        public int Health;
        public Vector2 SpawnPosition;
        private Vector2 entryPosition;
        private bool hasEntryPosition;

        public Vector2? EntryPosition
        {
            get
            {
                if(!hasEntryPosition)
                    return null;
                return entryPosition;
            }
        }

        public MobDefinition(String enemyType, Vector2 spawnPosition, int health)
        {
            Type = enemyType;
            SpawnPosition = spawnPosition;
            Health = health;
            hasEntryPosition = false;
        }
        
        public MobDefinition(String enemyType, Vector2 spawnPosition, int health, Vector2 entryPosition)
        {
            Type = enemyType;
            SpawnPosition = spawnPosition;
            Health = health;
            this.entryPosition = entryPosition;
            hasEntryPosition = true;
        }

        public IEnemy Spawn()
        {
            switch(this.Type)
            {
                case "Slider":
                    if(hasEntryPosition)
                        return new DefaultEnemy(TextureManager.Instance.GetTexture("DefaultEnemy"), this.SpawnPosition, this.Health, this.entryPosition);
                    else
                        return new DefaultEnemy(TextureManager.Instance.GetTexture("DefaultEnemy"), this.SpawnPosition, this.Health);
                    break;
                case "Popper":
                    if(hasEntryPosition)
                        return new TriangleEnemy(TextureManager.Instance.GetTexture("TriangleEnemy"), this.SpawnPosition, this.Health, this.entryPosition);
                    else
                        return new TriangleEnemy(TextureManager.Instance.GetTexture("TriangleEnemy"), this.SpawnPosition, this.Health);
                    break;
                case "Hunter":
                    if(hasEntryPosition)
                        return new HunterEnemy(TextureManager.Instance.GetTexture("HunterEnemy"), this.SpawnPosition, this.Health, this.entryPosition);
                    else
                        return new HunterEnemy(TextureManager.Instance.GetTexture("HunterEnemy"), this.SpawnPosition, this.Health);
                    break;
                default:
                    throw new Exception("Unexpected enemy type detected");
            }
        }
    }
}
