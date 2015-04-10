using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using Manic_Shooter.Interfaces;
using Manic_Shooter.Classes;

namespace Manic_Shooter.Classes
{
    public enum PlayerState
    {
        Invincible,
        Normal,
        Dead,
    }

    public enum GunUpgradeState
    {
        None,
        SinglePellet,
        DoublePellet,
        TriplePellet
    }

    public enum MissileUpgradeState
    {
        None,
        SingleMissile,
        DoubleMissile
    }

    class DefaultPlayer: Sprite, IPlayer, ISprite
    {
        private const float HORIZ_SPEED = 200;
        private const float VERT_SPEED = 180;

        private ushort lives = 3;
        private PlayerState pstate = PlayerState.Normal;
        private int maxDeathTime = 2000; //milliseconds
        private int maxInvincibleTime = 1000; //milliseconds
        private int timer = 0; //millisecond counter

        public bool isGameOver
        {
            get { return (this.lives <= 0 && this.pstate == PlayerState.Dead); }
        }

        public int Lives
        {
            get { return this.lives; }
        }

        public int Score
        {
            get;
            private set;
        }

        private List<IWeapon> _weapons;
        private bool _shootWeapon;

        private TimeSpan lastShot;
        private GunUpgradeState _currentGunUpgrade;
        private MissileUpgradeState _currentMissileUpgrade;

        public void UpgradeGun(System.Type weaponType)
        {
            bool gunChanged = false;
            bool missileChanged = false;

            if (weaponType == typeof(PelletGun))
            {
                if (_currentGunUpgrade != GunUpgradeState.SinglePellet &&
                    _currentGunUpgrade != GunUpgradeState.DoublePellet &&
                    _currentGunUpgrade != GunUpgradeState.TriplePellet)
                {
                    gunChanged = true;
                    _currentGunUpgrade = GunUpgradeState.SinglePellet;
                }
                else if (_currentGunUpgrade == GunUpgradeState.SinglePellet)
                {
                    gunChanged = true;
                    _currentGunUpgrade = GunUpgradeState.DoublePellet;
                }
                else if (_currentGunUpgrade == GunUpgradeState.DoublePellet)
                {
                    gunChanged = true;
                    _currentGunUpgrade = GunUpgradeState.TriplePellet;
                }
            }
            else if (weaponType == typeof(MissileLauncher))
            {
                if (_currentMissileUpgrade != MissileUpgradeState.SingleMissile &&
                    _currentMissileUpgrade != MissileUpgradeState.DoubleMissile)
                {
                    missileChanged = true;
                    _currentMissileUpgrade = MissileUpgradeState.SingleMissile;
                }
                else if (_currentMissileUpgrade == MissileUpgradeState.SingleMissile)
                {
                    missileChanged = true;
                    _currentMissileUpgrade = MissileUpgradeState.DoubleMissile;
                }
            }

            if (gunChanged)
            {
                _weapons.RemoveAll(x => x.GetType() == typeof(PelletGun));
                switch (_currentGunUpgrade)
                {
                    case GunUpgradeState.SinglePellet:
                        _weapons.Add(new PelletGun(this.centerPosition, new Vector2(0, -this.Height / 3), new Vector2(0, -350), 100, true));
                        break;
                    case GunUpgradeState.DoublePellet:
                        _weapons.Add(new PelletGun(this.centerPosition, new Vector2(-this.Width / 3, -this.Height / 3), new Vector2(0, -350), 100, true));
                        _weapons.Add(new PelletGun(this.centerPosition, new Vector2(this.Width / 3, -this.Height / 3), new Vector2(0, -350), 100, true));
                        break;
                    case GunUpgradeState.TriplePellet:
                        _weapons.Add(new PelletGun(this.centerPosition, new Vector2(0, -this.Height / 3), new Vector2(0, -350), 100, true));
                        _weapons.Add(new PelletGun(this.centerPosition, new Vector2(-this.Width / 3, -this.Height / 3), new Vector2(-100, -300), 100, true));
                        _weapons.Add(new PelletGun(this.centerPosition, new Vector2(this.Width / 3, -this.Height / 3), new Vector2(100, -300), 100, true));
                        break;
                }
            }
            else if (missileChanged)
            {
                _weapons.RemoveAll(x => x.GetType() == typeof(MissileLauncher));
                switch (_currentMissileUpgrade)
                {
                    case MissileUpgradeState.SingleMissile:
                        _weapons.Add(new MissileLauncher(this.centerPosition, new Vector2(0, 0), new Vector2(0, -200), 500, true));
                        break;
                    case MissileUpgradeState.DoubleMissile:
                        _weapons.Add(new MissileLauncher(this.centerPosition, new Vector2(-this.Width / 3, 0), new Vector2(0, -200), 500, true));
                        _weapons.Add(new MissileLauncher(this.centerPosition, new Vector2(this.Width / 3, 0), new Vector2(0, -200), 500, true));
                        break;
                }
            }
        }

