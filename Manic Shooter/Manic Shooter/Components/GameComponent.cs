using System.Collections;
using System.Collections.Generic;
using Component_Based_Base_Game;
using EntityComponentSystem.Interfaces;
using EntityComponentSystem.Structure;

namespace EntityComponentSystem.Components
{
    /// <summary>
    /// Defines a single game component to be used as a base 
    /// for other components
    /// </summary>
    /// <typeparam name="T">The struct that defines the properties that a subscribed entity should have</typeparam>
    public class GameComponent<T> : IGameComponent
    {
        /// <summary>
        /// AVL tree structure that stores the subscribed entities
        /// </summary>
        protected AVLTree<T> elements;

        /// <summary>
        /// Base Constructor for a new component
        /// </summary>
        public GameComponent()
        {
            elements = new AVLTree<T>();
        }

        /// <summary>
        /// Allows a concise way of accessing properties indexed by their entities' ID
        /// </summary>
        /// <param name="entityID">The ID of the owner</param>
        /// <returns>The property struct stored with the entity ID</returns>
        public T this[uint entityID]
        {
            set { elements.Find(entityID).dataValue = value; }
            get { return elements.Find(entityID).dataValue; }
        }

        /// <summary>
        /// Returns an enumerable set of all structs that the component stores
        /// (useful for processing a large number of subscribers)
        /// </summary>
        public IEnumerable<T> All
        {
            get { return elements.ToValueArray(); }
        }

        /// <summary>
        /// Returns an enumerable set of all entity ID's in the component
        /// (useful for seeing who is subscribed to a component)
        /// </summary>
        public IEnumerable<uint> Keys
        {
            get { return elements.ToKeyArray(); }
        }

        /// <summary>
        /// Adds an entity with the accompanied struct to the component
        /// </summary>
        /// <param name="elementID">ID of the entity subscribing to the component</param>
        /// <param name="component">Struct of properties to be kept for the element</param>
        public virtual void Add(uint elementID, T component)
        {
            elements.Insert(elementID, component);
        }

        /// <summary>
        /// Removes a subscribed entity from the component
        /// </summary>
        /// <param name="elementID">ID of the entitiy to remove</param>
        public virtual void Remove(uint elementID)
        {
            if (elements.Contains(elementID))
            {
                elements.Delete(elementID);
            }
        }

        /// <summary>
        /// Check if an entity is subscribed to the component
        /// </summary>
        /// <param name="elementID">ID of entity to check subscription of</param>
        /// <returns></returns>
        public bool Contains(uint elementID)
        {
            return elements.Contains(elementID);
        }

        /// <summary>
        /// Check the number of subscribers to the component
        /// </summary>
        /// <returns>The running count of subscribers</returns>
        public int Count()
        {
            return elements.Count;
        }

        /// <summary>
        /// Get a generic list of structs of each subscriber
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            return elements.ToValueArray();
        }

        //Not entirely sure what this was for, but if I had to guess it was
        //for some sort of message passing or event handling between components/systems
        //It was legacy code from the system I copied and was never implemented.
        /*
        public virtual void HandleTrigger(uint elementID, string type)
        {
		
        }
         * */
    }
}