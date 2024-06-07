using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIWorld.Environment;

namespace AIWorld.Agent
{
    public interface IAgent<StateData, TransitionData, AgentStateData>
        where StateData : IComparable<StateData>
        where TransitionData : IComparable<TransitionData>
        where AgentStateData : IComparable<AgentStateData>
    {
        AgentState<StateData, AgentStateData> GetCurrentState();
        IStateProviderBasic<StateData, TransitionData> GetEnvironment();
        AgentState<StateData, AgentStateData> Selector(HashSet<IStateTransition<StateData, TransitionData>> choices);
    }
}
