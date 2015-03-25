using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Manic_Shooter.Interfaces
{
    interface IPlayer:ISprite
    {
        /// <summary>
        /// Sets the players velocity. Use this if the players velocity should
        /// be overwritten immediately. Useful for tight and precise controls.
        /// </summary>
        /// <param name="newVelocity">The new velocity of the player</param>
        void SetVelocity(Vector2 newVelocity);

        /// <summary>
        /// Adds the given velocity to the players current velocity. Use this if 
        /// the movement of the player should be smoother. This represents momentum
        /// based controls and will be less controlable as the player will "slide" around
        /// </summary>
        /// <param name="appliedVelocity">The velocity to add to the players velocity</param>
        /// <param name="maxSpeed">Will cap the speed in any direction to the maxSpeed value
        /// (i.e. the magnitude of the velocity vector will be clamped by maxSpeed)</param>
        void AddVelocity(Vector2 appliedVelocity, uint maxSpeed = uint.MaxValue);

        /// <summary>
        /// Fire the players current weapon
        /// </summary>
        void Fire(TimeSpan elapsedTime);

        /// <summary>
        /// Called when collides with projectile
        /// </summary>
        void HitBy(IProjectile projectile);
    }
}
