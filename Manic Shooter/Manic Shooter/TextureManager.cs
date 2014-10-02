using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

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
    }
}
