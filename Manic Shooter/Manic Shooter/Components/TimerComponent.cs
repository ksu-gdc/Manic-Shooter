using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityComponentSystem.Components;

namespace Manic_Shooter
{
    public struct Timer
    {
        public uint EntityID;
        public int Length;
        public int Value;
    }
    class TimerComponent:GameComponent<Timer>
    {
    }
}
