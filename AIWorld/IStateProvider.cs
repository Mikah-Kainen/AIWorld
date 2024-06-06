using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld
{
    public interface IStateProvider<StateData, TransitionData>
    {
        public List<IStateTransition<TransitionData>> GetSuccessors(IStateMarker<StateData> stateMarker);
    }
}
