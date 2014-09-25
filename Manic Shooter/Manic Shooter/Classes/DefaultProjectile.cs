using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manic_Shooter.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manic_Shooter.Classes
{
    class DefaultProjectile:Sprite, ISprite, IProjectile
    {
        public int Damage { get; private set; }

        public DefaultProjectile(Texture2D texture, Vector2 position, int damage)
            : base(texture, position)
        {
            this.Damage = damage;
        }

        public int GetDamage()
        {
            throw new NotImplementedException();
        }

        public bool IsPlayerProjectile()
        {
            throw new NotImplementedException();
        }
    }
}
