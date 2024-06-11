using AIWorld.Environment;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Agent
{
    public class AgentState<StateData, TransitionData, AgentStateData>
        where StateData : IComparable<StateData>
        where TransitionData : IComparable<TransitionData>
        where AgentStateData : IComparable<AgentStateData>
    {
        private AgentState<StateData, TransitionData, AgentStateData> previousState;
        private AgentStateData agentStateData;
        private IStateTransition<StateData, TransitionData> lastMove;
        private IStateMarker<StateData> state;
        public AgentState(AgentState<StateData, TransitionData, AgentStateData> previousState, AgentStateData agentStateData, IStateTransition<StateData, TransitionData> lastMove) 
        {
            this.previousState = previousState;
            this.agentStateData = agentStateData;
            this.lastMove = lastMove;
            this.state = lastMove.GetEndState(); //TODO: Add uncertainty to the game. Agents' states can only be known in uncertain games after a transition is picked and the result is returned from the environment
        }

        public AgentState<StateData, TransitionData, AgentStateData> GetPreviousState() { return previousState; }
        public AgentStateData GetAgentStateData() { return agentStateData; }
        public IStateTransition<StateData, TransitionData> GetLastMove() {  return lastMove; }
        public IStateMarker<StateData> GetState() { return state; }
    }
}
