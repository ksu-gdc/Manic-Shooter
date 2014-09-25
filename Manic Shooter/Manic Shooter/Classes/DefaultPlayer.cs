using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manic_Shooter.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manic_Shooter.Classes
{
    class DefaultPlayer: Sprite, IPlayer, ISprite
    {
        public DefaultPlayer(Texture2D texture, Vector2 position)
            : base(texture, position)
        {

        }

        public void SetVelocity(Vector2 newVelocity)
        {
            this.Velocity = newVelocity;
        }

        public void AddVelocity(Vector2 appliedVelocity, uint maxSpeed = uint.MaxValue)
        {
            Vector2 currentVelocity = this.Velocity;
            Vector2.Add(ref currentVelocity, ref appliedVelocity, out currentVelocity);
            if (currentVelocity.LengthSquared() < (maxSpeed * maxSpeed))
            {
                currentVelocity.Normalize();
                currentVelocity = Vector2.Multiply(currentVelocity, maxSpeed);
            }

            this.Velocity = currentVelocity;
        }

        public void Fire()
        {
            throw new NotImplementedException();
        }
    }
}
