using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIWorld.Environment;

namespace AIWorld.Agent
{
    public interface IBackingStore<StateData, TransitionData, AgentStateData>
        where StateData : IComparable<StateData>
        where TransitionData : IComparable<TransitionData>
        where AgentStateData : IComparable<AgentStateData>
    {
        public void Add(AgentState<StateData, TransitionData, AgentStateData> state);
        public AgentState<StateData, TransitionData, AgentStateData> GetNext(); //Removes and Returns next value from BackingStore
    }

    public abstract class Agent<StateData, TransitionData, AgentStateData> : IAgent<StateData, TransitionData>
        where StateData : IComparable<StateData>
        where TransitionData : IComparable<TransitionData>
        where AgentStateData : IComparable<AgentStateData>
    {
        protected AgentState<StateData, TransitionData, AgentStateData> currentState;
        protected Dictionary<IStateMarker<StateData>, AgentStateData> bestSeen; //Cost kept with AgentStateData. Agents try to minimize. Might change in future to remember multiple AgentStateDatas if it becomes hard to directly compare two different AgentStateDatas
        protected IBackingStore<StateData, TransitionData, AgentStateData> frontier;
        protected IEnvironment<StateData, TransitionData> environment;

        private bool ExpandFrontier(AgentState<StateData, TransitionData, AgentStateData> newState)
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
        public Agent(IStateMarker<StateData> startingState, IEnvironment<StateData, TransitionData> environment, IBackingStore<StateData, TransitionData, AgentStateData> backingStore)
        {
            this.currentState = GetStartingState(startingState);
            bestSeen = new Dictionary<IStateMarker<StateData>, AgentStateData>();
            this.frontier = backingStore;
            this.environment = environment;
        }
        public IStateMarker<StateData> GetState() { return currentState.GetState(); }
        protected AgentState<StateData, TransitionData, AgentStateData> GetAgentState() { return currentState; }
        public IEnvironment<StateData, TransitionData> GetEnvironment() { return environment; }

        public IStateTransition<StateData, TransitionData> Selector(HashSet<IStateTransition<StateData, TransitionData>> choices)
        {
            List<AgentState<StateData, TransitionData, AgentStateData>> newAgentStates = new();
            foreach (IStateTransition<StateData, TransitionData> transition in choices)
            {
                newAgentStates.Add(GenerateState(currentState, transition));
            }

            foreach(AgentState<StateData, TransitionData, AgentStateData> newState in newAgentStates)
            {
                if (bestSeen.ContainsKey(newState.GetState())) //TODO: Change to account for non-certain state transition results
                {
                    if (bestSeen[newState.GetState()].CompareTo(newState.GetAgentStateData()) > 0) //TODO: Add option for maximizing agents; Right now all agents minimize
                    {
                        frontier.Add(newState);
                    }
                }
                else
                {
                    frontier.Add(newState);
                }
            }
            currentState = frontier.GetNext();
            bestSeen.Add(currentState.GetPreviousState().GetState(), currentState.GetPreviousState().GetAgentStateData());
            return currentState.GetLastMove();
        }

        protected abstract AgentState<StateData, TransitionData, AgentStateData> GenerateState(AgentState<StateData, TransitionData, AgentStateData> previousState, IStateTransition<StateData, TransitionData> transition);
        protected abstract AgentState<StateData, TransitionData, AgentStateData> GetStartingState(IStateMarker<StateData> startingState);
    }
}
