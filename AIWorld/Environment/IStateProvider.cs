using AIWorld.Agent;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public abstract class IStateProvider<TStateMarker, TStateData, TStateTransition, TTransitionData>
        where TStateMarker : IStateMarker<TStateData>
        where TStateTransition : IStateTransition<TStateData, TTransitionData>
    {
        public List<List<IStateMarker<TStateData>>> FindPaths(List<IAgent<TStateData, TTransitionData>> agents, IStateMarker<TStateData> targetState)
        {
            throw new NotImplementedException();
        }

        public abstract HashSet<TStateTransition> GetSuccessors(TStateMarker stateMarker);

        //public abstract HashSet<IStateTransition<TStateData, TTransitionData>> GetSuccessors(IStateMarker<TStateData> stateMarker);
        //HashSet<Func<TStateData, bool>> GetTransitionConditions(IAgent agent);
    }
}
