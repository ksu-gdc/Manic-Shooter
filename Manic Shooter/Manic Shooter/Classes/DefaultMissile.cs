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

        private const float MAX_TURN_VEL = 1f;

        private Sprite _target;

        public DefaultMissile(Texture2D texture, Vector2 position, Vector2 velocity, int damage, bool isPlayerProjectile = true)
            : base(texture, position)
        {
            this.Damage = damage;
            this.Velocity = velocity;
            this.ScaleSize(2M);
            this.isPlayerProjectile = isPlayerProjectile;

            //Lock on to target
            LockOnToEnemy();
        }

        public void LockOnToEnemy()
        {
            Vector2 thisPos = this.Position;
            float distance = float.MaxValue;

            if (this.isPlayerProjectile)
            {
                IEnemy targetedEnemy = null;

                foreach (IEnemy enemy in ResourceManager.Instance.ActiveEnemyList)
                {
                    if (!enemy.IsActive)
                        continue;

                    //Get the distance
                    float newDist;
                    Vector2 enemyPos = enemy.Position;
                    Vector2.Distance(ref enemyPos, ref thisPos, out newDist);
                    if (newDist < distance)
                    {
                        distance = newDist;
                        targetedEnemy = enemy;
                    }

                }

                if (targetedEnemy != null)
                    this._target = (Sprite)targetedEnemy;
            }
            else
            {
                IPlayer targetedPlayer = null;

                foreach (IPlayer player in ResourceManager.Instance.ActivePlayerList)
                {
                    if (!player.IsActive)
                        continue;

                    //Get the distance
                    float newDist;
                    Vector2 playerPos = player.Position;
                    Vector2.Distance(ref playerPos, ref thisPos, out newDist);
                    if (newDist < distance)
                    {
                        distance = newDist;
                        targetedPlayer = player;
                    }

                }

                if (targetedPlayer != null)
                    this._target = (Sprite)targetedPlayer;
            }
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
            TurnToTarget(gameTime);

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

        private void TurnToTarget(GameTime gameTime)
        {
            if(_target == null || !_target.IsActive)
            {
                _target = null;
                return;
            }
            //Calculate angle
            float angle = (float)Math.Atan2(_target.Position.Y - this.Position.Y,
                _target.Position.X - this.Position.X);

            //Add up to maximum turn
            float maxTurnVal = MAX_TURN_VEL * ((float)gameTime.ElapsedGameTime.TotalSeconds);
            //Vector2 newVel = Vector2.Transform(this.Velocity, Matrix.CreateRotationX());
            //this.Velocity = newVel;

            float vel = Vector2.Distance(Vector2.Zero, this.Velocity);
            float currentAngle = (float)Math.Atan2(Velocity.Y, Velocity.X);
            float newAngle = (currentAngle > angle ? currentAngle - maxTurnVal : currentAngle + maxTurnVal);

            if (Math.Abs(newAngle - currentAngle) < Math.Abs(angle - currentAngle))
            {
                angle = newAngle;
            }

            this.Velocity = new Vector2((float)Math.Cos(angle) * vel, (float)Math.Sin(angle) * vel);

            //var transformed = Vector2.Transform(dir, Matrix.CreateRotationX(angle));
           // direction = new Point((int)dir.X, (int)dir.Y);
            //
        }
    }
}
