using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Agent.BackingStores
{
    public class MinHeap<TStateData, TTransitionData, TAgentStateData> : IBackingStore<TStateData, TTransitionData, TAgentStateData>
        where TStateData : IComparable<TStateData>
        where TTransitionData : IComparable<TTransitionData>
        where TAgentStateData : IComparable<TAgentStateData>
    {
        private PriorityQueue<AgentState<TStateData, TTransitionData, TAgentStateData>, TAgentStateData> backingQueue;

        public MinHeap()
        {
            backingQueue = new PriorityQueue<AgentState<TStateData, TTransitionData, TAgentStateData>, TAgentStateData>();
        }
        public void Add(AgentState<TStateData, TTransitionData, TAgentStateData> state)
        {
            backingQueue.Enqueue(state, state.GetAgentStateData());
        }

        public AgentState<TStateData, TTransitionData, TAgentStateData> GetNext()
        {
            return backingQueue.Dequeue();
        }
    }
}
