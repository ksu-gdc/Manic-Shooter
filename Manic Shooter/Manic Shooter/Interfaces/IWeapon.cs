using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Manic_Shooter.Interfaces
{
    interface IWeapon
    {
        Vector2 MuzzleOffset { get; }
        void Fire();
        bool IsCoolingDown();
        void SetReferencePosition(int x, int y);
        void SetReferencePosition(Vector2 newPosition);
        void Update(GameTime gameTime);
    }
}
