using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public interface IStateTransition<StateData, TransitionData>
    {
        public IStateMarker<StateData> GetStartState();
        public TransitionData GetData();
        public IStateMarker<StateData> GetEndState();
    }
}
