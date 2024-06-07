using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public interface IStateProviderBasic<StateData, TransitionData>
    {
        public HashSet<IStateTransition<StateData, TransitionData>> GetSuccessors(IStateMarker<StateData> stateMarker);
    }
}
