using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace EntityComponentSystem.Components
{
    /// <summary>
    /// Defines a single entities property to be drawn as 
    /// a static image
    /// </summary>
    public struct RenderObject
    {
        public uint EntityID;
        public Texture2D Image;
        public bool isVisible;
    }

    /// <summary>
    /// Manages the entities subscribed to be rendered
    /// </summary>
    public class RenderComponent : GameComponent<RenderObject>
    {
        //No extra functionality needed, The Render System should handle the rest
    }
}
