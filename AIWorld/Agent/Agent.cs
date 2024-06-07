using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIWorld.Environment;

namespace AIWorld.Agent
{
    public interface IBackingStore<StateData, AgentStateData>
        where StateData : IComparable<StateData>
        where AgentStateData : IComparable<AgentStateData>
    {
        public void Add(AgentState<StateData, AgentStateData> state);
        public AgentState<StateData, AgentStateData> GetNext(); //Removes next value from BackingStore
    }

    public abstract class Agent<StateData, TransitionData, AgentStateData> : IAgent<StateData, TransitionData, AgentStateData>
        where StateData : IComparable<StateData>
        where TransitionData : IComparable<TransitionData>
        where AgentStateData : IComparable<AgentStateData>
    {
        protected AgentState<StateData, AgentStateData> currentState;
        protected Dictionary<IStateMarker<StateData>, int> closedSet; //Cost kept with int. Agents try to minimize. Might change in future
        protected IBackingStore<StateData, AgentStateData> frontier;
        protected IStateProviderBasic<StateData, TransitionData> environment;
        public Agent(IStateMarker<StateData> startingState, IStateProviderBasic<StateData, TransitionData> environment, IBackingStore<StateData, AgentStateData> backingStore)
        {
            this.currentState = GetStartingState(startingState);
            closedSet = new Dictionary<IStateMarker<StateData>, int>();
            this.frontier = backingStore;
            this.environment = environment;
        }
        public AgentState<StateData, AgentStateData> GetCurrentState() { return currentState; }

        public IStateProviderBasic<StateData, TransitionData> GetEnvironment() { return environment; }

        public AgentState<StateData, AgentStateData> Selector(HashSet<IStateTransition<StateData, TransitionData>> choices)
        {
            List<AgentState<StateData, AgentStateData>> newAgentStates = new();
            foreach (IStateTransition<StateData, TransitionData> transition in choices)
            {
                newAgentStates.Add(GenerateState(currentState, transition));
            }

            foreach(AgentState<StateData, AgentStateData> newState in newAgentStates)
            {
                if (closedSet.ContainsKey(newState.GetCurrentState()))
                {
                    if (closedSet[newState.GetCurrentState()].CompareTo(newState.GetAgentStateData()) > 0)
                    {
                        frontier.Add(newState);
                    }
                }
                else
                {
                    frontier.Add(newState);
                }
            }
            return frontier.GetNext();
        }

        protected abstract AgentState<StateData, AgentStateData> GenerateState(AgentState<StateData, AgentStateData> previousState, IStateTransition<StateData, TransitionData> transition);
        protected abstract AgentState<StateData, AgentStateData> GetStartingState(IStateMarker<StateData> startingState);
    }
}
