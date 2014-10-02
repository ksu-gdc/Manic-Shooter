using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Manic_Shooter.Interfaces
{
    interface IInputManager
    {
        /// <summary>
        /// Used to update the Manager and see the latest changes
        /// in the user's input.
        /// </summary>
        /// <param name="gameTime">The amount of time elapsed since the last call</param>
        void Update(GameTime gameTime);

    }
}
