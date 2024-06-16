using AIWorld.Agent;
using AIWorld.Agent.EnvironmentTokens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public abstract class StateProvider<TState, TStateData, TTransition, TTransitionData>
        where TState : IState<TStateData>
        where TTransition : ITransition<TStateData, TTransitionData>
    {
        //public List<List<IState<TStateData>>> FindPaths(List<IAgent<TStateData, TTransitionData>> agents, IState<TStateData> targetState)
        //{
        //    throw new NotImplementedException();
        //}

        private Dictionary<StateToken<TStateData>, TState> tokenToState;
        private Dictionary<TransitionToken<TStateData, TTransitionData>, TTransition> transitionToState;

        public StateProvider()
        {
            tokenToState = new Dictionary<StateToken<TStateData>, TState>();
            transitionToState = new Dictionary<TransitionToken<TStateData, TTransitionData>, TTransition>();
        }

        protected StateToken<TStateData> GenerateToken(TState state)
        {
            StateToken<TStateData> newToken = new StateToken<TStateData>(state);
            tokenToState.Add(newToken, state);
            return newToken;
        }

        protected TransitionToken<TStateData, TTransitionData> GenerateToken(TTransition transition)
        {
            StateToken<TStateData> startStateToken = GenerateToken(transition.GetStartState());
            StateToken<TStateData> endStateToken = GenerateToken(transition.GetEndState());
            TransitionToken<TStateData, TTransitionData> newToken = new TransitionToken<TStateData, TTransitionData>(transition.GetData(), startStateToken, endStateToken);
            transitionToState.Add(newToken, transition);
            return newToken;
        }

        /// <summary>
        /// Returns the state cooresponding to stateToken. Removes stateToken from the state map
        /// </summary>
        /// <param name="stateToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected TState RedeemToken(StateToken<TStateData> stateToken)
        {
            if (tokenToState.ContainsKey(stateToken))
            {
                TState returnState = tokenToState[stateToken];
                tokenToState.Remove(stateToken);
                return returnState;
            }
            else
            {
                throw new Exception("Invalid Token Redemption Request: State Token Not Mapped");
            }
        }

        /// <summary>
        /// Returns the transition cooresponding to transitionToken. Removes transitionToken from the transition map
        /// </summary>
        /// <param name="transitionToken"></param>
        /// <returns></returns>
        protected TTransition RedeemToken(TransitionToken<TStateData, TTransitionData> transitionToken)
        {
            if (transitionToState.ContainsKey(transitionToken))
            {
                TTransition returnTransition = transitionToState[transitionToken];
                transitionToState.Remove(transitionToken);
                return returnTransition;
            }
            else
            {
                throw new Exception("Invalid Token Redemption Request: Transition Token Not Mapped");
            }
        }

        /// <summary>
        /// Returns the successors corresponding to stateToken. Removes stateToken from the state map
        /// </summary>
        /// <param name="stateToken"></param>
        /// <returns></returns>
        public abstract HashSet<TransitionToken<TStateData, TTransitionData>> GetSuccessors(StateToken<TStateData> stateToken);

        //public abstract HashSet<IStateTransition<TStateData, TTransitionData>> GetSuccessors(IStateMarker<TStateData> stateMarker);
        //HashSet<Func<TStateData, bool>> GetTransitionConditions(IAgent agent);
    }
}
