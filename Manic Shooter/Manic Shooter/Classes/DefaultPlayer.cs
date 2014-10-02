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
    class DefaultPlayer: Sprite, IPlayer, ISprite
    {
        private const float HORIZ_SPEED = 60;
        private const float VERT_SPEED = 50;

        public DefaultPlayer(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            //Set up the keyboard movement/shooting events
            EnableKeyboardEvents(true);
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
            //DefaultProjectile proj = new DefaultProjectile(Content.Load<Texture2D>("Projectile_placeholder.png"), new Vector2(200, 200), 10);
            //ResourceManager.Instance.AddProjectile(
            DefaultProjectile defProj = new DefaultProjectile(TextureManager.Instance.GetTexture("DefaultProjectile"), this.Position, new Vector2(0, -120), 10);
            ResourceManager.Instance.AddProjectile(defProj);
        }

        public void SetVelocity(Vector2 newVelocity)
        {
            this.Velocity = newVelocity;
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
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            this.Position += this.Velocity * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000);
        }
    }
}
