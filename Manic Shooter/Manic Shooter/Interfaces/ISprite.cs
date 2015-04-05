using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manic_Shooter.Interfaces
{
    public interface ISprite
    {
        /// <summary>
        /// The current texture to be rendered
        /// </summary>
        Texture2D Texture { get; set; }

        /// <summary>
        /// The position of the center of the sprite
        /// </summary>
        Vector2 Position { get; }

        Vector2 HitBoxCenter { get; }

        float HitBoxRadius { get; set; }

        /// <summary>
        /// The hitbox of the sprite
        /// </summary>
        Rectangle TextureBox { get; set; }

        /// <summary>
        /// The current 2-component vector of the velocity
        /// </summary>
        Vector2 Velocity { get; set; }

        /// <summary>
        /// The current health of the Sprite. For the player this
        /// will likely be 1, so that any hit will kill them.
        /// </summary>
        int Health { get; set; }

        int MaxHealth { get; set; }

        /// <summary>
        /// Gets or Sets whether or not this sprite is active or not. If inactive it
        /// may be reused as another sprite or it may be removed from the lists.
        /// This is used for a form of Lazy Deletion.
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Used to update the sprite for changing position or taking actions. 
        /// This is best called in the Update part of the game loop
        /// </summary>
        /// <param name="gameTime">The amount of time elapsed since the last call</param>
        void Update(GameTime gameTime);

        void SetSize(Vector2 newSize);
        void ScaleSize(decimal scale);
        void ApplyVelocity(TimeSpan elapsedTime);

        void Destroy();
    }
}
