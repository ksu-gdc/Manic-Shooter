using System.Collections;
using System.Collections.Generic;

namespace EntityComponentSystem.Structure
{
    public class AVLTree<T>
    {
        /// <summary>
        /// The total number of nodes.
        /// </summary>
        public int Count;

        /// <summary>
        /// The root node of the tree.
        /// </summary>
        public AVLTreeNode<uint, T> Root;

        /// <summary>
        /// Initializes a new instance of the <see cref="AVLTree`1"/> class with
        /// an empty root node.
        /// </summary>
        public AVLTree()
        {
            Count = 0;
            Root = null;
        }

        /// <summary>
        /// Finds an entity with the given ID if it is in the tree
        /// </summary>
        /// <param name="elementID">ID of entity to retrieve</param>
        /// <returns>Node of the entity; null if entity is not in the tree</returns>
        public AVLTreeNode<uint, T> Find(uint elementID)
        {
            if (Root != null)
            {
                return Root.Find(elementID, 0);
            }

            return null;
        }

        /// <summary>
        /// Checks if the entity is contained in the tree
        /// </summary>
        /// <param name="elementID">ID of the entity to check</param>
        /// <returns>Whether the entity is contained in the tree or not</returns>
        public bool Contains(uint elementID)
        {
            return (this.Find(elementID) != null);
        }

        /// <summary>
        /// Finds and removes an entity from the tree
        /// </summary>
        /// <param name="nodeToRemove">The node to prune from the tree</param>
        /// <returns>Success of the operation</returns>
        public bool Delete(AVLTreeNode<uint, T> nodeToRemove)
        {
            if (Root != null)
            {
                if (nodeToRemove == Root)
                {
                    Root = Root.DeleteRoot(Root);
                    return true;
                }
                else
                {
                    return Root.Delete(nodeToRemove);
                }
            }

            return false;
        }

        /// <summary>
        /// Finds and removes an entity from the tree
        /// </summary>
        /// <param name="nodeValueToRemove">The ID of the entity to remove</param>
        /// <returns>Success of the operation</returns>
        public bool Delete(uint nodeValueToRemove)
        {
            if (Root != null)
            {
                AVLTreeNode<uint, T> nodeToRemove = Root.Find(nodeValueToRemove, 0);

                if (nodeToRemove != null)
                {
                    bool result = this.Delete(nodeToRemove);

                    if (result)
                    {
                        Count--;
                    }

                    return result;
                }
            }

            return false;
        }

        /// <summary>
        /// Inserts a new entity into the tree
        /// </summary>
        /// <param name="nodeKeyToAdd">ID of the entity to add</param>
        /// <param name="nodeValueToAdd">The struct that defines the component properties of the entity</param>
        public void Insert(uint nodeKeyToAdd, T nodeValueToAdd)
        {
            if (Root == null)
            {
                Root = new AVLTreeNode<uint, T>(nodeKeyToAdd, nodeValueToAdd, null, null, null);
                Root.height = 0;
                Count++;
                return;
            }

            AVLTreeNode<uint, T> newNode = new AVLTreeNode<uint, T>(nodeKeyToAdd, nodeValueToAdd, null, null, null);

            AVLTreeNode<uint, T>.Insert(Root, newNode);
            Count++;
            Root = AVLTreeNode<uint, T>.FindRoot(Root);
        }

        /// <summary>
        /// Converts the tree contents to a list of nodes
        /// </summary>
        /// <returns>returns a list of tree nodes</returns>
        public List<AVLTreeNode<uint, T>> ToArray()
        {
            if (Root == null)
            {
                return new List<AVLTreeNode<uint, T>>();
            }
            else
            {
                return Root.ToArray();
            }
        }

        /// <summary>
        /// Converts the tree contents to a list of struct values
        /// </summary>
        /// <returns>List of structs that represent the component properties of each entity</returns>
        public List<T> ToValueArray()
        {
            List<T> valueArray = new List<T>();

            foreach (AVLTreeNode<uint, T> node in this.ToArray())
            {
                valueArray.Add(node.dataValue);
            }

            return valueArray;
        }

        /// <summary>
        /// Converts the tree contents to a list of entity ID's
        /// </summary>
        /// <returns>List of ID's of all entities subscribed to the component</returns>
        public List<uint> ToKeyArray()
        {
            List<uint> keyArray = new List<uint>();

            if (Root == null) return keyArray;

            foreach (AVLTreeNode<uint, T> node in this.ToArray())
            {
                keyArray.Add(node.getKey());
            }

            return keyArray;
        }

        /// <summary>
        /// Converts the tree to a string representation.
        /// Primarily for debugging purposes
        /// </summary>
        /// <returns>String that represents the tree</returns>
        public override string ToString()
        {
            List<AVLTreeNode<uint, T>> nodeList = ToArray();

            string result = "Root = " + Root.getKey() + "\n";

            foreach (AVLTreeNode<uint, T> node in nodeList)
            {
                result += node.ToString() + "\n";
            }

            return result;
        }
    }
}