using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public interface IStateProvider<TStateMarker, StateData, TStateTransition, TransitionData> : IEnvironment<StateData, TransitionData>
        where TStateMarker : IStateMarker<StateData>
        where TStateTransition : IStateTransition<StateData, TransitionData>
    {
        public HashSet<TStateTransition> GetSuccessors(TStateMarker stateMarker);
        //HashSet<Func<StateData, bool>> GetTransitionConditions(IAgent agent);
    }
}
