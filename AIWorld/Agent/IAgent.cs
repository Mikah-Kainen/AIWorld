using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIWorld.Environment;

namespace AIWorld.Agent
{
    public interface IAgent<TStateData, TTransitionData>
        where TStateData : IComparable<TStateData>
        where TTransitionData : IComparable<TTransitionData>
    {
        IStateMarker<TStateData> GetState();
        IEnvironment<TStateData, TTransitionData> GetEnvironment();
        IStateTransition<TStateData, TTransitionData> Selector(HashSet<IStateTransition<TStateData, TTransitionData>> choices);
    }
}
