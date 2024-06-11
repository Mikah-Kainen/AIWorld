using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIWorld.Environment;

namespace AIWorld.Agent
{
    public interface IBackingStore<TStateData, TTransitionData, TAgentStateData>
        where TStateData : IComparable<TStateData>
        where TTransitionData : IComparable<TTransitionData>
        where TAgentStateData : IComparable<TAgentStateData>
    {
        public void Add(AgentState<TStateData, TTransitionData, TAgentStateData> state);
        public AgentState<TStateData, TTransitionData, TAgentStateData> GetNext(); //Removes and Returns next value from BackingStore
    }

    public abstract class Agent<TStateData, TTransitionData, TAgentStateData> : IAgent<TStateData, TTransitionData>
        where TStateData : IComparable<TStateData>
        where TTransitionData : IComparable<TTransitionData>
        where TAgentStateData : IComparable<TAgentStateData>
    {
        protected AgentState<TStateData, TTransitionData, TAgentStateData> currentState;
        protected Dictionary<IStateMarker<TStateData>, TAgentStateData> bestSeen; //Cost kept with AgentStateData. Agents try to minimize. Might change in future to remember multiple AgentStateDatas if it becomes hard to directly compare two different AgentStateDatas
        protected IBackingStore<TStateData, TTransitionData, TAgentStateData> frontier;
        protected IEnvironment<TStateData, TTransitionData> environment;

        private bool ExpandFrontier(AgentState<TStateData, TTransitionData, TAgentStateData> newState)
        {
            if (bestSeen.ContainsKey(newState.GetState())) //TODO: Change to account for non-certain state transition results
            {
                if (bestSeen[newState.GetState()].CompareTo(newState.GetAgentStateData()) > 0) //TODO: Add option for maximizing agents; Right now all agents minimize
                {
                    frontier.Add(newState);
                    bestSeen[newState.GetState()] = newState.GetAgentStateData();
                    return true;
                }
            }
            else
            {
                frontier.Add(newState);
                bestSeen.Add(newState.GetState(), newState.GetAgentStateData());
                return true;
            }
            return false;
        }
        public Agent(IStateMarker<TStateData> startingState, IEnvironment<TStateData, TTransitionData> environment, IBackingStore<TStateData, TTransitionData, TAgentStateData> backingStore)
        {
            this.currentState = GetStartingState(startingState);
            bestSeen = new Dictionary<IStateMarker<TStateData>, TAgentStateData>();
            this.frontier = backingStore;
            this.environment = environment;
        }
        public IStateMarker<TStateData> GetState() { return currentState.GetState(); }
        protected AgentState<TStateData, TTransitionData, TAgentStateData> GetAgentState() { return currentState; }
        public IEnvironment<TStateData, TTransitionData> GetEnvironment() { return environment; }

        public IStateTransition<TStateData, TTransitionData> Selector(HashSet<IStateTransition<TStateData, TTransitionData>> choices)
        {
            foreach (IStateTransition<TStateData, TTransitionData> transition in choices)
            {
                AgentState<TStateData, TTransitionData, TAgentStateData> newState = GenerateState(currentState, transition);
                ExpandFrontier(newState);
            }

            currentState = frontier.GetNext();
            return currentState.GetLastMove();
        }

        protected abstract AgentState<TStateData, TTransitionData, TAgentStateData> GenerateState(AgentState<TStateData, TTransitionData, TAgentStateData> previousState, IStateTransition<TStateData, TTransitionData> transition);
        protected abstract AgentState<TStateData, TTransitionData, TAgentStateData> GetStartingState(IStateMarker<TStateData> startingState);
    }
}
