using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public interface IStateTransition<TStateData, TTransitionData> //TODO: Make transitions have chances to allow for games with uncertain moves
    {
        public IStateMarker<TStateData> GetStartState();
        public TTransitionData GetData();
        public IStateMarker<TStateData> GetEndState();
    }
}
