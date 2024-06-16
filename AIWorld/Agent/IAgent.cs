using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIWorld.Agent.EnvironmentTokens;
using AIWorld.Environment;

namespace AIWorld.Agent
{
    public interface IAgent<TStateData, TTransitionData> : IDisplayable
    {
        StateToken<TStateData> GetState();

        /// <summary>
        /// Selects and moves the agent to the next-best spot among currently know or newly provided(choices) spots
        /// </summary>
        /// <param name="choices"></param>
        /// <returns></returns>
        TransitionToken<TStateData, TTransitionData> SelectMove(HashSet<TransitionToken<TStateData, TTransitionData>> choices);

        List<StateToken<TStateData>> GetCurrentPath();
    }
}
