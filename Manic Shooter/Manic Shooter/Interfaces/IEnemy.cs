using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Manic_Shooter.Interfaces
{
    public enum EnemyState
    {
        Entering,
        Attacking,
        Leaving
    }

    public interface IEnemy:ISprite
    {
        EnemyState State { get; }

        /// <summary>
        /// Method for firing a shot from an IWeapon
        /// </summary>
        void Fire(TimeSpan elapsedTime);
    }
}
