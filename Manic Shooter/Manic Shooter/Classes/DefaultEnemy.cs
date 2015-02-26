using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manic_Shooter.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manic_Shooter.Classes
{
    class DefaultEnemy:Sprite, IEnemy
    {
        private int _screenPasses;
        private int _maxPasses;
        private int _maxSpeed;
        
        /// <summary>
        /// The target entry point is used while the sprite is entering the screen. 
        /// Once the sprite reaches this point, it will progress to the next state
        /// </summary>
        protected Vector2 targetEntryPoint;

        protected List<IWeapon> _weapons;

        protected EnemyState _state;
        public EnemyState State { get { return _state; } private set { _state = value; } }

        public DefaultEnemy(Texture2D texture, Vector2 position, int health)
            : base(texture, position, health)
        {
            Random rng = new Random();

            //Finds a random point on the x axis between the left and right side of the screen
            //and a random point on the y axis between the top and the midpoint of the screen
            //with some padding for both
            Vector2 randomTargetEntryPoint = new Vector2(rng.Next(50,ManicShooter.ScreenSize.Width-50), rng.Next(50,ManicShooter.ScreenSize.Height/2-50));

            InitializeDefaultEnemy(randomTargetEntryPoint);
        }

        public DefaultEnemy(Texture2D texture, Vector2 position, int health, Vector2 targetEntryPoint)
            : base(texture, position, health)
        {
            InitializeDefaultEnemy(targetEntryPoint);
        }

        private void InitializeDefaultEnemy(Vector2 TargetEntryPoint)
        {
            this.ScaleSize(0.5M);

            _screenPasses = 0;
            _maxPasses = 3;
            this.Visible = true;

            _maxSpeed = 150;

            _state = EnemyState.Entering;

            this.targetEntryPoint = TargetEntryPoint;

            this.Velocity = Vector2.Zero;

            _weapons = new List<IWeapon>();

            //Top Left pellet gun
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(-this.Width / 2, -this.Height / 2), new Vector2(-250, -250), 0.3d));
            //Top Right pellet gun
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(this.Width / 2, -this.Height / 2), new Vector2(250, -250), 0.3d));
            //Bottom Left pellet gun
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(-this.Width / 2, this.Height / 2), new Vector2(-250, 250), 0.3d));
            //Bottom Right pellet gun
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(this.Width / 2, this.Height / 2), new Vector2(250, 250), 0.3d));
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
                    if (Entering(gameTime.ElapsedGameTime))
                    {
                        _state = EnemyState.Attacking;
                        this.Velocity = (this.Position.X < ManicShooter.ScreenSize.Width / 2) ? new Vector2(_maxSpeed, 0) : new Vector2(-_maxSpeed, 0);
                    }
                    break;
                case EnemyState.Attacking:
                    if (HorizontalPasses(gameTime.ElapsedGameTime)) _state = EnemyState.Leaving;
                    break;
                case EnemyState.Leaving:
                    LeaveScreen(gameTime.ElapsedGameTime);
                    break;
            }

            ApplyVelocity(gameTime.ElapsedGameTime);
            UpdateWeaponPositions();

            base.Update(gameTime);
        }

        public bool Entering(TimeSpan elapsedTime)
        {
            Vector2 newVelocity = targetEntryPoint - this.Position;

            //If we're close enough to move to the spot in one tick then
            //we can just go there
            if (newVelocity.LengthSquared() < Math.Pow(_maxSpeed * elapsedTime.TotalSeconds, 2))
            {
                MoveTo(targetEntryPoint);
                return true;
            }

            this.SetVelocity(newVelocity, _maxSpeed);

            return false;
        }

        /// <summary>
        /// An enemy movement where the enemy passes horizontally across
        /// the screen while firing shots.
        /// </summary>
        /// <param name="elapsedTime">The time elapsed since this sprite was last updated</param>
        /// <returns>Returns true if the move is completed, false otherwise. This is needed
        /// to because this method needs to be called multiple times as the enemy is updated</returns>
        public bool HorizontalPasses(TimeSpan elapsedTime)
        {
            bool isOnEdge = TextureBox.Right > ManicShooter.ScreenSize.Width || TextureBox.Left < 0;
            if (_screenPasses < _maxPasses)
            {
                //Enemy is still making passes

                //Check if enemy still in bounds
                if (isOnEdge)
                {
                    //If this sprite is off screen then it should go the other way
                    this.Velocity = new Vector2(-this.Velocity.X, this.Velocity.Y);
                    _screenPasses++;
                }
            }
            else
                return true; //Move has been completed

            Fire(elapsedTime);

            return false;
        }

        public bool LeaveScreen(TimeSpan elapsedTime)
        {
            Vector2 topLeftSafeSpot = new Vector2(-this.Width, -this.Height);
            Vector2 topRightSafeSpot = new Vector2(ManicShooter.ScreenSize.Right + this.Width, -this.Height);

            if (this.Position == topLeftSafeSpot || this.Position == topRightSafeSpot) return true;

            float topLeftDistanceSquared = Vector2.DistanceSquared(this.Position, topLeftSafeSpot);
            float topRightDistanceSquared = Vector2.DistanceSquared(this.Position, topRightSafeSpot);

            float snapDistance = 10;

            //Determining which side of the screen to leave
            if (topLeftDistanceSquared < topRightDistanceSquared)
            {
                if (topLeftDistanceSquared < (snapDistance * snapDistance))
                {
                    this.MoveTo(topLeftSafeSpot);
                    this.IsActive = false;
                    return true;
                }
                this.SetVelocity(topLeftSafeSpot, this._maxSpeed);
            }
            else
            {
                if (topRightDistanceSquared < (snapDistance * snapDistance))
                {
                    this.MoveTo(topRightSafeSpot);
                    this.IsActive = false;
                    return true;
                }
                this.SetVelocity(topRightSafeSpot, this._maxSpeed);
            }
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

        public override void Destroy()
        {
            base.Destroy();
            //Spawn a default droppable

            DefaultDroppable drop = new DefaultDroppable(
                TextureManager.Instance.GetTexture("DefaultProjectile"),
                this.Position);
            ResourceManager.Instance.AddDroppable(drop);
        }
    }
}
