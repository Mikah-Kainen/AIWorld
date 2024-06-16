using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public interface ITransition<TStateData, TTransitionData> //TODO: Make transitions have chances to allow for games with uncertain moves
    {
        public TTransitionData GetData();
        public IState<TStateData> GetStartState();
        public IState<TStateData> GetEndState();
    }
}
