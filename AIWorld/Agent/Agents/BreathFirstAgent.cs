using AIWorld.Agent.BackingStores;
using AIWorld.Environment;
using AIWorld.Environment.Environments;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Agent.Agents
{
    //public class BreathFirstAgent : Agent<int, int, int>
    //{
    //    private int currentLayer;
    //    public BreathFirstAgent(IStateMarker<int> startingState, IStateProviderBasic<int, int> environment)
    //        : base(startingState, environment, new MinHeap<int, int>())
    //    {
    //        currentLayer = 0;
    //    }

    //    public override IStateTransition<int, int> Selector(HashSet<IStateTransition<int, int>> choices)
    //    {
    //        List<AgentState<int, int>> 
    //        foreach (IStateTransition<int, int> transition in choices)
    //        {
    //            IStateMarker<int> endState = transition.GetEndState();
    //            if (!closedSet.ContainsKey(endState))
    //            {
    //                frontier.Add(transition, currentLayer);
    //            }
    //        }
    //    }
    //}
}
