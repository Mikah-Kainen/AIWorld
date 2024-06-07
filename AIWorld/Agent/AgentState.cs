using AIWorld.Environment;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Agent
{
    public class AgentState<StateData, AgentStateData>
        where AgentStateData : IComparable<AgentStateData>
    {
        private AgentState<StateData, AgentStateData> previousState;
        private AgentStateData agentStateData;
        private IStateMarker<StateData> currentState;
        public AgentState(AgentState<StateData, AgentStateData> previousState, AgentStateData agentStateData, IStateMarker<StateData> currentState) 
        {
            this.previousState = previousState;
            this.agentStateData = agentStateData;
            this.currentState = currentState;
        }

        public AgentState<StateData, AgentStateData> GetPreviousState() { return previousState; }
        public AgentStateData GetAgentStateData() { return agentStateData; }
        public IStateMarker<StateData> GetCurrentState() {  return currentState; }
    }
}
