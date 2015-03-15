using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace EntityComponentSystem.Components
{
    public enum LoadSets
    {
        All,
    }

    //The required fields that each asset should contain
    public struct BaseAsset<T>
    {
        public String Name;
        public String Path;
        public List<LoadSets> LoadSets;
        public T Asset;

        public BaseAsset(String name, String path)
        {
            this.Name = name;
            this.Path = path;
            this.LoadSets = new List<LoadSets>();
            this.Asset = default(T);
        }
    }

    //The following are templates for various Assets
    //allowed by the game engine. These can easily be expanded
    public struct ModelAsset
    {
        public uint EntityID;
        public BaseAsset<Model> Model;
    }

    public struct TextureAsset
    {
        public uint EntityID;
        public BaseAsset<Texture2D> Texture;
    }

    public struct SoundAsset
    {
        public uint EntityID;
        public BaseAsset<SoundEffect> SoundEffect;
    }

    public struct SpriteFontAsset
    {
        public uint EntityID;
        public BaseAsset<SpriteFont> SpriteFont;
    }

    public struct EffectAsset
    {
        public uint EntityID;
        public BaseAsset<Effect> Effect;
    }

    public class ModelComponent : GameComponent<ModelAsset> { }
    public class TextureComponent : GameComponent<TextureAsset> { }
    public class SoundComponent : GameComponent<SoundAsset> { }
    public class SpriteFontComponent : GameComponent<SpriteFontAsset> { }
    public class EffectComponent : GameComponent<EffectAsset> { }

}
