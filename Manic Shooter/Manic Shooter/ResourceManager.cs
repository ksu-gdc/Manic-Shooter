using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Manic_Shooter.Classes;
using Manic_Shooter.Interfaces;

namespace Manic_Shooter
{
    class ResourceManager
    {
        #region List fields and Properties

        /// <summary>
        /// Singleton class holder. I have a thing for
        ///  singletons, unfortunately.
        /// </summary>
        private static ResourceManager _instance = null;


        /// <summary>
        /// Singleton access to the ResourceManager class. Allows any
        ///  object to acces the same ResourceManager through ResourceManager.Instance
        /// </summary>
        public static ResourceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ResourceManager();
                }

                return _instance;
            }
        }

        /// <summary>
        /// A list of all players in the game, will likely be just 1 but it doesn't hurt too much
        /// in this case to keep the option around.
        /// </summary>
        private List<IPlayer> playerList;

        /// <summary>
        /// A list of all enemies in the game.
        /// </summary>
        private List<IEnemy> enemyList;

        /// <summary>
        /// A list of all projectiles in the game
        /// </summary>
        private List<IProjectile> projectileList;

        /// <summary>
        /// A list of renderable objects in the game that are not included in the 
        /// player, enemy, or projectile lists (i.e. backgrounds, star maps, etc.), whether they are on the screen or not. 
        /// When a renderable object leaves
        /// the screen buffer area it should be despawned from this list
        /// </summary>
        private List<IRenderable> renderList;

        /// <summary>
        /// Gets the list of active players
        /// </summary>
        public List<IPlayer> ActivePlayerList { get { return playerList.FindAll(x => x.IsActive == true); } }

        /// <summary>
        /// Gets the list of active enemies
        /// </summary>
        public List<IEnemy> ActiveEnemyList { get { return enemyList.FindAll(x => x.IsActive == true); } }

        /// <summary>
        /// Gets the list of active projectiles
        /// </summary>
        public List<IProjectile> ActiveProjectileList { get { return projectileList.FindAll(x => x.IsActive == true); } }

        /// <summary>
        /// Gets the list of active renderable objects
        /// </summary>
        public List<IRenderable> ActiveRenderableList { get { return renderList.FindAll(x => x.IsActive == true); } }

        #endregion 

        #region Add Element

        /// <summary>
        /// Adds a new player to the game
        /// </summary>
        /// <param name="newPlayer">The new player to add</param>
        public void AddPlayer(IPlayer newPlayer)
        {

            if (playerList.Exists(x => x.IsActive == false))
            {
                int index = playerList.FindIndex(x => x.IsActive == false);
                playerList[index] = newPlayer;
            }
            else
                playerList.Add(newPlayer);
        }

        /// <summary>
        /// Adds a new enemy to the game
        /// </summary>
        /// <param name="newEnemy">The new enemy to add</param>
        public void AddEnemy(IEnemy newEnemy)
        {
            if (enemyList.Exists(x => x.IsActive == false))
            {
                int index = enemyList.FindIndex(x => x.IsActive == false);
                enemyList[index] = newEnemy;
            }
            else
                enemyList.Add(newEnemy);
        }

        /// <summary>
        /// Adds a new projectile to the game
        /// </summary>
        /// <param name="newProjectile">The new projectile to add</param>
        public void AddProjectile(IProjectile newProjectile)
        {
            if (projectileList.Exists(x => x.IsActive == false))
            {
                int index = projectileList.FindIndex(x => x.IsActive == false);
                projectileList[index] = newProjectile;
            }
            else
                projectileList.Add(newProjectile);
        }

        /// <summary>
        /// Adds the sprite to the spriteList
        /// </summary>
        /// <param name="sprite">sprite to add</param>
        public void AddRenderableObject(IRenderable renderableObject)
        {
            if (renderList.Exists(x => x.IsActive == false))
            {
                IRenderable objectToReuse = renderList.Find(x => x.IsActive == false);
                objectToReuse = renderableObject;
            }
            else
                renderList.Add(renderableObject);
        }

        #endregion 

        //TODO: Test if it would be faster to use Linq (like what is currently implemented) or 
        //To just use the List<T>.Remove method. My guess is that Linq is slower, but I don't think it's by any significant amount
        //and this implementation at least guarantees that all duplicate instances will be removed, even though there shouldn't be
        //any in the first place

        #region Remove Element

        /// <summary>
        /// Removes a player from the list
        /// </summary>
        /// <param name="playerToRemove">The player to remove</param>
        public void RemovePlayer(IPlayer playerToRemove)
        {
            playerList.RemoveAll(x => x.Equals(playerToRemove));
        }

        /// <summary>
        /// Removes an enemy from the list
        /// </summary>
        /// <param name="enemyToRemove">The enemy to remove</param>
        public void RemoveEnemy(IEnemy enemyToRemove)
        {
            enemyList.RemoveAll(x => x.Equals(enemyToRemove));
        }

        /// <summary>
        /// Removes a projectile from the list
        /// </summary>
        /// <param name="projectileToRemove">The projectile to remove</param>
        public void RemoveProjectile(IProjectile projectileToRemove)
        {
            projectileList.RemoveAll(x => x.Equals(projectileList));
        }

        /// <summary>
        /// Removes a renderable object from the list
        /// </summary>
        /// <param name="renderableToRemove">The renderableObject to remove</param>
        public void RemoveRenderable(IRenderable renderableToRemove)
        {
            renderList.RemoveAll(x => x.Equals(renderableToRemove));
        }

        #endregion

        /// <summary>
        /// Cleans inactive elements from the player, enemy, projectile, and render lists. 
        /// Should not be called too often as it can be expensive. This is effectively
        /// a garbage collector
        /// </summary>
        public void CleanUpLists()
        {
            renderList.RemoveAll(x => x.IsActive == false);
            playerList.RemoveAll(x => x.IsActive == false);
            enemyList.RemoveAll(x => x.IsActive == false);
            projectileList.RemoveAll(x => x.IsActive == false);
        }

        /// <summary>
        /// Renders the lists in the order of renderable objects, players, enemies, and the projectiles.
        /// Should be called between spriteBatch.Begin() and spriteBatch.End() calls
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use for the drawingc</param>
        public void RenderSprites(SpriteBatch spriteBatch)
        {
            List<IRenderable> visibleRenderableObjects = renderList.FindAll(x => x.Visible == true && ((IRenderable)x).IsActive == true);
            List<IPlayer> visiblePlayers = playerList.FindAll(x => ((IRenderable)x).Visible == true && ((IRenderable)x).IsActive == true);
            List<IEnemy> visibleEnemies = enemyList.FindAll(x => ((IRenderable)x).Visible == true && ((IRenderable)x).IsActive == true);
            List<IProjectile> visibleProjectiles = projectileList.FindAll(x => ((IRenderable)x).Visible == true && ((IRenderable)x).IsActive == true);

            foreach (IRenderable r in visibleRenderableObjects)
            {
                r.Render(spriteBatch);
            }

            foreach (IRenderable r in visiblePlayers)
            {
                r.Render(spriteBatch);
            }

            foreach (IRenderable r in visibleEnemies)
            {
                r.Render(spriteBatch);
            }

            foreach (IRenderable r in visibleProjectiles)
            {
                r.Render(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (IPlayer p in playerList)
            {
                if(p.IsActive)
                    p.Update(gameTime);
            }

            foreach (IEnemy e in enemyList)
            {
                if (e.IsActive)
                    e.Update(gameTime);
            }

            foreach (IProjectile p in projectileList)
            {
                if (p.IsActive)
                    p.Update(gameTime);
            }

            CheckCollisions();

        }

        private void CheckCollisions()
        {
            //Check player/enemy collisions
            foreach (IPlayer p in playerList)
            {
                foreach (IEnemy e in enemyList)
                {
                    if (Vector2.Distance(p.HitBoxCenter, e.HitBoxCenter) < p.HitBoxRadius + e.HitBoxRadius)
                    {
                        //kill player? kill enemy?

                    }
                }
            }

            foreach (IProjectile p in projectileList)
            {
                if (!p.IsActive) continue;

                //Check bullet/player collisions
                if (!p.IsPlayerProjectile())
                {
                    foreach (IPlayer pl in playerList)
                    {
                        if (!pl.IsActive) continue;

                        if (Vector2.Distance(p.HitBoxCenter, pl.HitBoxCenter) < p.HitBoxRadius + pl.HitBoxRadius)
                        {
                            pl.Health -= p.GetDamage();
                            pl.HitBy(p);

                            if (pl.Health <= 0)
                                pl.Destroy();

                            p.Destroy();
                        }
                    }
                }
                else
                {
                    //Check bullet/enemy collisions
                    foreach (IEnemy e in enemyList)
                    {
                        if (!e.IsActive) continue;

                        if (Vector2.Distance(p.HitBoxCenter, e.HitBoxCenter) < p.HitBoxRadius + e.HitBoxRadius)
                        {
                            e.Health -= p.GetDamage();
                            if (e.Health <= 0)
                                e.Destroy();

                            p.Destroy();
                        }
                    }
                }
            }
        }

        private ResourceManager()
        {
            playerList = new List<IPlayer>();
            enemyList = new List<IEnemy>();
            projectileList = new List<IProjectile>();
            renderList = new List<IRenderable>();
        }
    }
}
