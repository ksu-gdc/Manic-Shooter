using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EntityComponentSystem.Interfaces;
using EntityComponentSystem.Components;
using EntityComponentSystem.Systems;
using Manic_Shooter;

namespace EntityComponentSystem.Systems
{
    /// <summary>
    /// Singleton System for rendering visual components to the screen.
    /// </summary>
    public class RenderSystem : ISystem
    {
        /// <summary>
        /// Stored instance of the RenderSystem
        /// </summary>
        private static RenderSystem _instance;
        
        /// <summary>
        /// Property for global read access to the instance of the RenderSystem
        /// </summary>
        public static RenderSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RenderSystem();

                return _instance;
            }
        }

        /// <summary>
        /// Constructor for the RenderSystem
        /// </summary>
        public RenderSystem()
        {
            if (!ComponentManagementSystem.Instance.ContainsComponent(typeof(RenderComponent)))
                ComponentManagementSystem.Instance.AddComponent(typeof(RenderComponent));
        }

        /// <summary>
        /// An update that occurs between frames. May be called more than once
        /// before anything is drawn
        /// </summary>
        /// <param name="gameTime">The time elapsed since the last update call</param>
        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// The draw phase of the game loop. This is called exactly once per
        /// frame.
        /// </summary>
        /// <param name="gameTime">The time elapsed since the last draw call</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            RenderComponent RenderComponent = ComponentManagementSystem.Instance.GetComponent<RenderComponent>();//ComponentManagementSystem.Instance.RenderComponent;
            PositionComponent PositionComponent = ComponentManagementSystem.Instance.GetComponent<PositionComponent>();
            RotationComponent RotationComponent = ComponentManagementSystem.Instance.GetComponent<RotationComponent>();

            RenderObject renderObject;
            Position position;
            Rotation rotation;

            Vector2? pointToVector;

            spriteBatch.Begin();
            foreach(uint id in RenderComponent.Keys)
            {
                renderObject = RenderComponent[id];

                if (PositionComponent.Contains(id))
                {
                    position = PositionComponent[id];
                    renderObject.drawRectangle = null;
                    pointToVector = new Vector2(position.Point.X, position.Point.Y);
                }
                else
                {
                    position = new Position();
                    pointToVector = null;
                    if(renderObject.drawRectangle == null)
                    {
                        renderObject.drawRectangle = new Rectangle(0, 0, renderObject.Image.Width, renderObject.Image.Height);
                    }
                }

                if(RotationComponent.Contains(id))
                    rotation = RotationComponent[id];
                else
                {
                    rotation = new Rotation();
                    rotation.Origin = Vector2.Zero;
                    rotation.Radians = 0f;
                }

                if(renderObject.isVisible)
                {
                    spriteBatch.Draw(renderObject.Image, pointToVector, renderObject.drawRectangle, renderObject.sourceRectangle, rotation.Origin, rotation.Radians,
                        renderObject.scale, renderObject.color, renderObject.effect, renderObject.depth);
                }
            }
            spriteBatch.End();
        }
    }
}
