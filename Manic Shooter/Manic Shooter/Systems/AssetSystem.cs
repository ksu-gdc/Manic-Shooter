using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityComponentSystem.Interfaces;
using EntityComponentSystem.Components;
using EntityComponentSystem.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace EntityComponentSystem.Systems
{
    public enum AssetType
    {
        Model,
        Texture,
        Sound,
        Effect,
        SpriteFont
    }
    
    public class AssetSystem : ISystem
    {
        public const string _MODELCOMPONENT_ = "ModelComponent";
        public const string _TEXTURECOMPONENT_ = "TextureComponent";
        public const string _SOUNDCOMPONENT_ = "SoundComponent";
        public const string _SPRITEFONTCOMPONENT_ = "SpriteFontComponent";
        public const string _EFFECTCOMPONENT_ = "EffectComponent";

        /// <summary>
        /// Stored instance of the AssetSystem
        /// </summary>
        private static AssetSystem _instance;

        public static AssetSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AssetSystem();

                return _instance;
            }
        }

        public ContentManager _content;
        public bool _isInitialized;

        public void InitializeContent(ContentManager content)
        {
            _content = content;
            _isInitialized = true;

            //Assets can hard coded to be loaded here or in the LoadContent Method.
            //Ideally here would be faster for the reduction in method calls but it's a process that should be relatively quick
            //either way since assets aren't **actually** being loaded at this point. They will be lazy loaded as needed.
            //this.AddAsset(AssetType.Texture, "Player", @"Main_Character");
            //this.AddAsset(AssetType.Sound, "Blop", @"Blop");
        }

        public void AddAsset(AssetType type, String name, String path, List<LoadSets> loadSets = null)
        {
            switch(type)
            {
                case AssetType.Model:
                    if (!ComponentManagementSystem.Instance.ContainsComponent(_MODELCOMPONENT_))
                        ComponentManagementSystem.Instance.AddComponent(_MODELCOMPONENT_, new ModelComponent());
                    ComponentFactory.Instance.CreateModelAsset(name, path, loadSets);
                    return;
                case AssetType.Texture:
                    if (!ComponentManagementSystem.Instance.ContainsComponent(_TEXTURECOMPONENT_))
                        ComponentManagementSystem.Instance.AddComponent(_TEXTURECOMPONENT_, new TextureComponent());
                    ComponentFactory.Instance.CreateTextureAsset(name, path, loadSets);
                    return;
                case AssetType.Sound:
                    if (!ComponentManagementSystem.Instance.ContainsComponent(_SOUNDCOMPONENT_))
                        ComponentManagementSystem.Instance.AddComponent(_SOUNDCOMPONENT_, new SoundComponent());
                    ComponentFactory.Instance.CreateSoundEffectAsset(name, path, loadSets);
                    return;
                case AssetType.SpriteFont:
                    if (!ComponentManagementSystem.Instance.ContainsComponent(_SPRITEFONTCOMPONENT_))
                        ComponentManagementSystem.Instance.AddComponent(_SPRITEFONTCOMPONENT_, new SpriteFontComponent());
                    ComponentFactory.Instance.CreateSpriteFontAsset(name, path, loadSets);
                    return;
                case AssetType.Effect:
                    if (!ComponentManagementSystem.Instance.ContainsComponent(_EFFECTCOMPONENT_))
                        ComponentManagementSystem.Instance.AddComponent(_EFFECTCOMPONENT_, new EffectComponent());
                    ComponentFactory.Instance.CreateEffectAsset(name, path, loadSets);
                    return;
            }

            throw new Exception("Attempted to load unknown asset type");
        }
        
        public Model GetModel(String name)
        {
            if (!_isInitialized) throw new NullReferenceException("Asset System must be initialized before it can be used");
            List<ModelAsset> modelList = ComponentManagementSystem.Instance.GetComponent<ModelComponent>(_MODELCOMPONENT_).ToList();
            //List<ModelAsset> modelList = ComponentManagementSystem.Instance.ModelComponent.ToList();
            
            ModelAsset asset = modelList.Find(x => x.Model.Name.Equals(name));

            if (asset.Model.Name != name) throw new KeyNotFoundException("The given asset name is not a valid model");

            return LoadModel(asset);
        }

        public Texture2D GetTexture(String name)
        {
            if (!_isInitialized) throw new NullReferenceException("Asset System must be initialized before it can be used");
            List<TextureAsset> textureList = ComponentManagementSystem.Instance.GetComponent<TextureComponent>(_TEXTURECOMPONENT_).ToList();

            TextureAsset asset = textureList.Find(x => x.Texture.Name.Equals(name));

            if (asset.Texture.Name != name) throw new KeyNotFoundException("The given asset name is not a valid 2D texture");

            return LoadTexture(asset);
        }

        public SoundEffect GetSoundEffect(String name)
        {
            if (!_isInitialized) throw new NullReferenceException("Asset System must be initialized before it can be used");
            List<SoundAsset> soundList = ComponentManagementSystem.Instance.GetComponent<SoundComponent>(_SOUNDCOMPONENT_).ToList();

            SoundAsset asset = soundList.Find(x => x.SoundEffect.Name.Equals(name));

            if (asset.SoundEffect.Name != name) throw new KeyNotFoundException("The given asset name is not a valid sound effect");

            return LoadSoundEffect(asset);
        }

        public SpriteFont GetSpriteFont(String name)
        {
            if (!_isInitialized) throw new NullReferenceException("Asset System must be initialized before it can be used");
            List<SpriteFontAsset> spriteFontList = ComponentManagementSystem.Instance.GetComponent<SpriteFontComponent>(_SPRITEFONTCOMPONENT_).ToList();

            SpriteFontAsset asset = spriteFontList.Find(x => x.SpriteFont.Name.Equals(name));

            if (asset.SpriteFont.Name != name) throw new KeyNotFoundException("The given asset name is not a valid sprite font");

            return LoadSpriteFont(asset);
        }

        public Effect GetEffect(String name)
        {
            if (!_isInitialized) throw new NullReferenceException("Asset System must be initialized before it can be used");
            List<EffectAsset> effectList = ComponentManagementSystem.Instance.GetComponent<EffectComponent>(_EFFECTCOMPONENT_).ToList();

            EffectAsset asset = effectList.Find(x => x.Effect.Name.Equals(name));

            if (asset.Effect.Name != name) throw new KeyNotFoundException("The given asset name is not a valid effect");

            return LoadEffect(asset);
        }

        public Model LoadModel(ModelAsset asset)
        {
            if (!_isInitialized) throw new NullReferenceException("Asset System must be initialized before it can be used");

            if (asset.Model.Asset == null)
                asset.Model.Asset = _content.Load<Model>(asset.Model.Path);

            return asset.Model.Asset;
        }

        public Texture2D LoadTexture(TextureAsset asset)
        {
            if (!_isInitialized) throw new NullReferenceException("Asset System must be initialized before it can be used");

            if (asset.Texture.Asset == null)
                asset.Texture.Asset = _content.Load<Texture2D>(asset.Texture.Path);

            return asset.Texture.Asset;
        }

        public SoundEffect LoadSoundEffect(SoundAsset asset)
        {
            if (!_isInitialized) throw new NullReferenceException("Asset System must be initialized before it can be used");

            if (asset.SoundEffect.Asset == null)
                asset.SoundEffect.Asset = _content.Load<SoundEffect>(asset.SoundEffect.Path);

            return asset.SoundEffect.Asset;
        }

        public SpriteFont LoadSpriteFont(SpriteFontAsset asset)
        {
            if (!_isInitialized) throw new NullReferenceException("Asset System must be initialized before it can be used");

            if (asset.SpriteFont.Asset == null)
                asset.SpriteFont.Asset = _content.Load<SpriteFont>(asset.SpriteFont.Path);

            return asset.SpriteFont.Asset;
        }

        public Effect LoadEffect(EffectAsset asset)
        {
            if (!_isInitialized) throw new NullReferenceException("Asset System must be initialized before it can be used");

            if (asset.Effect.Asset == null)
                asset.Effect.Asset = _content.Load<Effect>(asset.Effect.Path);

            return asset.Effect.Asset;
        }

        public void Unload()
        {
            if (!_isInitialized) throw new NullReferenceException("Asset System must be initialized before it can be used");
            //unloads all content, should be a way around this with more
            //content managers, but needs more research
            //TODO: find a way to unload individual assets
            _content.Unload();
        }

        public void Load_LoadSet(LoadSets loadSet)
        {
            if (!_isInitialized) throw new NullReferenceException("Asset System must be initialized before it can be used");
            
            List<ModelAsset> modelList = ComponentManagementSystem.Instance.GetComponent<ModelComponent>(_MODELCOMPONENT_).All.Where(x => x.Model.LoadSets.Contains(loadSet) || x.Model.LoadSets.Contains(LoadSets.All)).ToList();
            List<TextureAsset> textureList = ComponentManagementSystem.Instance.GetComponent<TextureComponent>(_TEXTURECOMPONENT_).All.Where(x => x.Texture.LoadSets.Contains(loadSet) || x.Texture.LoadSets.Contains(LoadSets.All)).ToList();
            List<SoundAsset> soundList = ComponentManagementSystem.Instance.GetComponent<SoundComponent>(_SOUNDCOMPONENT_).All.Where(x => x.SoundEffect.LoadSets.Contains(loadSet) || x.SoundEffect.LoadSets.Contains(LoadSets.All)).ToList();
            List<SpriteFontAsset> spriteFontList = ComponentManagementSystem.Instance.GetComponent<SpriteFontComponent>(_SPRITEFONTCOMPONENT_).All.Where(x => x.SpriteFont.LoadSets.Contains(loadSet) || x.SpriteFont.LoadSets.Contains(LoadSets.All)).ToList();
            List<EffectAsset> effectList = ComponentManagementSystem.Instance.GetComponent<EffectComponent>(_EFFECTCOMPONENT_).All.Where(x => x.Effect.LoadSets.Contains(loadSet) || x.Effect.LoadSets.Contains(LoadSets.All)).ToList();

            foreach(ModelAsset m in modelList)
            {
                LoadModel(m);
            }
            foreach(TextureAsset t in textureList)
            {
                LoadTexture(t);
            }
            foreach(SoundAsset s in soundList)
            {
                LoadSoundEffect(s);
            }
            foreach(SpriteFontAsset sf in spriteFontList)
            {
                LoadSpriteFont(sf);
            }
            foreach(EffectAsset e in effectList)
            {
                LoadEffect(e);
            }
        }
    }
}
