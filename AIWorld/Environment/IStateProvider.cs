using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public interface IStateProvider<TStateMarker, TStateData, TStateTransition, TTransitionData> : IEnvironment<TStateData, TTransitionData>
        where TStateMarker : IStateMarker<TStateData>
        where TStateTransition : IStateTransition<TStateData, TTransitionData>
    {
        public HashSet<TStateTransition> GetSuccessors(TStateMarker stateMarker);
        //HashSet<Func<TStateData, bool>> GetTransitionConditions(IAgent agent);
    }
}
