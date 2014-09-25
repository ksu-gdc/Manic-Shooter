using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manic_Shooter.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manic_Shooter.Classes
{
    class DefaultEnemy:Sprite, IEnemy
    {
        public DefaultEnemy(Texture2D texture, Vector2 position, int health)
            : base(texture, position, health)
        {

        }
    }
}
