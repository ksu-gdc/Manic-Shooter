using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TextPackage;
using Manic_Shooter.Classes;

namespace Manic_Shooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        FontRenderer fontRenderer;
        ResourceManager resourceManager;

        DefaultPlayer player1;
        DefaultEnemy enemy1;
        DefaultProjectile projectile1;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;

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
            resourceManager = new ResourceManager();
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
            player1 = new DefaultPlayer(Content.Load<Texture2D>("Player_placeholder.png"), new Vector2(30, 30));
            enemy1 = new DefaultEnemy(Content.Load<Texture2D>("Enemy_placeholder.png"), new Vector2(100, 100), 10);
            projectile1 = new DefaultProjectile(Content.Load<Texture2D>("Projectile_placeholder.png"), new Vector2(200, 200), 10);

            resourceManager.AddPlayer(player1);
            resourceManager.AddEnemy(enemy1);
            resourceManager.AddProjectile(projectile1);

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

            // TODO: Add your update logic here
            resourceManager.Update(gameTime);

            base.Update(gameTime);
        }
        bool temp = false;
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
            resourceManager.RenderSprites(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
