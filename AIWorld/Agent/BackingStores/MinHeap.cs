using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Agent.BackingStores
{
    public class MinHeap<StateData, TransitionData, AgentStateData> : IBackingStore<StateData, TransitionData, AgentStateData>
        where StateData : IComparable<StateData>
        where TransitionData : IComparable<TransitionData>
        where AgentStateData : IComparable<AgentStateData>
    {
        private PriorityQueue<AgentState<StateData, TransitionData, AgentStateData>, AgentStateData> backingQueue;

        public MinHeap()
        {
            backingQueue = new PriorityQueue<AgentState<StateData, TransitionData, AgentStateData>, AgentStateData>();
        }
        public void Add(AgentState<StateData, TransitionData, AgentStateData> state)
        {
            backingQueue.Enqueue(state, state.GetAgentStateData());
        }

        public AgentState<StateData, TransitionData, AgentStateData> GetNext()
        {
            return backingQueue.Dequeue();
        }
    }
}
