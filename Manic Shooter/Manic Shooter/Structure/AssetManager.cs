using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

/*
 * Notice, at the moment sounds do not work well with Monogame and require
 * a content pipeline project in order to use them. It will be a while before
 * Monogame fixes this issue
 */

/*
 *  Should Support:
 *  3D Models {.x, .fbx}
 *  Textures/Images {.bmp, .dds, .dib, .hdr, .jpg, .pfm, .ppm, .tga}
 *  Audio {.xap, .wma, .mp3, .wav}
 *  Fonts {.spritefont}
 *  Effects {.fx}
 *  
 * TODO:Expand functionality for TextureCube {.dds}
 *  
 *  Realistically these will all be .xnb files as that will be their
 *  compression after using the pipeline tool. The .xnb file extension
 *  is also one of the only ways to get Monogame to work with assets
 *  without fussing so much.
 */

namespace Component_Based_Base_Game.Structure
{
    /// <summary>
    /// IDentifier for sets of assets to be loaded all at once
    /// </summary>
    public enum LoadSets
    {
        All, //Will be included in all loadsets, ideal for critical assets such as the player or background music
    }

    public enum AssetType
    {
        Model,
        Texture,
        Sound,
        Effect,
        Font
    }

    /// <summary>
    /// Manages the assets of the game
    /// </summary>
    public static class AssetManager
    {
        //Dictionaries of assets to manage
        //The dictionaries need to be split up in this way to be able to serve up the different types of assets
        private static Dictionary<String, AssetProperties<Model>> ModelAssets = new Dictionary<string, AssetProperties<Model>>();
        private static Dictionary<String, AssetProperties<Texture2D>> TextureAssets = new Dictionary<String, AssetProperties<Texture2D>>();
        private static Dictionary<String, AssetProperties<SoundEffect>> SoundAssets = new Dictionary<String, AssetProperties<SoundEffect>>();
        private static Dictionary<String, AssetProperties<SpriteFont>> FontAssets = new Dictionary<String, AssetProperties<SpriteFont>>();
        private static Dictionary<String, AssetProperties<Effect>> EffectAssets = new Dictionary<String, AssetProperties<Effect>>();

        /// <summary>
        /// Tracks whether the asset dictionary has been initialized
        /// </summary>
        private static bool isInitialized = false;

        /// <summary>
        /// Content Manager for loading assets
        /// </summary>
        private static ContentManager _content;

        /// <summary>
        /// Adds all necessary assets to the asset dictionary so that they can be loaded and unloaded later
        /// </summary>
        public static void Initialize(ContentManager content)
        {
            _content = content;
            isInitialized = true;

            //Assets can hard coded to be loaded here or in the LoadContent Method.
            //Ideally here would be faster for the reduction in method calls but it's a process that should be relatively quick
            //either way since assets aren't **actually** being loaded at this point. They will be lazy loaded as needed.
            AssetManager.AddAsset(AssetType.Texture, "Player", @"Main_Character");
            AssetManager.AddAsset(AssetType.Sound, "Blop", @"Blop");
        }

