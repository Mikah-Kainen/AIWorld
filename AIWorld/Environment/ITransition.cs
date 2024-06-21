using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public interface ITransition<TState, TStateData, TTransitionData> //TODO: Make transitions have chances to allow for games with uncertain moves
        where TState : IState<TStateData>
    {
       TTransitionData GetData();
       TState GetStartState();
       TState GetEndState();
    }
}
