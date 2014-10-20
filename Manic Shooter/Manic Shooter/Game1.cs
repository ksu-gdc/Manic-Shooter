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
#endregion

namespace Manic_Shooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ManicShooter : Game
    {
        public static Rectangle ScreenSize;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        FontRenderer fontRenderer;

        DefaultPlayer player1;
        DefaultEnemy enemy1;
        DefaultProjectile projectile1;

        public ManicShooter()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;

            ScreenSize = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            
            Content.RootDirectory = "Content";
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

            TextureManager.Instance.AddTexture("DefaultPlayer", Content.Load<Texture2D>("Player_placeholder.png"));
            TextureManager.Instance.AddTexture("DefaultEnemy", Content.Load<Texture2D>("Enemy_placeholder.png"));
            TextureManager.Instance.AddTexture("DefaultProjectile", Content.Load<Texture2D>("Projectile_placeholder.png"));

            player1 = new DefaultPlayer(TextureManager.Instance.GetTexture("DefaultPlayer"), new Vector2(30, 30));
            enemy1 = new DefaultEnemy(TextureManager.Instance.GetTexture("DefaultEnemy"), new Vector2(-50, -50), 10);
            projectile1 = new DefaultProjectile(TextureManager.Instance.GetTexture("DefaultProjectile"), new Vector2(200, 200), new Vector2(0, 120), 10);

            ResourceManager.Instance.AddPlayer(player1);
            ResourceManager.Instance.AddEnemy(enemy1);
            ResourceManager.Instance.AddProjectile(projectile1);

            player1.ScaleSize((decimal)0.5);
            enemy1.ScaleSize((decimal)0.5);
            projectile1.ScaleSize((decimal)0.5);

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

            // TODO: Add your update logic here
            ResourceManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            spriteBatch.Begin();

            fontRenderer.DrawText(spriteBatch, 50, 50, "Hello World!\nGame Time\t=\t" + gameTime.TotalGameTime.ToString());
            ResourceManager.Instance.RenderSprites(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
