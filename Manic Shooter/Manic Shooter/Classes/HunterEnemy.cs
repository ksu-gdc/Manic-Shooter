using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Manic_Shooter.Interfaces;

namespace Manic_Shooter.Classes
{
    class HunterEnemy:Sprite, IEnemy
    {
        private Vector2 targetEntryPosition, exitPosition;
        private int _maxSpeed;
        private EnemyState _state;
        private Queue<Vector2> _lastPlayerPositions;

        private List<IWeapon> _weapons;
        private int _lifeTimer;

        private int _maxShotTime;
        private int _shotTimer;
        private int _shotCount;

        public HunterEnemy(Texture2D texture, Vector2 position, int health)
            :base(texture, position, health)
        {
            Vector2 randomPosition = new Vector2(ManicShooter.RNG.Next(0, ManicShooter.ScreenSize.Width), ManicShooter.RNG.Next(0,ManicShooter.ScreenSize.Height/3));
            Initialize(randomPosition);
        }

        public HunterEnemy(Texture2D texture, Vector2 position, int health, Vector2 targetEntryPosition)
            :base(texture, position, health)
        {
            Initialize(targetEntryPosition);
        }

        private void Initialize(Vector2 entryPosition)
        {
            this.ScaleSize(0.12M);
            this.Visible = true;

            _maxSpeed = 150;

            _state = EnemyState.Entering;

            this.targetEntryPosition = entryPosition;
            this.exitPosition = new Vector2(-50, -50);

            this._maxShotTime = 1000;
            this._shotTimer = this._maxShotTime;
            this._shotCount = 0;

            this._lastPlayerPositions = new Queue<Vector2>();

            this._lifeTimer = 10000;

            this.Velocity = Vector2.Zero;

            _weapons = new List<IWeapon>();

            //Point pellet gun
            _weapons.Add(new PelletGun(this.centerPosition, new Vector2(0, this.Height / 2), new Vector2(0, 250), 0.3d));
        }

        public void Fire(TimeSpan elapsedTime)
        {
            //Should be a 3 round burst

            bool coolingDown = false;

            _shotTimer -= (int)elapsedTime.TotalMilliseconds;
            if(_shotTimer <= 0)
            {
                foreach(IWeapon w in _weapons)
                {
                    if (!coolingDown) coolingDown = w.IsCoolingDown();
                    w.Fire(elapsedTime);
                }
                if(!coolingDown)
                    _shotCount++;

                if(_shotCount >= 3)
                {
                    _shotTimer = _maxShotTime;
                    _shotCount = 0;
                }
            }
        }

        public EnemyState State
        {
            get { return _state; }
        }

        /// <summary>
        /// Updates this enemy instance
        /// </summary>
        /// <param name="gameTime">The time elapsed since the last update</param>
        public override void Update(GameTime gameTime)
        {
            AddPlayerPosition(ManicShooter.playerPosition);

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
                    _lifeTimer -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (_lifeTimer > 0)
                        Attacking(gameTime);
                    else
                        _state = EnemyState.Leaving;
                    break;
                case EnemyState.Leaving:
                    Leaving(gameTime);

                    if (this.IsOffScreen()) this.Destroy();
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
            Vector2 playerPosition = GetPlayerPosition();
            playerPosition.Y = this.Position.Y;
            Vector2 newVelocity = playerPosition - this.Position;

            Fire(gameTime.ElapsedGameTime);

            //If we're close enough to move to the spot in one tick then
            //we can just go there
            if (newVelocity.LengthSquared() < Math.Pow(_maxSpeed * gameTime.ElapsedGameTime.TotalSeconds, 2))
            {
                MoveTo(playerPosition);
                return true;
            }

            this.SetVelocity(newVelocity, _maxSpeed);

            return false;
        }

        public bool Leaving(GameTime gameTime)
        {
            Vector2 newVelocity = exitPosition - this.Position;

            //If we're close enough to move to the spot in one tick then
            //we can just go there
            if (newVelocity.LengthSquared() < Math.Pow(_maxSpeed * gameTime.ElapsedGameTime.TotalSeconds, 2))
            {
                MoveTo(exitPosition);
                this.IsActive = false;
                return true;
            }

            this.SetVelocity(newVelocity, _maxSpeed);

            return false;
        }

        /// <summary>
        /// Updates the position of each of the weapons so they stay in one place relative
        /// to the movement of this enemy instance
        /// </summary>
        public void UpdateWeaponPositions()
        {
            foreach (IWeapon w in _weapons)
            {
                w.SetReferencePosition(Position);
            }
        }

        public Vector2 GetPlayerPosition()
        {
            Vector2 returnValue;
            if(_lastPlayerPositions.Count > 0)
            {
                returnValue = _lastPlayerPositions.Dequeue();
            }
            else
            {
                returnValue = this.Position;
            }

            return returnValue;
        }

        public void AddPlayerPosition(Vector2 position)
        {
            if(_lastPlayerPositions.Count > 15)
            {
                _lastPlayerPositions.Dequeue();
            }
            _lastPlayerPositions.Enqueue(position);
        }

        public override void Destroy()
        {
            base.Destroy();
            //Spawn a default droppable

            MissileUpgradeDroppable drop = new MissileUpgradeDroppable(
                TextureManager.Instance.GetTexture("MissileDroppable"),
                this.Position);
            ResourceManager.Instance.AddDroppable(drop);
        }
    }
}
