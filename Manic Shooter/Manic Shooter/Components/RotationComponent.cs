using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EntityComponentSystem.Components;

namespace Manic_Shooter
{
    public struct Rotation
    {
        public uint EntityID;
        public float Radians;
        public Vector2 Origin;
    }

    class RotationComponent:GameComponent<Rotation>
    {
    }
}
