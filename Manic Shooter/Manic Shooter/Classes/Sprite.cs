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
        protected Rectangle hitbox;

        protected bool _hurtFlashing = false;

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

        /// <summary>
        /// The hitbox of the sprite
        /// </summary>
        public Rectangle HitBox { get { return hitbox; } set { hitbox = value; } }

        /// <summary>
        /// The current 2-component vector of the velocity
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// The current health of the Sprite. For the player this
        /// will likely be 1, so that any hit will kill them.
        /// </summary>
        public int Health { get; set; }

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
        public void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.TextureBox, SpriteTint);
            
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
            this.HitBox = this.TextureBox;
            this.SpriteTint = Color.White;
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

            hitbox.Width = (int)Math.Round(hitbox.Width / horizRatio);
            hitbox.Height = (int)Math.Round(hitbox.Height / vertRatio);
        }

        public void ScaleSize(decimal scale)
        {
            texturebox.Width =(int)(texturebox.Width * scale);
            texturebox.Height = (int)(texturebox.Height * scale);

            hitbox.Width = (int)(hitbox.Width * scale);
            hitbox.Height = (int)(hitbox.Height * scale);
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
            hitbox.X = (int)centerPosition.X;
            hitbox.Y = (int)centerPosition.Y;
            hitbox.Offset((int)hitbox.Width / -2, (int)hitbox.Height / -2);
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
            hitbox.X = (int)centerPosition.X;
            hitbox.Y = (int)centerPosition.Y;
            hitbox.Offset((int)hitbox.Width / -2, (int)hitbox.Height / -2);
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

        }
    }
}
