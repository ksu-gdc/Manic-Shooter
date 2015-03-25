using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manic_Shooter;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using EntityComponentSystem.Interfaces;
using EntityComponentSystem.Components;
using EntityComponentSystem.Structure;
using EntityComponentSystem.Systems;

namespace Manic_Shooter
{
    class MovementSystem:ISystem
    {
        private static MovementSystem _instance;

        public static MovementSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MovementSystem();

                return _instance;
            }
        }

        public void Update(GameTime gameTime)
        {
            MovementComponent MovementComponent = ComponentManagementSystem.Instance.GetComponent<MovementComponent>();
            PositionComponent PositionComponent = ComponentManagementSystem.Instance.GetComponent<PositionComponent>();

            foreach(uint id in MovementComponent.Keys)
            {
                Position position = PositionComponent[id];
                Movement movement = MovementComponent[id];

                if (movement.Speed > movement.MaxSpeed) movement.Speed = movement.MaxSpeed;

                double speedAdjustment = movement.Speed * gameTime.ElapsedGameTime.TotalSeconds;

                float squared_magnitude = movement.VelocityVector.LengthSquared();

                if (squared_magnitude != 1 && squared_magnitude != 0)
                {
                    movement.VelocityVector.Normalize();
                }

                position.Point.X += (int)(movement.VelocityVector.X * speedAdjustment);
                position.Point.Y += (int)(movement.VelocityVector.Y * speedAdjustment);

                //Player Bounds
                if(id == ManicShooter.PlayerID)
                {
                    position.Point.X = MathHelper.Clamp(position.Point.X, ManicShooter.ScreenSize.Left, ManicShooter.ScreenSize.Right);
                    position.Point.Y = MathHelper.Clamp(position.Point.Y, ManicShooter.ScreenSize.Top, ManicShooter.ScreenSize.Bottom);
                }

                PositionComponent[id] = position;
                MovementComponent[id] = movement;
            }
        }
    }
}
