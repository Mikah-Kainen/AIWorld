using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public class StateToken<TStateData>
    {
        private TStateData stateData;

        public StateToken(IState<TStateData> state)
        {
            stateData = state.GetData();
        }

        public TStateData GetData() { return stateData; }
    }
}
