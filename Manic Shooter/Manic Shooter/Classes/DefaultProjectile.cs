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

        public DefaultProjectile(Texture2D texture, Vector2 position, int damage)
            : base(texture, position)
        {
            this.Damage = damage;
            IsActive = false;
        }

        public int GetDamage()
        {
            throw new NotImplementedException();
        }

        public bool IsPlayerProjectile()
        {
            return _isPlayerProjectile;
        }

        public virtual void Spawn(Vector2 position, Vector2 velocity, bool isPlayerProjectile)
        {
            this.Position = position;
            this.Velocity = velocity;

            _isPlayerProjectile = isPlayerProjectile;
        }

        public void Update(GameTime gameTime)
        {
            this.Position += this.Velocity;

            //Do hit detection here, or en masse?


            //Detect if it is off screen and de-activate it
            if (IsOffScreen())
            {
                IsActive = false;
            }
        }
    }
}
