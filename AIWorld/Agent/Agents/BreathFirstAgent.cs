using AIWorld.Agent.BackingStores;
using AIWorld.Agent.EnvironmentTokens;
using AIWorld.Environment;
using AIWorld.Environment.Environments;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Agent.Agents
{
    public class BreathFirstAgent<TStateData, TTransitionData> : Agent<TStateData, TTransitionData, int>
        where TStateData : IComparable<TStateData>
        where TTransitionData : IComparable<TTransitionData>
    {
        public BreathFirstAgent()
            : base("Breath First Agent", new MinHeap<TStateData, TTransitionData, int>()) {}

        protected override AgentState<TStateData, TTransitionData, int> GenerateState(AgentState<TStateData, TTransitionData, int> previousState, TransitionToken<TStateData, TTransitionData> transition)
        {
            return new AgentState<TStateData, TTransitionData, int>(previousState, previousState.GetAgentStateData() + 1, transition, transition.GetEndState());
        }

        protected override int GetStartingStateData(StateToken<TStateData> startingState)
        {
            return 0;
        }
    }
}
