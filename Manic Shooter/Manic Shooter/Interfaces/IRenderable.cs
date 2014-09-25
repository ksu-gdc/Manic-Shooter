using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Manic_Shooter.Interfaces
{
    interface IRenderable
    {
        /// <summary>
        /// Renders the renderable object to the screen
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use for rendering</param>
        void Render(SpriteBatch spriteBatch);

        /// <summary>
        /// Makes the renderable object visible
        /// </summary>
        void Show();

        /// <summary>
        /// Makes the renderable object invisible
        /// </summary>
        void Hide();

        /// <summary>
        /// The visibility of the renderable object
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Whether the resource is active or not. If not then the memory used for this
        /// resource can be reused rather than deallocated with this property.
        /// </summary>
        bool IsActive { get; set; }
    }
}
