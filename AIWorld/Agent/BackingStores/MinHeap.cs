using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Agent.BackingStores
{
    public class MinHeap<TStateData, TTransitionData, TAgentStateData> : IBackingStore<TStateData, TTransitionData, TAgentStateData>
        where TAgentStateData : IComparable<TAgentStateData>
    {
        private PriorityQueue<AgentState<TStateData, TTransitionData, TAgentStateData>, TAgentStateData> backingQueue;
        private HashSet<AgentState<TStateData, TTransitionData, TAgentStateData>> agentStates; //Debug Set

        HashSet<AgentState<TStateData, TTransitionData, TAgentStateData>> IBackingStore<TStateData, TTransitionData, TAgentStateData>.GetFrontierStates() => agentStates;

        public MinHeap()
        {
            backingQueue = new PriorityQueue<AgentState<TStateData, TTransitionData, TAgentStateData>, TAgentStateData>();
            agentStates = new HashSet<AgentState<TStateData, TTransitionData, TAgentStateData>>();
        }
        public void Add(AgentState<TStateData, TTransitionData, TAgentStateData> state)
        {
            backingQueue.Enqueue(state, state.GetAgentStateData());
            agentStates.Add(state);
        }

        public AgentState<TStateData, TTransitionData, TAgentStateData> GetNext()
        {
            agentStates.Remove(backingQueue.Peek());
            return backingQueue.Dequeue();
        }
    }
}
