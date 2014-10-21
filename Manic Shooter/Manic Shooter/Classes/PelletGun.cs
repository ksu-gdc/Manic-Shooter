using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manic_Shooter.Interfaces;
using Microsoft.Xna.Framework;

//TODO: Use perprocessor commands or 
//remove later since this is for debugging
//purposes only. No need to bloat the game.
using System.Diagnostics;

namespace Manic_Shooter.Classes
{
    class PelletGun:IWeapon
    {
        private Vector2 _referencePosition;

        private Vector2 _muzzleOffset;
        /// <summary>
        /// The position of the muzzle used to spawn projectiles
        /// </summary>
        public Vector2 MuzzleOffset { get{return _muzzleOffset;} }

        /// <summary>
        /// The 2-component Vector for the speed and direction of the bullet
        /// </summary>
        private Vector2 _firingVelocity;

        /// <summary>
        /// The max time to cool down before firing another shot
        /// </summary>
        private double _maxCoolDown;

        /// <summary>
        /// The cool down time left (in seconds) of the weapon
        /// </summary>
        private double _coolDown;

        /// <summary>
        /// Initialize the pellet gun weapon
        /// </summary>
        /// <param name="muzzleOffset">The 2-component Vector position of the muzzle or firing location</param>
        /// <param name="firingAngle">The angle (in radians) to fire the pellet at</param>
        /// <param name="firingSpeed">The speed (in pixels per second) to fire the pellet</param>
        /// <param name="maxCoolDownTime">The amount of time (in seconds) to cool down before allowing another shot to fire</param>
        public PelletGun(ref Vector2 referencePosition, Vector2 muzzleOffset, double firingAngle, float firingSpeed, double maxCoolDownTime)
        {
            _referencePosition = referencePosition;
            _muzzleOffset = muzzleOffset;
            
            _coolDown = 0.0d;
            _maxCoolDown = maxCoolDownTime;

            //First we get the unit vector that corresponds to the firing angle
            //While these are expensive calculations, they shouldn't be cumbersome since we 
            //probably won't be initializing new weapons on the fly. If that were the case
            //then we could use the alternative constructor to pass in the velocity directly
            float unitX = (float)Math.Cos(firingAngle);
            float unitY = (float)Math.Sin(firingAngle);

            //Then we can just scale the velocity by the speed
            _firingVelocity = new Vector2(firingSpeed * unitX, firingSpeed * unitY);
        }

        public PelletGun(Vector2 referencePosition, Vector2 muzzlePosition, Vector2 firingVelocity, double maxCoolDown)
        {
            _referencePosition = referencePosition;
            _muzzleOffset = muzzlePosition;
            _firingVelocity = firingVelocity;
            _maxCoolDown = maxCoolDown;
        }

        public void Fire(TimeSpan elapsedTime)
        {
            //First we check the cooldown, if the timer is greater than
            //zero then we shouldn't be firing
            if (_coolDown > 0)
            {
                //But we still need to decrement it
                _coolDown -= elapsedTime.TotalSeconds;
                return;
            }

            Debug.WriteLine("Reference Position = " + _referencePosition.ToString());
            //Otherwise we can go ahead and Fire by creating a new projectile
            ResourceManager.Instance.AddProjectile(
                new DefaultProjectile(TextureManager.Instance.GetTexture("DefaultProjectile"), Vector2.Add(_referencePosition, _muzzleOffset), _firingVelocity, 1, false)
                );

            //And setting the cool down timer
            _coolDown = _maxCoolDown;
        }


        public void SetReferencePosition(int x, int y)
        {
            _referencePosition.X = x;
            _referencePosition.Y = y;
        }

        public void SetReferencePosition(Vector2 newPosition)
        {
            _referencePosition = newPosition;
        }
    }
}
