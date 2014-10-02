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
        private Vector2 centerPosition;
        private Rectangle hitbox;

        /// <summary>
        /// The current texture to be rendered
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The position of the center of the sprite
        /// </summary>
        public Vector2 Position
        {
            get { return centerPosition; }
            set
            {
                centerPosition = value;
                hitbox.X = (int)value.X;
                hitbox.Y = (int)value.Y;
                hitbox.Offset((int)hitbox.Width / -2, (int)hitbox.Height / -2);
            }
        }

        /// <summary>
        /// The hitbox of the sprite
        /// </summary>
        public Rectangle HitBox { get{return hitbox;} set{hitbox = value;} }

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
        /// Renders the Sprite with it's current texture
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use for rendering</param>
        public void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.HitBox, Color.White);
        }

        public Sprite(Texture2D texture, Vector2 position, int health = 1)
        {
            this.Texture = texture;
            this.Position = position;
            this.Health = health;
            this.IsActive = true;
            this.Visible = true;
            this.HitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            this.HitBox.Offset((int)texture.Width / -2, (int)texture.Height / -2);
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
            hitbox.Width = (int)newSize.X;
            hitbox.Height = (int)newSize.Y;
        }

        public void ScaleSize(decimal scale)
        {
            hitbox.Width =(int)(hitbox.Width * scale);
            hitbox.Height = (int)(hitbox.Height * scale);
        }

        /// <summary>
        /// Used to determine if the sprite is offscreen (used for de-activation)
        /// </summary>
        public bool IsOffScreen()
        {
            if (hitbox.X > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width ||
                hitbox.X + hitbox.Width < 0 ||
                 hitbox.Y > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height ||
                  hitbox.Y + hitbox.Height < 0)
                return true;
            
            return false;
        }
    }
}
