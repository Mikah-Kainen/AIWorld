using AIWorld.Environment;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Agent
{
    public class AgentState<TStateData, TTransitionData, TAgentStateData>
        where TStateData : IComparable<TStateData>
        where TTransitionData : IComparable<TTransitionData>
        where TAgentStateData : IComparable<TAgentStateData>
    {
        private AgentState<TStateData, TTransitionData, TAgentStateData> previousState;
        private TAgentStateData agentStateData;
        private IStateTransition<TStateData, TTransitionData> lastMove;
        private IStateMarker<TStateData> state;
        public AgentState(AgentState<TStateData, TTransitionData, TAgentStateData> previousState, TAgentStateData agentStateData, IStateTransition<TStateData, TTransitionData> lastMove) 
        {
            this.previousState = previousState;
            this.agentStateData = agentStateData;
            this.lastMove = lastMove;
            this.state = lastMove.GetEndState(); //TODO: Add uncertainty to the game. Agents' states can only be known in uncertain games after a transition is picked and the result is returned from the environment
        }

        public AgentState<TStateData, TTransitionData, TAgentStateData> GetPreviousState() { return previousState; }
        public TAgentStateData GetAgentStateData() { return agentStateData; }
        public IStateTransition<TStateData, TTransitionData> GetLastMove() {  return lastMove; }
        public IStateMarker<TStateData> GetState() { return state; }
    }
}
