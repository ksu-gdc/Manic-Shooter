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
using Microsoft.Xna.Framework.Media;
#endregion

namespace Manic_Shooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ManicShooter : Game
    {
        public static Rectangle ScreenSize;
        public static Vector2 playerPosition;
        public static Random RNG;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        FontRenderer fontRenderer;

        DefaultPlayer player1;
        DefaultEnemy enemy1;
        TriangleEnemy enemy2;
        HunterEnemy enemy3;
        DefaultProjectile projectile1;

        bool isPaused = false;

        Texture2D titleTexture;
        Texture2D menuSelectionTexture;

        GameTime inGameTotalTime;

        protected Song song;

        public enum gameStates { Menu, Play, GameOver };

        enum MenuStates { Main, About };

        private MenuStates MenuState = MenuStates.Main;

        private int MenuIndex = 0;

        public gameStates GameState { set { _gameState = value; GameStateSwitched(); } get { return _gameState; } }
        public gameStates _gameState = gameStates.Menu;

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

            RNG = new Random();

            KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.Pause, gameKey_pausePressed);
            KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.SpawnDefaultEnemy, gameKey_spawnDefault);
            KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.SpawnTriangleEnemy, gameKey_spawnTriangle);
            KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.SpawnHunterEnemy, gameKey_spawnHunter);
            KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.SpawnMobSet1, gameKey_spawnMob1);
            KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.SpawnMobSet2, gameKey_spawnMob2);
            KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.SpawnMobSet3, gameKey_spawnMob3);

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

            fontRenderer = new FontRenderer(Content, "Times New Roman");

            TextureManager.Instance.AddTexture("DefaultPlayer", Content.Load<Texture2D>("player2.png"));
            TextureManager.Instance.AddTexture("DefaultEnemy", Content.Load<Texture2D>("newEnemy1.png"));
            TextureManager.Instance.AddTexture("TriangleEnemy", Content.Load<Texture2D>("newEnemy3.png"));
            TextureManager.Instance.AddTexture("DefaultProjectile", Content.Load<Texture2D>("bulletFull.png"));
            TextureManager.Instance.AddTexture("PlayerProjectile", Content.Load<Texture2D>("playerBulletFull.png"));
            TextureManager.Instance.AddTexture("MissileDroppable", Content.Load<Texture2D>("missileUpgrade.png"));
            TextureManager.Instance.AddTexture("GunDroppable", Content.Load<Texture2D>("gunUpgrade.png"));
            TextureManager.Instance.AddTexture("ScoreDroppable", Content.Load<Texture2D>("scoreUpgrade.png"));
            TextureManager.Instance.AddTexture("DefaultMissile", Content.Load<Texture2D>("missileFull.png"));
            TextureManager.Instance.AddTexture("HunterEnemy", Content.Load<Texture2D>("newEnemy2.png"));

            song = Content.Load<Song>("597237_Lets-Fight-Loop");

            titleTexture = Content.Load<Texture2D>("title.png");
            menuSelectionTexture = TextureManager.Instance.GetTexture("DefaultPlayer");

            //Texture2D hitboxTexture = new Texture2D(GraphicsDevice, 1, 1);
            //hitboxTexture.SetData(new Color[] { Color.Red });

            TextureManager.Instance.AddTexture("Hitbox", TextureManager.Instance.createCircleText(GraphicsDevice, 40, Color.Red));


            player1 = new DefaultPlayer(TextureManager.Instance.GetTexture("DefaultPlayer"), new Vector2(300, 300));
            //projectile1 = new DefaultProjectile(TextureManager.Instance.GetTexture("DefaultProjectile"), new Vector2(200, 200), new Vector2(0, 120), 3);

            ResourceManager.Instance.AddPlayer(player1);
            //ResourceManager.Instance.AddProjectile(projectile1);

            player1.ScaleSize((decimal)0.5);
            //enemy1.ScaleSize((decimal)2);
            //projectile1.ScaleSize((decimal)0.5);

            //HACK: pretty jank way of getting the MobSpawner singleton to instantiate itself
            MobSpawner.Instance.GetType();

            MobSpawner.Instance.SetSpawnerMode(SpawnerMode.Constant);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            KeyboardManager.Instance.Update(gameTime);
            MobSpawner.Instance.Update(gameTime);

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

            if (ResourceManager.Instance.ActivePlayerList.Count == 0)
            {
                this.GameState = gameStates.GameOver;
            }

            base.Update(gameTime);
        }

        bool _menuUpPressed = false;
        bool _menuDownPressed = false;
        bool _menuEnterPressed = false;

        private void UpdateMenu(GameTime gameTime)
        {
            if (KeyboardManager.Instance.IsKeyDown(KeyboardManager.GameKeys.MoveUp))
            {
                if (!_menuUpPressed)
                {
                    _menuUpPressed = true;
                    MenuIndex--;
                    if (MenuIndex < 0)
                        MenuIndex = 2;
                }
            }
            else
            {
                if (_menuUpPressed)
                {
                    _menuUpPressed = false;
                }
            }
            if (KeyboardManager.Instance.IsKeyDown(KeyboardManager.GameKeys.MoveDown))
            {
                if (!_menuDownPressed)
                {
                    _menuDownPressed = true;
                    MenuIndex++;
                    if (MenuIndex > 2)
                        MenuIndex = 0;
                }
            }
            else
            {
                if (_menuDownPressed)
                {
                    _menuDownPressed = false;
                }
            }
            
            if (KeyboardManager.Instance.IsKeyDown(KeyboardManager.GameKeys.Shoot))
            {
                if (!_menuEnterPressed)
                {
                    _menuEnterPressed = true;

                    ParseMenuAction();
                }
            }
            else
            {
                if (_menuEnterPressed)
                {
                    _menuEnterPressed = false;
                }
            }
        }

        private void ParseMenuAction()
        {
            if (this.MenuState == MenuStates.Main)
            {
                if (MenuIndex == 0)
                {
                    //Start game
                    GameState = gameStates.Play;
                }
                else if (MenuIndex == 1)
                {
                    //About page
                    this.MenuState = MenuStates.About;
                }
                else if (MenuIndex == 2)
                {
                    //Exit
                    this.Exit();
                }
            }
            else if (this.MenuState == MenuStates.About)
            {
                MenuIndex = 1;
                this.MenuState = MenuStates.Main;
            }
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

            base.Draw(gameTime);
        }

        private void DrawPlay(GameTime gameTime)
        {
            spriteBatch.Begin();

            //fontRenderer.DrawText(spriteBatch, 50, 50, "Hello World!\nGame Time\t=\t" + inGameTotalTime.ElapsedGameTime.ToString());
            ResourceManager.Instance.RenderSprites(spriteBatch);

            fontRenderer.DrawText(spriteBatch, 20, 20, "Lives = " + player1.Lives);
            fontRenderer.DrawText(spriteBatch, ScreenSize.Width / 2 - 80, 20, "Score = " + player1.Score);
            fontRenderer.DrawText(spriteBatch, ScreenSize.Width - 150, 20, "Health = " + player1.Health + "/" + player1.MaxHealth);
            if(isPaused)
                fontRenderer.DrawText(spriteBatch, ScreenSize.Width / 2 - 30, ScreenSize.Height / 2 - 10, "Pause");

            spriteBatch.End();
        }

        private void DrawMenu(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (this.MenuState == MenuStates.Main)
            {
                spriteBatch.Draw(titleTexture, new Vector2(100, 40), Color.White);

                fontRenderer.DrawText(spriteBatch, 140, 150, "Play Game");
                fontRenderer.DrawText(spriteBatch, 140, 200, "About");
                fontRenderer.DrawText(spriteBatch, 140, 250, "Quit");

                float rotationFloat = (float)(Math.PI / 2);
                spriteBatch.Draw(menuSelectionTexture, position: new Vector2(134, 158 + 50 * MenuIndex), rotation: rotationFloat, scale: new Vector2(0.25f, 0.25f));
            }
            else if (this.MenuState == MenuStates.About)
            {
                fontRenderer.DrawText(spriteBatch, 100, 100, "Manic Shooter");
                fontRenderer.DrawText(spriteBatch, 120, 200, "Made in the KSU Game Development Club");
                fontRenderer.DrawText(spriteBatch, 140, 300, "By Nick Boen and Matthew McHaney");
            }
            spriteBatch.End();
        }

        private void DrawGameOver(GameTime gameTime)
        {
            spriteBatch.Begin();
            fontRenderer.DrawText(spriteBatch, ScreenSize.Width / 2 - 50, ScreenSize.Height / 2 - 10, "Game Over");
            fontRenderer.DrawText(spriteBatch, ScreenSize.Width / 2 - 40, ScreenSize.Height / 2 + 40, string.Format("Score: {0}", player1.Score));
            spriteBatch.End();
        }

        public void gameKey_pausePressed(Keys key)
        {
            if (GameState == gameStates.Play)
            {
                if (isPaused)
                {
                    try
                    {
                        MediaPlayer.Resume();
                    }
                    catch
                    {

                        MediaPlayer.Stop();
                        try
                        {
                            MediaPlayer.Play(song);
                        }
                        catch
                        {
                            MediaPlayer.Play(song);
                        }
                    }
                }
                else
                {
                    MediaPlayer.Pause();
                }
                isPaused = !isPaused;
            }
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

        public void gameKey_spawnMob1(Keys Key)
        {
            MobSpawner.Instance.SpawnMobSet(0);
        }

        public void gameKey_spawnMob2(Keys Key)
        {
            MobSpawner.Instance.SpawnMobSet(1);
        }

        public void gameKey_spawnMob3(Keys key)
        {
            MobSpawner.Instance.SpawnMobSet(2);
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
            player1.ClearScore();
            if (player1.Lives == 0)
            {
                player1 = new DefaultPlayer(TextureManager.Instance.GetTexture("DefaultPlayer"), new Vector2(300, 300));
                ResourceManager.Instance.AddPlayer(player1);

                player1.ScaleSize((decimal)0.5);

                MobSpawner.Instance.Reset();
                MobSpawner.Instance.SetSpawnerMode(SpawnerMode.Constant);
            }
            try
            {
                MediaPlayer.Play(song);
            }
            catch
            {
                MediaPlayer.Play(song);
            }
            MediaPlayer.IsRepeating = true;
        }

        private void InitMenuState()
        {
            MenuState = MenuStates.Main;

            MenuIndex = 0;
        }

        private Timer gameOverTimer;

        private void InitGameOverState()
        {
            gameOverTimer = new Timer();
            gameOverTimer.Interval = 3000;
            gameOverTimer.Elapsed += new ElapsedEventHandler(gameOverTimer_Elapsed);
            gameOverTimer.Start();

            MediaPlayer.Stop();

            ResourceManager.Instance.ResetAll();
        }

        private void gameOverTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            gameOverTimer.Stop();
            GameState = gameStates.Menu;
        }
    }
}
