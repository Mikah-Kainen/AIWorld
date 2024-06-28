using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public class EnvironmentNew<TPermissions, TAgentID, TStateID, TTransitionID, TState, TStateData, TTransition, TTransitionData> : IEnvironment<TPermissions, TAgentID, TStateID, TTransitionID, TState, TStateData, TTransition, TTransitionData>
        where TState : IState<TStateData>
        where TTransition : ITransition<TState, TStateData, TTransitionData>

        where TAgentID : IEquatable<TAgentID>
        where TStateID : IEquatable<TStateID>
        where TTransitionID : IEquatable<TTransitionID>
    {
        public EnvironmentNew()
        {
            throw new NotImplementedException();
        }

        public List<TTransitionID> GetMoves(TAgentID agentID)
        {
            throw new NotImplementedException();
        }

        public List<TTransitionID> GetMoves(TAgentID agentID, TStateID stateID)
        {
            throw new NotImplementedException();
        }

        public TAgentID GetNextAgentID()
        {
            throw new NotImplementedException();
        }

        public TStateID GetNextStateID()
        {
            throw new NotImplementedException();
        }

        public TTransitionID GetNextTransitionID()
        {
            throw new NotImplementedException();
        }

        public TPermissions GetPermissions(TAgentID agent)
        {
            throw new NotImplementedException();
        }

        public TStateID MakeMove(TAgentID agentID, TTransitionID transitionID)
        {
            throw new NotImplementedException();
        }

        public TAgentID RegisterAgent(TStateData agentStartingData, TPermissions agentStartingPermissions)
        {
            throw new NotImplementedException();
        }
    }
}
