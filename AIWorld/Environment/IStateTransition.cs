using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public interface IStateTransition<StateData, TransitionData> //TODO: Make transitions have chances to allow for games with uncertain moves
    {
        public IStateMarker<StateData> GetStartState();
        public TransitionData GetData();
        public IStateMarker<StateData> GetEndState();
    }
}
