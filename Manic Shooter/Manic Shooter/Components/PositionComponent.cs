using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EntityComponentSystem.Components;

namespace Manic_Shooter
{
    public struct Position
    {
        public uint EntityID;
        public Point Point;
    }

    class PositionComponent:GameComponent<Position>
    {
    }
}
