using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EntityComponentSystem.Components;

namespace Manic_Shooter
{
    public struct Movement
    {
        public uint EntityID;
        public Vector2 VelocityVector;
        public int Speed;
        public int MaxSpeed;
    }

    class MovementComponent:GameComponent<Movement>
    {
    }
}
