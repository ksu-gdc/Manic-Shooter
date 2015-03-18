using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EntityComponentSystem.Components
{
    /// <summary>
    /// Defines a single entities property to be drawn as 
    /// a static image
    /// </summary>
    public struct RenderObject
    {
        public uint EntityID;
        public Texture2D Image; //TODO:Could possibly be in Animation Component
        public bool isVisible;
        public Rectangle? drawRectangle;
        public Rectangle? sourceRectangle;
        //public Vector2 origin; //HACK: Included this in Rotation Component, We'll see how it works
        public Vector2 scale;
        public Color color;
        public SpriteEffects effect;
        public float depth;
    }

    /// <summary>
    /// Manages the entities subscribed to be rendered
    /// </summary>
    public class RenderComponent : GameComponent<RenderObject>
    {
        //No extra functionality needed, The Render System should handle the rest
    }
}
