﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIWorld.Environment;

namespace AIWorld.Agent
{
    public interface IBackingStore<TStateData, TTransitionData, TAgentStateData>
        where TAgentStateData : IComparable<TAgentStateData>
    {
        public void Add(AgentState<TStateData, TTransitionData, TAgentStateData> state);
        public AgentState<TStateData, TTransitionData, TAgentStateData> GetNext(); //Removes and Returns next value from BackingStore
    }

    public abstract class Agent<TStateMarker, TStateData, TStateTransition, TTransitionData, TAgentStateData> : IAgent<TStateData, TTransitionData>
        where TStateMarker : IStateMarker<TStateData>
        where TStateTransition : IStateTransition<TStateData, TTransitionData>
        where TAgentStateData : IComparable<TAgentStateData>
    {
        public String Name;
        protected AgentState<TStateData, TTransitionData, TAgentStateData> currentState;
        protected Dictionary<IStateMarker<TStateData>, TAgentStateData> bestSeen; //Cost kept with AgentStateData. Agents try to minimize. Might change in future to remember multiple AgentStateDatas if it becomes hard to directly compare two different AgentStateDatas
        protected IBackingStore<TStateData, TTransitionData, TAgentStateData> frontier;

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
        public Agent(String name, IStateMarker<TStateData> startingState, IBackingStore<TStateData, TTransitionData, TAgentStateData> backingStore)
        {
            this.Name = name;
            this.currentState = new AgentState<TStateData, TTransitionData, TAgentStateData>(GetStartingStateData(startingState), startingState);
            bestSeen = new Dictionary<IStateMarker<TStateData>, TAgentStateData>();
            this.frontier = backingStore;
        }
        public TStateMarker GetState() { return currentState.GetState(); }
        protected AgentState<TStateData, TTransitionData, TAgentStateData> GetAgentState() { return currentState; }
        public IStateTransition<TStateData, TTransitionData> SelectMove(HashSet<IStateTransition<TStateData, TTransitionData>> choices)
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
        protected abstract TAgentStateData GetStartingStateData(IStateMarker<TStateData> startingState);

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
            Console.Write(" ");
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

        public List<IStateMarker<TStateData>> GetCurrentPath()
        {
            List<AgentState<TStateData, TTransitionData, TAgentStateData>> path = currentState.GetPath();
            List<IStateMarker<TStateData>> returnList = new();
            foreach(var state in path)
            {
                returnList.Add(state.GetState());
            }
            return returnList;
        }
    }
}
