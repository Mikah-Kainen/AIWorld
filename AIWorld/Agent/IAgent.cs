using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIWorld.Environment;

namespace AIWorld.Agent
{
    public interface IAgent<StateData, TransitionData>
        where StateData : IComparable<StateData>
        where TransitionData : IComparable<TransitionData>
    {
        IStateMarker<StateData> GetState();
        IEnvironment<StateData, TransitionData> GetEnvironment();
        IStateTransition<StateData, TransitionData> Selector(HashSet<IStateTransition<StateData, TransitionData>> choices);
    }
}