        public DefaultPlayer(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            //Set up the keyboard movement/shooting events
            EnableKeyboardEvents(true);

            int hitboxWidth = 11, hitboxHeight = 11;
            int hitboxX = this.TextureBox.X + (int)((float)(this.TextureBox.Width) / 2 - (float)(hitboxWidth) / 2);
            int hitboxY = this.TextureBox.Y + (int)((float)(this.TextureBox.Height) / 2- (float)(hitboxHeight) / 2);
            this.HitBoxRadius = 8;
            this.HitBoxVertRatio = 0.4f;

            this.Health = 5;
            this.MaxHealth = this.Health;
            _drawHitbox = true;
            _weapons = new List<IWeapon>();

            //Init to pellet gun?
            _currentGunUpgrade = GunUpgradeState.None;
            UpgradeGun(typeof(PelletGun));
        }

        private void EnableKeyboardEvents(bool enabled)
        {
            if (enabled)
            {
                KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.MoveDown, gameKey_moveDownPressed);
                KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.MoveUp, gameKey_moveUpPressed);
                KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.MoveRight, gameKey_moveRightPressed);
                KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.MoveLeft, gameKey_moveLeftPressed);
                KeyboardManager.Instance.AddGameKeyPressed(KeyboardManager.GameKeys.Shoot, gameKey_shootPressed);

                KeyboardManager.Instance.AddGameKeyReleased(KeyboardManager.GameKeys.MoveDown, gameKey_moveDownReleased);
                KeyboardManager.Instance.AddGameKeyReleased(KeyboardManager.GameKeys.MoveUp, gameKey_moveUpReleased);
                KeyboardManager.Instance.AddGameKeyReleased(KeyboardManager.GameKeys.MoveRight, gameKey_moveRightReleased);
                KeyboardManager.Instance.AddGameKeyReleased(KeyboardManager.GameKeys.MoveLeft, gameKey_moveLeftReleased);
            }
            else
            {
                KeyboardManager.Instance.RemoveGameKeyPressed(KeyboardManager.GameKeys.MoveDown, gameKey_moveDownPressed);
                KeyboardManager.Instance.RemoveGameKeyPressed(KeyboardManager.GameKeys.MoveUp, gameKey_moveUpPressed);
                KeyboardManager.Instance.RemoveGameKeyPressed(KeyboardManager.GameKeys.MoveRight, gameKey_moveRightPressed);
                KeyboardManager.Instance.RemoveGameKeyPressed(KeyboardManager.GameKeys.MoveLeft, gameKey_moveLeftPressed);
                KeyboardManager.Instance.RemoveGameKeyPressed(KeyboardManager.GameKeys.Shoot, gameKey_shootPressed);

                KeyboardManager.Instance.RemoveGameKeyReleased(KeyboardManager.GameKeys.MoveDown, gameKey_moveDownReleased);
                KeyboardManager.Instance.RemoveGameKeyReleased(KeyboardManager.GameKeys.MoveUp, gameKey_moveUpReleased);
                KeyboardManager.Instance.RemoveGameKeyReleased(KeyboardManager.GameKeys.MoveRight, gameKey_moveRightReleased);
                KeyboardManager.Instance.RemoveGameKeyReleased(KeyboardManager.GameKeys.MoveLeft, gameKey_moveLeftReleased);
            }
        }

        public void gameKey_moveDownPressed(Keys key)
        {
            AddVelocity(new Vector2(0, VERT_SPEED));
        }

        public void gameKey_moveUpPressed(Keys key)
        {
            AddVelocity(new Vector2(0, -VERT_SPEED));
        }

        public void gameKey_moveLeftPressed(Keys key)
        {
            AddVelocity(new Vector2(-HORIZ_SPEED, 0));
        }

        public void gameKey_moveRightPressed(Keys key)
        {
            AddVelocity(new Vector2(HORIZ_SPEED, 0));
        }

        public void gameKey_moveDownReleased(Keys key)
        {
            AddVelocity(new Vector2(0, -VERT_SPEED));
        }

        public void gameKey_moveUpReleased(Keys key)
        {
            AddVelocity(new Vector2(0, VERT_SPEED));
        }

        public void gameKey_moveLeftReleased(Keys key)
        {
            AddVelocity(new Vector2(HORIZ_SPEED, 0));
        }

        public void gameKey_moveRightReleased(Keys key)
        {
            AddVelocity(new Vector2(-HORIZ_SPEED, 0));
        }

        public void gameKey_shootPressed(Keys key)
        {
            _shootWeapon = true;
        }

        public void AddVelocity(Vector2 appliedVelocity, uint maxSpeed = 200000)
        {
            Vector2 currentVelocity = this.Velocity;
            Vector2.Add(ref currentVelocity, ref appliedVelocity, out currentVelocity);
            if (currentVelocity.LengthSquared() > (maxSpeed * maxSpeed))
            {
                currentVelocity.Normalize();
                currentVelocity = Vector2.Multiply(currentVelocity, maxSpeed);
            }

            this.Velocity = currentVelocity;
        }

        public void Fire()
        {
            foreach (IWeapon weapon in _weapons)
            {
                weapon.Fire();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (pstate == PlayerState.Invincible || pstate == PlayerState.Normal)
            {
                Vector2 deltaV = this.Velocity * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                this.MoveBy(deltaV.X, deltaV.Y);
            }

            UpdateWeaponPositions();
            foreach (IWeapon weapon in _weapons)
                weapon.Update(gameTime);

            if(pstate == PlayerState.Invincible)
            {
                this.SpriteTint = new Color(255, 0, 0, 255);
                this._hurtFlashing = true;
            }

            if (KeyboardManager.Instance.IsKeyDown(KeyboardManager.GameKeys.Shoot))//_shootWeapon)
            {
                Fire();
                lastShot = gameTime.TotalGameTime;
                //_shootWeapon = false;
            }

            if(timer > 0)
            {
                timer -= gameTime.ElapsedGameTime.Milliseconds;
            }

            if(timer <= 0)
            {
                if(pstate == PlayerState.Dead)
                {
                    pstate = PlayerState.Invincible;
                    //EnableKeyboardEvents(true);
                    ResetVelocityOnRevive();
                    timer = maxInvincibleTime;
                }
                else if(pstate == PlayerState.Invincible)
                {
                    pstate = PlayerState.Normal;
                    timer = 0;
                }
            }

            //We can also use gameTime.ElapsedGameTime.TotalSeconds to achieve the same value without the division
            //this.Position += this.Velocity * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000);

        }

        private void ResetVelocityOnRevive()
        {
            this.Velocity = Vector2.Zero;
            if (KeyboardManager.Instance.IsKeyDown(KeyboardManager.GameKeys.MoveUp))
                gameKey_moveUpPressed(Keys.A);
            if (KeyboardManager.Instance.IsKeyDown(KeyboardManager.GameKeys.MoveDown))
                gameKey_moveDownPressed(Keys.A);
            if (KeyboardManager.Instance.IsKeyDown(KeyboardManager.GameKeys.MoveLeft))
                gameKey_moveLeftPressed(Keys.A);
            if (KeyboardManager.Instance.IsKeyDown(KeyboardManager.GameKeys.MoveRight))
                gameKey_moveRightPressed(Keys.A);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            bool render = true;
            switch(pstate)
            {
                case PlayerState.Normal:
                    render = true;
                    break;
                case PlayerState.Invincible:
                    if(this._hurtFlashing)
                    {
                        this._hurtFlashing = false;
                    }
                    else
                    {
                        base.Render(spriteBatch);
                        this._hurtFlashing = true;
                    }
                    render = true;
                    break;
                case PlayerState.Dead:
                default:
                    render = false;
                    break;
            }

            if(render)
                base.Render(spriteBatch);
        }
        public new void SetVelocity(Vector2 newVelocity)
        {
            //HACK: Need to consider removing this from IPlayer as 
            //well as possibly adding a _maxspeed instead of fixed speed's in either direction
            this.SetVelocity(newVelocity,(int) HORIZ_SPEED);
        }

        public void HitBy(IProjectile projectile)
        {
            if (pstate == PlayerState.Normal)
            {
                this.Health -= projectile.GetDamage();
                if(this.Health > 0)
                    this.DebugFlash();
                else
                {
                    this.Destroy();
                    timer = maxDeathTime;
                }
            }
        }

        public override void Destroy()
        {
            if (lives <= 0)
            {
                this.EnableKeyboardEvents(false);
                //ManicShooter.GameState = ManicShooter.gameStates.GameOver;
                base.Destroy();
            }
            else
            {
                lives--;
                this.Health = this.MaxHealth;
                pstate = PlayerState.Dead;
            }
        }

        private void DebugFlash()
        {
            //this.SpriteTint = new Color(255, this.SpriteTint.G, this.SpriteTint.B, this.SpriteTint.A);
            this.SpriteTint = new Color(255, 0, 0, 255);
            this._hurtFlashing = true;
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

        public void AddScore(int additionalPoints)
        {
            this.Score += additionalPoints;
        }

        public void ClearScore()
        {
            this.Score = 0;
        }
    }
}
