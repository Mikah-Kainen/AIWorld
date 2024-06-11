using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public interface IEnvironment<TStateData, TTransitionData>
    {
        public HashSet<IStateTransition<TStateData, TTransitionData>> GetSuccessors(IStateMarker<TStateData> stateMarker);
        
        //TODO: Add a function that returns the state resulting from a transition for problems with uncertain transitions
    }
}
