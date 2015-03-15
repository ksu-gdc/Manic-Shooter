using System.Collections.Generic;

namespace EntityComponentSystem.Structure
{
    public static class IDManager
    {
        /// <summary>
        /// Number of ID's already taken for tracking the leading edge of new ID's
        /// </summary>
        private static uint IDCount = 0;

        /// <summary>
        /// When entities are freed up they are added to this queue to be used.
        /// This way we recycle the ID's that we use. If we end up overflowing IDCount
        /// then that's a sign that objects are not being managed (or at least removed)
        /// properly
        /// </summary>
        private static Queue<uint> AvailableIDs = new Queue<uint>();

        /// <summary>
        /// Retrieve a free ID to use
        /// </summary>
        /// <returns>ID to be used</returns>
        public static uint GetNewID()
        {
            if (AvailableIDs.Count > 0)
            {
                return AvailableIDs.Dequeue();
            }

            return IDCount++;
        }

        /// <summary>
        /// Frees up an ID to be used by another entity
        /// </summary>
        /// <param name="ID">ID to destroy</param>
        public static void DestroyID(uint ID)
        {
            //TODO: invoke the ComponentManagementSystem to cut all loose ends on the entity
            AvailableIDs.Enqueue(ID);
        }

    }
}