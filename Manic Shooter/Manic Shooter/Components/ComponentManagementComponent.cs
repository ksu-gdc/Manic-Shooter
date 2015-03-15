using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityComponentSystem.Interfaces;

namespace EntityComponentSystem.Components
{
    /// <summary>
    /// Maps an entity to all of the various components it subscribes too.
    /// Makes adjusting all components of an entity at once (such as removing
    /// an entity entirely) much easier.
    /// </summary>
    public struct MetaComponent
    {
        public uint EntityID;
        public List<IGameComponent> Subscriptions;
    }

    /// <summary>
    /// The component structure that keeps track of all MetaComponents
    /// </summary>
    public class ComponentManagementComponent : GameComponent<MetaComponent>
    {
    }
}
