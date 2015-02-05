using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Manic_Shooter
{
    public class TextureManager
    {
        private Dictionary<string, Texture2D> Textures;
        private static TextureManager _instance;

        public static TextureManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TextureManager();
                }

                return _instance;
            }
        }

        public Texture2D GetTexture(string textureName)
        {
            return Textures[textureName];
        }

        public void AddTexture(string name, Texture2D Texture)
        {
            Textures.Add(name, Texture);
        }

        private TextureManager()
        {
            Textures = new Dictionary<string, Texture2D>();
        }

        //Guess what I stole from StackOverflow
        public Texture2D createCircleText(GraphicsDevice graphics, int radius, Color circleColor)
        {
            Texture2D texture = new Texture2D(graphics, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = circleColor;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
