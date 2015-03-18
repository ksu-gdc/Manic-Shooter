#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using TextPackage;
using Manic_Shooter.Classes;
using System.Timers;
using EntityComponentSystem.Systems;
using EntityComponentSystem.Components;
using EntityComponentSystem.Structure;
#endregion

namespace Manic_Shooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ManicShooter : Game
    {
        //Example Action code for quick anonymous methods:
            //Action test = () =>
            //{
            //    position.Point.X += 100;
            //};

            //test();

        public static uint PlayerID;

        public static Rectangle ScreenSize;
        public static Vector2 playerPosition;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        FontRenderer fontRenderer;

        DefaultPlayer player1;
        DefaultEnemy enemy1;
        TriangleEnemy enemy2;
        HunterEnemy enemy3;
        DefaultProjectile projectile1;

        bool isPaused = false;
        GameTime inGameTotalTime;

        public enum gameStates { Menu, Play, GameOver };

        enum MenuStates { Main, About };

        private MenuStates MenuState = MenuStates.Main;

        private int MenuIndex = 0;

        public static gameStates GameState = gameStates.Play;

        public ManicShooter()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;

            ScreenSize = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            playerPosition = new Vector2(0, 0);
            
            Content.RootDirectory = "Content";
            inGameTotalTime = new GameTime();

            KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.Pause, gameKey_pausePressed);
            KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.SpawnDefaultEnemy, gameKey_spawnDefault);
            KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.SpawnTriangleEnemy, gameKey_spawnTriangle);
            KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.SpawnHunterEnemy, gameKey_spawnHunter);
        }

        private void SpawnEnemy()
        {
            //spawn an enemy
            ResourceManager.Instance.AddEnemy(new DefaultEnemy(TextureManager.Instance.GetTexture("DefaultEnemy"), new Vector2(-50, -50), 3));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            AssetSystem.Instance.InitializeContent(Content);

            ComponentManagementSystem.Instance.AddComponent(typeof(RenderComponent));
            ComponentManagementSystem.Instance.AddComponent(typeof(PositionComponent));
            ComponentManagementSystem.Instance.AddComponent(typeof(RotationComponent));
            ComponentManagementSystem.Instance.AddComponent(typeof(MovementComponent));

            AssetSystem.Instance.AddAsset(AssetType.Texture, "Enemy1", @"enemy1");
            AssetSystem.Instance.AddAsset(AssetType.Texture, "Player", @"player2");
            AssetSystem.Instance.AddAsset(AssetType.Texture, "Pellet", @"Projectile_placeholder");
            AssetSystem.Instance.AddAsset(AssetType.Texture, "Enemy2", @"Enemy_placeholder");

            AssetSystem.Instance.AddAsset(AssetType.SpriteFont, "HUDFont", @"HUDFont");
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetSystem.Instance.GetTexture("Player");
            AssetSystem.Instance.GetTexture("Pellet");
            AssetSystem.Instance.GetSpriteFont("HUDFont");

            PlayerID = ComponentFactory.Instance.CreatePlayer();
            
            //Texture2D hitboxTexture = new Texture2D(GraphicsDevice, 1, 1);
            //hitboxTexture.SetData(new Color[] { Color.Red });

            //TextureManager.Instance.AddTexture("Hitbox", TextureManager.Instance.createCircleText(GraphicsDevice, 40, Color.Red));


            //player1 = new DefaultPlayer(TextureManager.Instance.GetTexture("DefaultPlayer"), new Vector2(300, 300));
            //projectile1 = new DefaultProjectile(TextureManager.Instance.GetTexture("DefaultProjectile"), new Vector2(200, 200), new Vector2(0, 120), 3);

            //ResourceManager.Instance.AddPlayer(player1);
            //ResourceManager.Instance.AddProjectile(projectile1);
            
            //player1.ScaleSize((decimal)0.5);
            //enemy1.ScaleSize((decimal)2);
            //projectile1.ScaleSize((decimal)0.5);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            AssetSystem.Instance.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            /*
            KeyboardManager.Instance.Update(gameTime);

            playerPosition = player1.Position;

            switch (GameState)
            {
                case gameStates.Menu:
                    UpdateMenu(gameTime);
                    break;
                case gameStates.Play:
                    UpdatePlay(gameTime);
                    if (player1.isGameOver)
                        GameState = gameStates.GameOver;
                    break;
                case gameStates.GameOver:
                    UpdateGameOver(gameTime);
                    break;
            }
            */
            InputSystem.Instance.Update(gameTime);
            MovementSystem.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        private void UpdatePlay(GameTime gameTime)
        {
            if (isPaused)
                return;

            inGameTotalTime.ElapsedGameTime += gameTime.ElapsedGameTime;

            //demo code
            if (!(ResourceManager.Instance.ActivePlayerList.Count == 0))
            {
                if(gameTime.TotalGameTime.TotalMilliseconds % 4000 <= 5) 
                    SpawnEnemy();
            }

            // TODO: Add your update logic here
            ResourceManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        private void UpdateMenu(GameTime gameTime)
        {

        }

        private void UpdateGameOver(GameTime gameTime)
        {

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            /*
            switch(GameState)
            {
                case gameStates.Play:
                    DrawPlay(gameTime);
                    break;
                case gameStates.Menu:
                    DrawMenu(gameTime);
                    break;
                case gameStates.GameOver:
                    DrawGameOver(gameTime);
                    break;
            }
            */

            RenderSystem.Instance.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }

        private void DrawPlay(GameTime gameTime)
        {
            spriteBatch.Begin();

            //fontRenderer.DrawText(spriteBatch, 50, 50, "Hello World!\nGame Time\t=\t" + inGameTotalTime.ElapsedGameTime.ToString());
            ResourceManager.Instance.RenderSprites(spriteBatch);

            fontRenderer.DrawText(spriteBatch, 20, 20, "Lives = " + player1.Lives);
            fontRenderer.DrawText(spriteBatch, ScreenSize.Width - 150, 20, "Health = " + player1.Health + "/" + player1.MaxHealth);
            if(isPaused)
                fontRenderer.DrawText(spriteBatch, 200, 200, "Pawsed :3");

            spriteBatch.End();
        }

        private void DrawMenu(GameTime gameTime)
        {
            spriteBatch.Begin();

            fontRenderer.DrawText(spriteBatch, 100, 100, "Play Game");
            fontRenderer.DrawText(spriteBatch, 100, 200, "About");
            fontRenderer.DrawText(spriteBatch, 100, 300, "Quit");


            fontRenderer.DrawText(spriteBatch, 80, 100 + 100 * MenuIndex, ">");

            spriteBatch.End();
        }

        private void DrawGameOver(GameTime gameTime)
        {
            spriteBatch.Begin();
            fontRenderer.DrawText(spriteBatch, ScreenSize.Width / 2 - 50, ScreenSize.Height / 2 - 5, "Game Over");
            spriteBatch.End();
        }

        public void gameKey_pausePressed(Keys key)
        {
            if (GameState == gameStates.Play)
                isPaused = !isPaused;
            //This will create a bug where the timer updates bad times from unpause to pause
        }

        public void gameKey_spawnDefault(Keys key)
        {
            EnemySpawner.Instance.SpawnDefaultEnemy();
        }

        public void gameKey_spawnTriangle(Keys key)
        {
            EnemySpawner.Instance.SpawnTriangleEnemy();
        }

        public void gameKey_spawnHunter(Keys key)
        {
            EnemySpawner.Instance.SpawnHunterEnemy();
        }

        private void GameStateSwitched()
        {
            switch (GameState)
            {
                case gameStates.Play:
                    InitPlayState();
                    break;
                case gameStates.Menu:
                    InitMenuState();
                    break;
                case gameStates.GameOver:
                    InitGameOverState();
                    break;
            }
        }

        private void InitPlayState()
        {

        }

        private void InitMenuState()
        {
            MenuState = MenuStates.Main;

            MenuIndex = 0;
        }

        private void InitGameOverState()
        {

        }
    }
}
