using System.Collections.Generic;
using EntityComponentSystem.Components;
using EntityComponentSystem.Interfaces;
using System;

namespace EntityComponentSystem.Systems
{
    /// <summary>
    /// Singleton Class for managing components and their subscribers. Meant to streamline
    /// certain processes like entity deletion and globally accessing components.
    /// </summary>
    public class ComponentManagementSystem
    {
        /// <summary>
        /// Singleton Instance for global static access to System methods
        /// </summary>
        private static ComponentManagementSystem _instance;

        public static ComponentManagementSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ComponentManagementSystem();
                }
                return _instance;
            }
        }

        private Dictionary<string, IGameComponent> _gameComponents;

        //Running list of Components to be managed
        public RenderComponent RenderComponent;
        public ModelComponent ModelComponent;
        public TextureComponent TextureComponent;
        public SoundComponent SoundComponent;
        public SpriteFontComponent SpriteFontComponent;
        public EffectComponent EffectComponent;

        /// <summary>
        /// Initializes all components, this is also useful for
        /// reseting the game as this is one step in clearing all entities
        /// </summary>
        public ComponentManagementSystem()
        {
            //RenderComponent = new RenderComponent();
            _gameComponents = new Dictionary<string, IGameComponent>();
        }

        public void AddComponent(string name, IGameComponent component)
        {
            if (_gameComponents.ContainsKey(name)) throw new Exception("The given key '" + name + "' already exists!");
            _gameComponents.Add(name, component);
        }

        public bool ContainsComponent(string name)
        {
            return _gameComponents.ContainsKey(name);
        }
        
        public T GetComponent<T>(string name) where T:class, IGameComponent
        {
            if (!_gameComponents.ContainsKey(name)) throw new KeyNotFoundException("Key:\'" + name + "\' must first be added before it can be retrieved");
            //T component = (T)Convert.ChangeType(gameComponents[name], typeof(T));
            T component = _gameComponents[name] as T;

            if(component == null) throw new TypeAccessException("The given generic type '" + typeof(T).ToString() + "' does not match component type of '" + name + "'");

            return component;
        }

        /// <summary>
        /// Removes a single entity from the components that it subscribes to
        /// </summary>
        /// <param name="elementID">ID of the entity to remove</param>
        public void Remove(uint elementID)
        {
            //TODO: Remove element from Components that it is subscribed to
            /*
            PositionComponent.Remove(elementID);
            */
        }
    }
}