using AIWorld.Environment;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Agent.EnvironmentTokens
{
    public class TransitionToken<TStateData, TTransitionData>
    {
        private TTransitionData transitionData;
        private StateToken<TStateData> startStateToken;
        private StateToken<TStateData> endStateToken;

        public TransitionToken(TTransitionData transitionData, StateToken<TStateData> startStateToken, StateToken<TStateData> endStateToken)
        {
            this.transitionData = transitionData;
            this.startStateToken = startStateToken;
            this.endStateToken = endStateToken;
        }
        public TTransitionData GetData() => transitionData;
        public StateToken<TStateData> GetStartState() => startStateToken;
        public StateToken<TStateData> GetEndState() => endStateToken;
    }
}
