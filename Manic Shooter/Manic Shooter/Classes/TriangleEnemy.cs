using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manic_Shooter.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manic_Shooter.Classes
{
    class TriangleEnemy: Sprite, IEnemy
    {
        private Vector2 targetEntryPosition, pointPosition, endingPosition;
        private int _maxSpeed;
        private EnemyState _state;
        private const int pathBuffer = 50;

        private List<IWeapon> _weapons;
        public TriangleEnemy(Texture2D texture, Vector2 position, int health)
            :base(texture, position, health)
        {
            Vector2 randomPosition = new Vector2(ManicShooter.RNG.Next(pathBuffer, ManicShooter.ScreenSize.Width - pathBuffer), -20);
            Initialize(position, randomPosition);
        }

        public TriangleEnemy(Texture2D texture, Vector2 position, int health, Vector2 targetEntryPosition)
            :base(texture, position, health)
        {
            Initialize(position,targetEntryPosition);
        }

        public void Initialize(Vector2 position, Vector2 targetEntryPosition)
        {
            this.ScaleSize(0.12M);
            this.Visible = true;

            _maxSpeed = 150;

            _state = EnemyState.Entering;

            this.targetEntryPosition = targetEntryPosition;
            
            Random rng = new Random();
            int screenQuarterWidth = (ManicShooter.ScreenSize.Width - 100) / 4;
            int verticalTravel = rng.Next((2 * ManicShooter.ScreenSize.Height) / 3) + ManicShooter.ScreenSize.Height/3;
            if (this.targetEntryPosition.X > (2 * screenQuarterWidth) + 50)//Check if ship is starting on the right side of the screen
                this.pointPosition = new Vector2(this.targetEntryPosition.X - screenQuarterWidth, verticalTravel);
            else
                this.pointPosition = new Vector2(this.targetEntryPosition.X + screenQuarterWidth, verticalTravel);

            endingPosition = new Vector2(this.pointPosition.X + screenQuarterWidth, this.targetEntryPosition.Y);

            this.Velocity = Vector2.Zero;

            _weapons = new List<IWeapon>();

            Vector2 normalVector = new Vector2(250, 250);
            normalVector.Normalize();
            float xNormal = normalVector.X;
            float yNormal = normalVector.Y;

            //Point pellet gun
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(0, this.Height / 2), new Vector2(0, 250), 0.3d));
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(0, this.Height / 2), new Vector2(0, -250), 0.3d));
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(0, this.Height / 2), new Vector2(250, 0), 0.3d));
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(0, this.Height / 2), new Vector2(-250,0), 0.3d));
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(0, this.Height / 2), new Vector2(xNormal, yNormal)*250, 0.3d));
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(0, this.Height / 2), new Vector2(-xNormal, yNormal)*250, 0.3d));
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(0, this.Height / 2), new Vector2(xNormal, -yNormal)*250, 0.3d));
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(0, this.Height / 2), new Vector2(-xNormal, -yNormal)*250, 0.3d));
            
        }

        EnemyState IEnemy.State
        {
            get { return _state; }
        }

        void IEnemy.Fire(TimeSpan elapsedTime)
        {
            foreach (IWeapon weapon in _weapons)
            {
                weapon.Fire(elapsedTime);
            }
        }

        /// <summary>
        /// Updates this enemy instance
        /// </summary>
        /// <param name="gameTime">The time elapsed since the last update</param>
        public override void Update(GameTime gameTime)
        {
            switch (_state)
            {
                case EnemyState.Entering:
                    if (Entering(gameTime))
                    {
                        _state = EnemyState.Attacking;
                        if (this.Position.X < ManicShooter.ScreenSize.Width / 2)
                            this.Velocity = new Vector2(_maxSpeed, 0);
                        else
                            this.Velocity = new Vector2(-_maxSpeed, 0);
                    }
                    break;
                case EnemyState.Attacking:
                    if (Attacking(gameTime)) _state = EnemyState.Leaving;
                    break;
                case EnemyState.Leaving:
                    Leaving(gameTime);
                    break;
            }

            ApplyVelocity(gameTime.ElapsedGameTime);
            UpdateWeaponPositions();

            base.Update(gameTime);
        }

        public bool Entering(GameTime gameTime)
        {
            Vector2 newVelocity = targetEntryPosition - this.Position;

            //If we're close enough to move to the spot in one tick then
            //we can just go there
            if (newVelocity.LengthSquared() < Math.Pow(_maxSpeed * gameTime.ElapsedGameTime.TotalSeconds, 2))
            {
                MoveTo(targetEntryPosition);
                return true;
            }

            this.SetVelocity(newVelocity, _maxSpeed);

            return false;
        }

        public bool Attacking(GameTime gameTime)
        {
            Vector2 newVelocity = pointPosition - this.Position;

            //If we're close enough to move to the spot in one tick then
            //we can just go there
            if (newVelocity.LengthSquared() < Math.Pow(_maxSpeed * gameTime.ElapsedGameTime.TotalSeconds, 2))
            {
                MoveTo(pointPosition);
                Fire(gameTime.ElapsedGameTime);
                return true;
            }

            this.SetVelocity(newVelocity, _maxSpeed);

            return false;
        }

        public bool Leaving(GameTime gameTime)
        {
            Vector2 newVelocity = endingPosition - this.Position;

            //If we're close enough to move to the spot in one tick then
            //we can just go there
            if (newVelocity.LengthSquared() < Math.Pow(_maxSpeed * gameTime.ElapsedGameTime.TotalSeconds, 2))
            {
                MoveTo(endingPosition);
                this.IsActive = false;
                return true;
            }

            this.SetVelocity(newVelocity, _maxSpeed);

            return false;
        }

        /// <summary>
        /// Fires a shot from all available weapons
        /// </summary>
        /// <param name="elapsedTime">The time elapsed since the last update</param>
        public void Fire(TimeSpan elapsedTime)
        {
            foreach (IWeapon weapon in _weapons)
            {
                weapon.Fire(elapsedTime);
            }
        }

        /// <summary>
        /// Updates the position of each of the weapons so they stay in one place relative
        /// to the movement of this enemy instance
        /// </summary>
        public void UpdateWeaponPositions()
        {
            foreach (IWeapon w in _weapons)
            {
                w.SetReferencePosition(this.centerPosition);
            }
        }
    }
}
