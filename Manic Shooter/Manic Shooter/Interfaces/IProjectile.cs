using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Manic_Shooter.Interfaces
{
    interface IProjectile:ISprite
    {
        /// <summary>
        /// Get the damage that this projectile will do, will likely be 1 for enemy projectiles
        /// </summary>
        /// <returns>Returns an integer representing the amount of damage of this projectile</returns>
        int GetDamage();

        /// <summary>
        /// Determines whether the projectile was fired by a player
        /// </summary>
        /// <returns>Returns true if the projectile was fired by a player</returns>
        bool IsPlayerProjectile();
    }
}
