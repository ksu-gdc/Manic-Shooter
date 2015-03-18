using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using EntityComponentSystem.Components;
using EntityComponentSystem.Systems;
using Manic_Shooter;

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
                Image = AssetSystem.Instance.GetTexture("Player"),
                drawRectangle = null,
                sourceRectangle = null,
                depth = 1.0f,
                color = Color.White,
                effect = SpriteEffects.None,
                scale = new Vector2(0.5f,0.5f)
            };
            ComponentManagementSystem.Instance.GetComponent<RenderComponent>().Add(eid, renderObject);

            Position position = new Position
            {
                EntityID = eid,
                Point = Point.Zero
            };
            ComponentManagementSystem.Instance.GetComponent<PositionComponent>().Add(eid, position);

            Movement movement = new Movement
            {
                EntityID = eid,
                Speed = 500,
                MaxSpeed = 500,
                VelocityVector = new Vector2(0,0)
            };
            ComponentManagementSystem.Instance.GetComponent<MovementComponent>().Add(eid, movement);

            Rotation rotation = new Rotation
            {
                EntityID = eid,
                Origin = new Vector2(renderObject.Image.Width/2, renderObject.Image.Height/2),
                Radians = 0f
            };
            ComponentManagementSystem.Instance.GetComponent<RotationComponent>().Add(eid, rotation);
            
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

            ComponentManagementSystem.Instance.GetComponent<ModelComponent>().Add(eid, modelAsset);
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

            ComponentManagementSystem.Instance.GetComponent<TextureComponent>().Add(eid, textureAsset);
            
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

            ComponentManagementSystem.Instance.GetComponent<SoundComponent>().Add(eid, soundAsset);

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

            ComponentManagementSystem.Instance.GetComponent<SpriteFontComponent>().Add(eid, spriteFontAsset);

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

            ComponentManagementSystem.Instance.GetComponent<EffectComponent>().Add(eid, effectAsset);

            return eid;
        }
    }
}
