using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manic_Shooter.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manic_Shooter.Classes
{
    class Sprite:IRenderable,ISprite
    {
        private const float HURT_FLASH_SPEED = 10;

        protected Vector2 centerPosition;
        protected Rectangle texturebox;
        protected float hitboxRadius;
        protected float hitboxHorizRatio;
        protected float hitboxVertRatio;
        protected int randomDropChance;

        protected bool _hurtFlashing = false;

        protected bool _drawHitbox = false;

        protected float _rotation = 0.0f;

        /// <summary>
        /// The current texture to be rendered
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The position of the center of the sprite
        /// </summary>
        public Vector2 Position { get { return centerPosition; } }

        /// <summary>
        /// The texturebox of the sprite
        /// </summary>
        public Rectangle TextureBox { get{return texturebox;} set{texturebox = value;} }

        public float Width { get { return (float)this.TextureBox.Width; } }

        public float Height { get { return (float)this.TextureBox.Height; } }

        /// <summary>
        /// The center of the hitbox for the sprite
        /// </summary>
        public Vector2 HitBoxCenter { get { return new Vector2(
            (float)Math.Round(centerPosition.X + hitboxHorizRatio * (Width / 2)),
            (float)Math.Round(centerPosition.Y + hitboxVertRatio * (Height / 2))); } }

        /// <summary>
        /// The horizontal ratio of the hitbox center for the sprite - 0 is center, 1 is right side, -1 is left side
        /// </summary>
        public float HitBoxHorizRatio { get { return hitboxHorizRatio; } set { hitboxHorizRatio = value; } }

        /// <summary>
        /// The vertical ratio of the hitbox center for the sprite - 0 is center, 1 is bottom, -1 is top
        /// </summary>
        public float HitBoxVertRatio { get { return hitboxVertRatio; } set { hitboxVertRatio = value; } }

        /// <summary>
        /// The radius of the hitbox for the sprite
        /// </summary>
        public float HitBoxRadius { get { return hitboxRadius; } set { hitboxRadius = value; } }

        /// <summary>
        /// The current 2-component vector of the velocity
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// The current health of the Sprite. For the player this
        /// will likely be 1, so that any hit will kill them.
        /// </summary>
        public int Health { get; set; }

        public int MaxHealth { get; set; }

        /// <summary>
        /// Gets or Sets whether or not this sprite is active or not. If inactive it
        /// may be reused as another sprite or it may be removed from the lists.
        /// This is used for a form of Lazy Deletion.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Tint coloring of the specific sprite. Default is white
        /// </summary>
        protected Color SpriteTint { get; set; }

        /// <summary>
        /// Renders the Sprite with it's current texture
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use for rendering</param>
        public virtual void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if(_rotation != 0.0f)
            {
                Vector2 origin = new Vector2((float)texturebox.Width / 2, (float)texturebox.Height / 2);
                spriteBatch.Draw(this.Texture, null, this.TextureBox, null, origin, _rotation, null, SpriteTint);
                if (this._drawHitbox)
                {
                    Vector2 hitboxCenter = this.HitBoxCenter;
                    Texture2D texture = TextureManager.Instance.GetTexture("Hitbox");
                    spriteBatch.Draw(texture, hitboxCenter - new Vector2(hitboxRadius, hitboxRadius), null, new Color(255, 0, 0, 120), _rotation, origin, hitboxRadius * 2 / texture.Width, SpriteEffects.None, 1);
                }
            }
            else
            {
                spriteBatch.Draw(this.Texture, this.TextureBox, SpriteTint);
                if (this._drawHitbox)
                {
                    Vector2 hitboxCenter = this.HitBoxCenter;
                    Texture2D texture = TextureManager.Instance.GetTexture("Hitbox");
                    spriteBatch.Draw(texture, hitboxCenter - new Vector2(hitboxRadius, hitboxRadius), null, new Color(255, 0, 0, 120), 0, Vector2.Zero, hitboxRadius * 2 / texture.Width, SpriteEffects.None, 1);
                }
            }
            

            if (this._hurtFlashing)
            {
                this.SpriteTint = new Color((int)Math.Min(255, this.SpriteTint.R + HURT_FLASH_SPEED), (int)Math.Min(255, this.SpriteTint.G + HURT_FLASH_SPEED),
                    (int)Math.Min(255, this.SpriteTint.B + HURT_FLASH_SPEED), this.SpriteTint.A);

                if (this.SpriteTint.R == 255 && this.SpriteTint.G == 255 && this.SpriteTint.B == 255)
                    this._hurtFlashing = false;
            }
        }

        public Sprite(Texture2D texture, Vector2 position, int health = 1)
        {
            this.Texture = texture;
            centerPosition = new Vector2();
            this.MoveTo(position.X, position.Y);
            this.Health = health;
            this.IsActive = true;
            this.Visible = true;
            this.TextureBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            this.TextureBox.Offset((int)texture.Width / -2, (int)texture.Height / -2);
            this.HitBoxHorizRatio = 0;
            this.HitBoxVertRatio = 0;
            this.HitBoxRadius = texture.Width / 2;
            this.SpriteTint = Color.White;
            this.randomDropChance = 25;
        }

        /// <summary>
        /// Makes the sprite visible
        /// </summary>
        public void Show()
        {
            Visible = true;
        }

        /// <summary>
        /// Makes the sprite invisible
        /// </summary>
        public void Hide()
        {
            Visible = false;
        }

        /// <summary>
        /// The current visibility of the Sprite, if true then the sprite should be
        /// rendered to the screen at it's current location.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Used to update the sprite for changing position or taking actions. 
        /// This is best called in the Update part of the game loop
        /// </summary>
        /// <param name="gameTime">The amount of time elapsed since the last call</param>
        public virtual void Update(GameTime gameTime) { }


        public void SetSize(Vector2 newSize)
        {
            float horizRatio = newSize.X / texturebox.Width;
            float vertRatio = newSize.Y / texturebox.Height;

            texturebox.Width = (int)newSize.X;
            texturebox.Height = (int)newSize.Y;

            hitboxRadius *= horizRatio;
        }

        public void ScaleSize(decimal scale)
        {
            texturebox.Width =(int)(texturebox.Width * scale);
            texturebox.Height = (int)(texturebox.Height * scale);

            hitboxRadius *= (float)scale;
        }

        /// <summary>
        /// Applies the current velocity to the sprite given the time elapsed since the last
        /// update call. 
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update</param>
        public void ApplyVelocity(TimeSpan elapsedTime)
        {
            float deltaX = this.Velocity.X * (float)elapsedTime.TotalSeconds;
            float deltaY = this.Velocity.Y * (float)elapsedTime.TotalSeconds;

            this.MoveBy(deltaX, deltaY);
        }

        /// <summary>
        /// Used to determine if the sprite is offscreen (used for de-activation)
        /// </summary>
        public bool IsOffScreen()
        {
            return 
            (
                texturebox.Left > ManicShooter.ScreenSize.Width ||
                texturebox.Right < ManicShooter.ScreenSize.Top ||
                texturebox.Top > ManicShooter.ScreenSize.Bottom ||
                texturebox.Bottom < ManicShooter.ScreenSize.Top
            );
        }

        protected void MoveTo(float x, float y)
        {
            centerPosition.X = x;
            centerPosition.Y = y;
            texturebox.X = (int)centerPosition.X;
            texturebox.Y = (int)centerPosition.Y;
            texturebox.Offset((int)texturebox.Width / -2, (int)texturebox.Height / -2);
        }

        protected void MoveTo(Vector2 position)
        {
            this.MoveTo(position.X, position.Y);
        }

        protected void MoveBy(float deltaX, float deltaY)
        {
            centerPosition.X += deltaX;
            centerPosition.Y += deltaY;
            texturebox.X = (int)centerPosition.X;
            texturebox.Y = (int)centerPosition.Y;
            texturebox.Offset((int)texturebox.Width / -2, (int)texturebox.Height / -2);
        }

        protected void MoveBy(Vector2 deltaVector)
        {
            this.MoveBy(deltaVector.X, deltaVector.Y);
        }

        protected void SetVelocity(Vector2 newVelocity)
        {
            this.Velocity = newVelocity;
        }

        protected void SetVelocity(Vector2 destination, int speed)
        {
            destination.Normalize();
            this.SetVelocity(speed * destination);
        }

        public virtual void Destroy()
        {
            //Here you'd put an animation for blowing up the sprite, but for now
            // we're using no animation
            this.Visible = false;
            this.IsActive = false;
            this.Health = 0;

        }
    }
}
