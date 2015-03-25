using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manic_Shooter.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manic_Shooter.Classes
{
    class PelletUpgradeDroppable:DefaultDroppable
    {
        public float downwardAccel = 100;
        public float maxSpeed = 250;

        public PelletUpgradeDroppable(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            this.Velocity = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaA = downwardAccel * ((float)gameTime.ElapsedGameTime.TotalSeconds);
            float newV = this.Velocity.Y + deltaA;
            if(newV > maxSpeed)
                newV = maxSpeed;
            this.Velocity = new Vector2(this.Velocity.X, newV);
            
            Vector2 deltaV = this.Velocity * ((float)gameTime.ElapsedGameTime.TotalSeconds);
            this.MoveBy(deltaV.X, deltaV.Y);

            //We can also use gameTime.ElapsedGameTime.TotalSeconds to achieve the same value without the division
            //this.Position += this.Velocity * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000);

            //Do hit detection here, or en masse?

            //RESPONSE: It would probably be best to do it en masse since we could do some filtering for
            //efficiency ~Nick Boen

            //Detect if it is off screen and de-activate it
            if (IsOffScreen())
            {
                IsActive = false;
            }
        }

        public override void ApplyEffect(IPlayer player)
        {
            //For now default to upgrading pellet gun
            //We can fix the code later
            DefaultPlayer dPlayer = (DefaultPlayer)player;
            dPlayer.UpgradeGun(typeof(PelletGun));
        }
    }
}
