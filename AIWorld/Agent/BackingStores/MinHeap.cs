using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Agent.BackingStores
{
    public class MinHeap<StateData, AgentStateData> : IBackingStore<StateData, AgentStateData>
        where StateData : IComparable<StateData>
        where AgentStateData : IComparable<AgentStateData>
    {
        private PriorityQueue<AgentState<StateData, AgentStateData>, AgentStateData> backingQueue;

        public MinHeap()
        {
            backingQueue = new PriorityQueue<AgentState<StateData, AgentStateData>, AgentStateData>();
        }
        public void Add(AgentState<StateData, AgentStateData> state)
        {
            backingQueue.Enqueue(state, state.GetAgentStateData());
        }

        public AgentState<StateData, AgentStateData> GetNext()
        {
            return backingQueue.Dequeue();
        }
    }
}
