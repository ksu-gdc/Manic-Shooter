using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using EntityComponentSystem.Components;
using EntityComponentSystem.Systems;

namespace EntityComponentSystem.Structure
{
    /// <summary>
    /// Global factory for creating all components in the game
    /// </summary>
    public class ComponentFactory
    {
        private static ComponentFactory _instance;

        public static ComponentFactory Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new ComponentFactory();

                return _instance;
            }
        }

        public uint CreatePlayer()
        {
            uint eid = IDManager.GetNewID();
            RenderObject renderObject = new RenderObject
            {
                EntityID = eid,
                isVisible = true,
                Image = AssetSystem.Instance.GetTexture("Player")
            };

            ComponentManagementSystem.Instance.GetComponent<RenderComponent>(RenderSystem._RENDERCOMPONENT_).Add(eid, renderObject);

            return eid;
        }

        public uint CreateModelAsset(String name, String path, List<LoadSets> loadsets = null)
        {
            uint eid = IDManager.GetNewID();
            ModelAsset modelAsset = new ModelAsset()
            {
                EntityID = eid,
                Model = new BaseAsset<Model>(name, path)
            };
            if (loadsets != null) modelAsset.Model.LoadSets = loadsets;

            ComponentManagementSystem.Instance.GetComponent<ModelComponent>(AssetSystem._MODELCOMPONENT_).Add(eid, modelAsset);
            //ComponentManagementSystem.Instance.ModelComponent.Add(eid, modelAsset);
            return eid;
        }

        public uint CreateTextureAsset(String name, String path, List<LoadSets> loadsets = null)
        {
            uint eid = IDManager.GetNewID();
            TextureAsset textureAsset = new TextureAsset()
            {
                EntityID = eid,
                Texture = new BaseAsset<Texture2D>(name, path)
            };
            if (loadsets != null) textureAsset.Texture.LoadSets = loadsets;

            ComponentManagementSystem.Instance.GetComponent<TextureComponent>(AssetSystem._TEXTURECOMPONENT_).Add(eid, textureAsset);
            
            return eid;
        }

        public uint CreateSoundEffectAsset(String name, String path, List<LoadSets> loadsets = null)
        {
            uint eid = IDManager.GetNewID();
            SoundAsset soundAsset = new SoundAsset()
            {
                EntityID = eid,
                SoundEffect = new BaseAsset<SoundEffect>(name, path)
            };
            if(loadsets != null) soundAsset.SoundEffect.LoadSets = loadsets;

            ComponentManagementSystem.Instance.GetComponent<SoundComponent>(AssetSystem._SOUNDCOMPONENT_).Add(eid, soundAsset);

            return eid;
        }

        public uint CreateSpriteFontAsset(String name, String path, List<LoadSets> loadsets = null)
        {
            uint eid = IDManager.GetNewID();
            SpriteFontAsset spriteFontAsset = new SpriteFontAsset()
            {
                EntityID = eid,
                SpriteFont = new BaseAsset<SpriteFont>(name, path)
            };
            if (loadsets != null) spriteFontAsset.SpriteFont.LoadSets = loadsets;

            ComponentManagementSystem.Instance.GetComponent<SpriteFontComponent>(AssetSystem._SPRITEFONTCOMPONENT_).Add(eid, spriteFontAsset);

            return eid;
        }

        public uint CreateEffectAsset(String name, String path, List<LoadSets> loadsets = null)
        {
            uint eid = IDManager.GetNewID();
            EffectAsset effectAsset = new EffectAsset()
            {
                EntityID = eid,
                Effect = new BaseAsset<Effect>(name, path)
            };
            if (loadsets != null) effectAsset.Effect.LoadSets = loadsets;

            ComponentManagementSystem.Instance.GetComponent<EffectComponent>(AssetSystem._EFFECTCOMPONENT_).Add(eid, effectAsset);

            return eid;
        }
    }
}
