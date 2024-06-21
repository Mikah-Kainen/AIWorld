using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIWorld.Agent.EnvironmentTokens;
using AIWorld.Environment;

namespace AIWorld.Agent
{
    public interface IBackingStore<TStateData, TTransitionData, TAgentStateData>
        where TAgentStateData : IComparable<TAgentStateData>
    {
        public HashSet<AgentState<TStateData, TTransitionData, TAgentStateData>> GetFrontierStates(); //Debug Set
        public void Add(AgentState<TStateData, TTransitionData, TAgentStateData> state);
        public AgentState<TStateData, TTransitionData, TAgentStateData> GetNext(); //Removes and Returns next value from BackingStore
    }

    public abstract class Agent<TStateData, TTransitionData, TAgentStateData> : IAgent<TStateData, TTransitionData>
        where TAgentStateData : IComparable<TAgentStateData>
    {
        public String Name;
        protected AgentState<TStateData, TTransitionData, TAgentStateData> currentState;
        protected Dictionary<StateToken<TStateData>, TAgentStateData> bestSeen; //Cost kept with AgentStateData. Agents try to minimize. Might change in future to remember multiple AgentStateDatas if it becomes hard to directly compare two different AgentStateDatas
        protected IBackingStore<TStateData, TTransitionData, TAgentStateData> frontier;

        /// <summary>
        /// Creates an Agent that can operate in an environment. All Agents try to minimize AgentStateData scores
        /// </summary>
        /// <param name="name"></param>
        /// <param name="backingStore"></param>
        public Agent(String name, IBackingStore<TStateData, TTransitionData, TAgentStateData> backingStore)
        {
            this.Name = name;
            this.currentState = null;
            bestSeen = new Dictionary<StateToken<TStateData>, TAgentStateData>();
            this.frontier = backingStore;
        }
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

        public void SetStartState(StateToken<TStateData> startingState)
        {
            if (currentState != null)
            {
                throw new Exception("Starting State Allocation: Starting State Already Set");
            }
            currentState = new AgentState<TStateData, TTransitionData, TAgentStateData>(GetStartingStateData(startingState), startingState);
        }
        public StateToken<TStateData> GetState() 
        { 
            if (currentState == null)
            {
                throw new Exception("State Access: Starting State Not Set");
            }
            return currentState.GetState(); 
        }
        protected AgentState<TStateData, TTransitionData, TAgentStateData> GetAgentState() 
        { 
            if (currentState == null)
            {
                throw new Exception("Agent State Access: Starting State Not Set");
            }
            return currentState; 
        }
        public TransitionToken<TStateData, TTransitionData> SelectMove(HashSet<TransitionToken<TStateData, TTransitionData>> choices)
        {
            foreach (TransitionToken<TStateData, TTransitionData> transition in choices)
            {
                AgentState<TStateData, TTransitionData, TAgentStateData> newState = GenerateState(currentState, transition);
                ExpandFrontier(newState);
            }

            currentState = frontier.GetNext();
            return currentState.GetLastMove();
        }

        protected abstract AgentState<TStateData, TTransitionData, TAgentStateData> GenerateState(AgentState<TStateData, TTransitionData, TAgentStateData> previousState, TransitionToken<TStateData, TTransitionData> transition);
        protected abstract TAgentStateData GetStartingStateData(StateToken<TStateData> startingState);

        private const int minDisplayLength = 3;
        protected virtual void Display(TStateData stateData) { SpacedDisplay(stateData.ToString(), minDisplayLength); }
        protected virtual void Display(TTransitionData transitionData) { SpacedDisplay(transitionData.ToString(), minDisplayLength); }
        protected virtual void Display(TAgentStateData agentStateData) { SpacedDisplay(agentStateData.ToString(), minDisplayLength); }
        private void SpacedDisplay(String message, int minLength)
        {
            int extra = minLength - message.Length;
            for (int i = 0; i < extra; i++)
            {
                Console.Write(" ");
            }
            Console.Write(message);
        }
        public void Display()
        {
            if (currentState == null)
            {
                throw new Exception("Invalid Display: Starting State Not Set");
            }

            Console.WriteLine($"{Name}: ");
            List<AgentState<TStateData, TTransitionData, TAgentStateData>> path = currentState.GetPath();
            
            Console.Write("     StateData:      ");
            Display(path[0].GetState().GetData());
            for (int i = 1; i < path.Count; i ++)
            {
                Console.Write(", ");
                Display(path[i].GetState().GetData());
            }
            Console.WriteLine();

            Console.Write("     TransitionData: ");
            Console.Write("   ");
            for (int i = 1; i < path.Count; i++)
            {
                Console.Write(", ");
                Display(path[i].GetLastMove().GetData());
            }
            Console.WriteLine();

            Console.Write("     AgentStateData: ");
            Display(path[0].GetAgentStateData());
            for (int i = 1; i < path.Count; i++)
            {
                Console.Write(", ");
                Display(path[i].GetAgentStateData());
            }

        }
        public List<StateToken<TStateData>> GetCurrentPath()
        {
            if (currentState == null)
            {
                throw new Exception("Invalid Path Calculation: Starting State Not Set");
            }
            List<AgentState<TStateData, TTransitionData, TAgentStateData>> path = currentState.GetPath();
            List<StateToken<TStateData>> returnList = new();
            foreach(var state in path)
            {
                returnList.Add(state.GetState());
            }
            return returnList;
        }
    }
}
