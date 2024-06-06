using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld
{
    public interface IStateProvider<TStateMarker, StateData, TStateTransition, TransitionData>
        where TStateMarker : IStateMarker<StateData>
        where TStateTransition : IStateTransition<TransitionData>
    {
        public List<TStateTransition> GetSuccessors(TStateMarker stateMarker);
        //List<Func<StateData, bool>> GetTransitionConditions(IAgent agent);
    }
}
