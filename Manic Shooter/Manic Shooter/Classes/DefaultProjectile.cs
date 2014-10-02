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

        private bool _isPlayerProjectile;

        public DefaultProjectile(Texture2D texture, Vector2 position, Vector2 velocity, int damage)
            : base(texture, position)
        {
            this.Damage = damage;
            this.Velocity = velocity;
        }

        public int GetDamage()
        {
            throw new NotImplementedException();
        }

        public bool IsPlayerProjectile()
        {
            return _isPlayerProjectile;
        }

        public override void Update(GameTime gameTime)
        {
            this.Position += this.Velocity * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000);

            //Do hit detection here, or en masse?


            //Detect if it is off screen and de-activate it
            if (IsOffScreen())
            {
                IsActive = false;
            }
        }
    }
}
