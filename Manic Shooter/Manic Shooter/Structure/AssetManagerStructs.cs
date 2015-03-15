using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Component_Based_Base_Game.Structure
{
    //TODO:Consider making this a class rather than a struct
    // it seems more appropriate since it can be large because of the load sets list

    /// <summary>
    /// Defines the generic properties that each asset will have
    /// </summary>
    public struct AssetProperties<T>
    {
        public String Name;
        public String Path;
        public List<LoadSets> LoadSets;
        public T Asset;

        public AssetProperties(String name, String path)
        {
            this.Name = name;
            this.Path = path;
            this.LoadSets = new List<LoadSets>();
            this.Asset = default(T);
        }
    }
}
