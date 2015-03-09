using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Manic_Shooter.Classes;

namespace Manic_Shooter
{
    class EnemySpawner
    {
        private static EnemySpawner _instance;

        public static EnemySpawner Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new EnemySpawner();
                }

                return _instance;
            }
        }

        public EnemySpawner()
        {

        }

        public DefaultEnemy SpawnDefaultEnemy(int health = 3)
        {
            DefaultEnemy enemy =  new DefaultEnemy(TextureManager.Instance.GetTexture("DefaultEnemy"), new Vector2(-50, -50), health);
            ResourceManager.Instance.AddEnemy(enemy);
            return enemy;
        }

        public TriangleEnemy SpawnTriangleEnemy(int health = 3)
        {
            TriangleEnemy enemy =  new TriangleEnemy(TextureManager.Instance.GetTexture("TriangleEnemy"), new Vector2(-50, -50), health);
            ResourceManager.Instance.AddEnemy(enemy);
            return enemy;
        }

        public HunterEnemy SpawnHunterEnemy(int health = 3)
        {
            HunterEnemy enemy =  new HunterEnemy(TextureManager.Instance.GetTexture("HunterEnemy"), new Vector2(-50, -50), health);
            ResourceManager.Instance.AddEnemy(enemy);
            return enemy;
        }
    }
}
