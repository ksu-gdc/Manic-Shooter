using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manic_Shooter.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manic_Shooter.Classes
{
    class DefaultMissile:Sprite, ISprite, IProjectile
    {
        public int Damage { get; private set; }

        private bool isPlayerProjectile;

        public DefaultMissile(Texture2D texture, Vector2 position, Vector2 velocity, int damage, bool isPlayerProjectile = true)
            : base(texture, position)
        {
            this.Damage = damage;
            this.Velocity = velocity;
            this.ScaleSize(2M);
            this.isPlayerProjectile = isPlayerProjectile;
        }

        public int GetDamage()
        {
            return this.Damage;
        }

        public bool IsPlayerProjectile()
        {
            return isPlayerProjectile;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 deltaV = this.Velocity * ((float)gameTime.ElapsedGameTime.TotalSeconds);
            this.MoveBy(deltaV.X, deltaV.Y);

            //We can also use gameTime.ElapsedGameTime.TotalSeconds to achieve the same value without the division
            //this.Position += this.Velocity * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000);

            //Detect if it is off screen and de-activate it
            if (IsOffScreen())
            {
                IsActive = false;
            }
        }
    }
}
