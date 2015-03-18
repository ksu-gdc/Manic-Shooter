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
        public const string _COMPONENTMANAGEMENTCOMPONENT_ = "ComponentManagementComponent";

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
            this.AddComponent(typeof(ComponentManagementComponent));
        }

        public void AddComponent(Type T) 
        {
            if (_gameComponents.ContainsKey(T.ToString())) throw new Exception("The given key '" + T.ToString() + "' already exists!");
            _gameComponents.Add(T.ToString(), (IGameComponent)Activator.CreateInstance(T));
        }

        public bool ContainsComponent(Type T)
        {
            return _gameComponents.ContainsKey(T.ToString());
        }

        public T GetComponent<T>() where T : class, IGameComponent
        {
            if (!_gameComponents.ContainsKey(typeof(T).ToString())) throw new KeyNotFoundException("Key:\'" + typeof(T).ToString() + "\' must first be added before it can be retrieved");
            //T component = (T)Convert.ChangeType(gameComponents[name], typeof(T));
            T component = _gameComponents[typeof(T).ToString()] as T;

            if (component == null) throw new TypeAccessException("The given generic type '" + typeof(T).ToString() + "' cannot be successfully cast");

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