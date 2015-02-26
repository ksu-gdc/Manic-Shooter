using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manic_Shooter.Interfaces
{
    interface IDroppable : ISprite
    {
        void ApplyEffect(IPlayer player);
    }
}