        /// <summary>
        /// Adds an asset of the given type to the asset manager
        /// </summary>
        /// <param name="type">Type of the asset to add</param>
        /// <param name="name">Name of the asset to use as a reference</param>
        /// <param name="path">The file path that you would give to Content<T>.Load()"/></param>
        /// <param name="loadSets">The sets that this image is a part of</param>
        /// <returns>Whether the operation succeeded or not</returns>
        public static bool AddAsset(AssetType type, String name, String path, List<LoadSets> loadSets = null)
        {
            switch(type)
            {
                case AssetType.Model:
                    AssetProperties<Model> modelAsset = new AssetProperties<Model>(name, path);
                    if (loadSets != null) modelAsset.LoadSets = loadSets;
                    if (ModelAssets.ContainsKey(name)) throw new MemberAccessException("The name of this asset already exists.");
                    ModelAssets.Add(name, modelAsset);
                    return true;

                case AssetType.Texture:
                    AssetProperties<Texture2D> textureAsset = new AssetProperties<Texture2D>(name, path);
                    if (loadSets != null) textureAsset.LoadSets = loadSets;
                    if (TextureAssets.ContainsKey(name)) throw new MemberAccessException("The name of this asset already exists.");
                    TextureAssets.Add(name, textureAsset);
                    return true;

                case AssetType.Sound:
                    AssetProperties<SoundEffect> soundAsset = new AssetProperties<SoundEffect>(name, path);
                    if (loadSets != null) soundAsset.LoadSets = loadSets;
                    if (SoundAssets.ContainsKey(name)) throw new MemberAccessException("The name of this asset already exists.");
                    SoundAssets.Add(name, soundAsset);
                    return true;

                case AssetType.Font:
                    AssetProperties<SpriteFont> fontAsset = new AssetProperties<SpriteFont>(name, path);
                    if (loadSets != null) fontAsset.LoadSets = loadSets;
                    if (FontAssets.ContainsKey(name)) throw new MemberAccessException("The name of this asset already exists.");
                    FontAssets.Add(name, fontAsset);
                    return true;

                case AssetType.Effect:
                    AssetProperties<Effect> effectAsset = new AssetProperties<Effect>(name, path);
                    if (loadSets != null) effectAsset.LoadSets = loadSets;
                    if (EffectAssets.ContainsKey(name)) throw new MemberAccessException("The name of this asset already exists.");
                    EffectAssets.Add(name, effectAsset);
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a single Model asset
        /// </summary>
        /// <param name="name">Name of the asset to load</param>
        /// <returns>The Texture that was loaded</returns>
        public static Model GetModel(String name)
        {
            if (!isInitialized) throw new NullReferenceException("Asset Manager must be initialized before it can be used");
            if (!ModelAssets.ContainsKey(name)) throw new KeyNotFoundException("The given asset name is not a valid model");

            AssetProperties<Model> assetProperties = ModelAssets[name];

            if (assetProperties.Asset == null)
            {
                assetProperties.Asset = _content.Load<Model>(assetProperties.Path);
                ModelAssets[name] = assetProperties;
            }

            return assetProperties.Asset;
        }
        /// <summary>
        /// Gets a single Model asset
        /// </summary>
        /// <param name="name">Name of the asset to load</param>
        /// <returns>The Texture that was loaded</returns>
        public static Texture2D GetTexture(String name)
        {
            if (!isInitialized) throw new NullReferenceException("Asset Manager must be initialized before it can be used");
            if (!TextureAssets.ContainsKey(name)) throw new KeyNotFoundException("The given asset name is not a valid texture");

            AssetProperties<Texture2D> assetProperties = TextureAssets[name];

            if (assetProperties.Asset == null)
            {
                assetProperties.Asset = _content.Load<Texture2D>(assetProperties.Path);
                TextureAssets[name] = assetProperties;
            }

            return assetProperties.Asset;
        }
        /// <summary>
        /// Gets a single Sound Effect asset
        /// </summary>
        /// <param name="name">Name of the asset to load</param>
        /// <returns>The Texture that was loaded</returns>
        public static SoundEffect GetSound(String name)
        {
            if (!isInitialized) throw new NullReferenceException("Asset Manager must be initialized before it can be used");
            if (!SoundAssets.ContainsKey(name)) throw new KeyNotFoundException("The given asset name is not a valid sound");

            AssetProperties<SoundEffect> assetProperties = SoundAssets[name];

            if (assetProperties.Asset == null)
            {
                assetProperties.Asset = _content.Load<SoundEffect>(assetProperties.Path);
                SoundAssets[name] = assetProperties;
            }

            return assetProperties.Asset;
        }
        /// <summary>
        /// Gets a single Sprite Font asset
        /// </summary>
        /// <param name="name">Name of the asset to load</param>
        /// <returns>The Texture that was loaded</returns>
        public static SpriteFont GetFont(String name)
        {
            if (!isInitialized) throw new NullReferenceException("Asset Manager must be initialized before it can be used");
            if (!FontAssets.ContainsKey(name)) throw new KeyNotFoundException("The given asset name is not a valid font");

            AssetProperties<SpriteFont> assetProperties = FontAssets[name];

            if (assetProperties.Asset == null)
            {
                assetProperties.Asset = _content.Load<SpriteFont>(assetProperties.Path);
                FontAssets[name] = assetProperties;
            }

            return assetProperties.Asset;
        }
        /// <summary>
        /// Gets a single Effect asset
        /// </summary>
        /// <param name="name">Name of the asset to load</param>
        /// <returns>The Texture that was loaded</returns>
        public static Effect GetEffect(String name)
        {
            if (!isInitialized) throw new NullReferenceException("Asset Manager must be initialized before it can be used");
            if (!EffectAssets.ContainsKey(name)) throw new KeyNotFoundException("The given asset name is not a valid effect");

            AssetProperties<Effect> assetProperties = EffectAssets[name];

            if (assetProperties.Asset == null)
            {
                assetProperties.Asset = _content.Load<Effect>(assetProperties.Path);
                EffectAssets[name] = assetProperties;
            }

            return assetProperties.Asset;
        }

        /// <summary>
        /// Unloads all loaded content. This is also the only way to unload loadsets
        /// as the content manager has no way of unloading individual assets
        /// </summary>
        public static void Unload()
        {
            _content.Unload();
            //TODO:Unset all assets that have been loaded
        }

        /// <summary>
        /// Loads all assets in an assetSet
        /// </summary>
        /// <param name="loadSet">LoadSet to load in</param>
        public static void Load_LoadSet(LoadSets loadSet)
        {
            if (!isInitialized) throw new NullReferenceException("Asset Manager must be initialized before it can be used");

            List<KeyValuePair<String, AssetProperties<Model>>> models = ModelAssets.Where(x => x.Value.LoadSets.Contains(loadSet) || x.Value.LoadSets.Contains(LoadSets.All)).ToList();
            List<KeyValuePair<String, AssetProperties<Texture2D>>> textures = TextureAssets.Where(x => x.Value.LoadSets.Contains(loadSet) || x.Value.LoadSets.Contains(LoadSets.All)).ToList();
            List<KeyValuePair<String, AssetProperties<SoundEffect>>> sounds = SoundAssets.Where(x => x.Value.LoadSets.Contains(loadSet) || x.Value.LoadSets.Contains(LoadSets.All)).ToList();
            List<KeyValuePair<String, AssetProperties<SpriteFont>>> fonts = FontAssets.Where(x => x.Value.LoadSets.Contains(loadSet) || x.Value.LoadSets.Contains(LoadSets.All)).ToList();
            List<KeyValuePair<String, AssetProperties<Effect>>> effects = EffectAssets.Where(x => x.Value.LoadSets.Contains(loadSet) || x.Value.LoadSets.Contains(LoadSets.All)).ToList();

            foreach(KeyValuePair<String, AssetProperties<Model>> kvp in models)
            {
                GetModel(kvp.Key);
            }

            foreach(KeyValuePair<String, AssetProperties<Texture2D>> kvp in textures)
            {
                GetTexture(kvp.Key);
            }

            foreach(KeyValuePair<String, AssetProperties<SoundEffect>> kvp in sounds)
            {
                GetSound(kvp.Key);
            }

            foreach(KeyValuePair<String, AssetProperties<SpriteFont>> kvp in fonts)
            {
                GetFont(kvp.Key);
            }

            foreach(KeyValuePair<String, AssetProperties<Effect>> kvp in effects)
            {
                GetEffect(kvp.Key);
            }
        }


    }
}
