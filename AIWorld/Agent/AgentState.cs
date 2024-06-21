using AIWorld.Environment;
using AIWorld.Agent.EnvironmentTokens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Agent
{
    public class AgentState<TStateData, TTransitionData, TAgentStateData>
        where TAgentStateData : IComparable<TAgentStateData>
    {
        private AgentState<TStateData, TTransitionData, TAgentStateData> previousState;
        private TAgentStateData agentStateData;
        private TransitionToken<TStateData, TTransitionData> lastMove;
        private StateToken<TStateData> currentState;
        public AgentState(AgentState<TStateData, TTransitionData, TAgentStateData> previousState, TAgentStateData agentStateData, TransitionToken<TStateData, TTransitionData> lastMove, StateToken<TStateData> currentState) 
        {
            this.previousState = previousState;
            this.agentStateData = agentStateData;
            this.lastMove = lastMove;
            this.currentState = currentState;
        }

        public AgentState(TAgentStateData agentStateData, StateToken<TStateData> currentState) : this(null, agentStateData, new TransitionToken<TStateData, TTransitionData>(default, null, currentState), currentState) { }

        public AgentState<TStateData, TTransitionData, TAgentStateData> GetPreviousState() { return previousState; }
        public TAgentStateData GetAgentStateData() { return agentStateData; }
        public TransitionToken<TStateData, TTransitionData> GetLastMove() {  return lastMove; }
        public StateToken<TStateData> GetState() { return currentState; }

        public List<AgentState<TStateData, TTransitionData, TAgentStateData>> GetPath()
        {
            List<AgentState<TStateData, TTransitionData, TAgentStateData>> returnList = new();
            Stack<AgentState<TStateData, TTransitionData, TAgentStateData>> reversalStack = new();
            AgentState<TStateData, TTransitionData, TAgentStateData> currentState = this;
            while (currentState.previousState != null)
            {
                reversalStack.Push(currentState);
                currentState = currentState.previousState;
            }
            returnList.Add(currentState);
            while(reversalStack.Count > 0)
            {
                returnList.Add(reversalStack.Pop());
            }
            return returnList;
        }
    }
}
