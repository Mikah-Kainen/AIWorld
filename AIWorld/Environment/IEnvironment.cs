using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public interface IEnvironment<StateData, TransitionData>
    {
        public HashSet<IStateTransition<StateData, TransitionData>> GetSuccessors(IStateMarker<StateData> stateMarker);
        
        //TODO: Add a function that returns the state resulting from a transition for problems with uncertain transitions
    }
}
