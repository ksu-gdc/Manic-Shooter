using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityComponentSystem.Interfaces
{
    /// <summary>
    /// Allows for meta management of components. See ComponentManagementSystem.cs
    /// </summary>
    public interface IGameComponent
    {
        void Remove(uint elementID);

        bool Contains(uint elementID);

        int Count();
    }
}
