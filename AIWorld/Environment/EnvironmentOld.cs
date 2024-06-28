using AIWorld.Agent;
using AIWorld.Agent.EnvironmentTokens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{

    public abstract class EnvironmentOld<TState, TStateData, TTransition, TTransitionData>
        where TState : IState<TStateData>
        where TTransition : ITransition<TState, TStateData, TTransitionData>
    {
		//public List<List<IState<TStateData>>> FindPaths(List<IAgent<TStateData, TTransitionData>> agents, IState<TStateData> targetState)
		//{
		//    throw new NotImplementedException();
		//}

		private Dictionary<StateToken<TStateData>, TState> tokenToState;
        private Dictionary<TransitionToken<TStateData, TTransitionData>, TTransition> transitionToState;
        public List<IAgent<TStateData, TTransitionData>> Agents;
        public List<TState> AgentPositions;

        public EnvironmentOld()
        {
            tokenToState = new Dictionary<StateToken<TStateData>, TState>();
            transitionToState = new Dictionary<TransitionToken<TStateData, TTransitionData>, TTransition>();
            Agents = new List<IAgent<TStateData, TTransitionData>>();
            AgentPositions = new List<TState>();
        }

        /// <summary>
        /// Runs the Agents until their current state's data .Equals() the target state data
        /// </summary>
        /// <returns></returns>
        public List<List<StateToken<TStateData>>> FindPaths(TStateData targetStateData)
        {
            List<List<StateToken<TStateData>>> returnLists = new();
            for (int i = 0; i < Agents.Count; i++)
            {
                returnLists.Add(null);
            }
            bool[] agentsComplete = new bool[Agents.Count];
            bool finished = false;
            while (!finished)
            {
                Console.Clear();
                for (int i = 0; i < Agents.Count; i++)
                {
                    if (agentsComplete[i])
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Agents[i].Display();
                    Console.Write("\n");
                    if (!Agents[i].GetState().GetData().Equals(targetStateData))
                    {
                        HashSet<TransitionToken<TStateData, TTransitionData>> successors = GetSuccessors(Agents[i].GetState());
                        Agents[i].SelectMove(successors);
                    }
                    else if (agentsComplete[i] != true)
                    {
                        agentsComplete[i] = true;
                        returnLists[i] = Agents[i].GetCurrentPath();
                    }
                    Console.Write("\n\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.ReadLine();

                finished = true;
                for (int i = 0; i < agentsComplete.Length; i++)
                {
                    if (agentsComplete[i] == false)
                    {
                        finished = false;
                        break;
                    }
                }
            }
            return returnLists;
        }

        public void PlaceAgent(IAgent<TStateData, TTransitionData> agent, TState state)
		{
			agent.SetStartState(GenerateToken(state));
			Agents.Add(agent);
		}

		public void PlaceAgents(List<IAgent<TStateData, TTransitionData>> agents, TState state)
		{
			for (int i = 0; i < agents.Count; i++)
			{
				PlaceAgent(agents[i], state);
			}
		}

		protected void PlaceAgents(List<IAgent<TStateData, TTransitionData>> agents, List<TState> states)
		{
			if (agents.Count != states.Count)
			{
				throw new Exception("Agent Placement: Agent-State Mapping Unclear, Agent Count != State Count");
			}
			for (int i = 0; i < agents.Count; i++)
			{
				PlaceAgent(agents[i], states[i]);
			}
		}

		//protected void RemoveAgent(IAgent<TStateData, TTransitionData> agent)
		//{
		//	if (!agents.Contains(agent))
		//	{
		//		throw new Exception("Removing Agent: Agent Not In Environment");
		//	}
		//	agents.Remove(agent);
		//}

		//protected void ClearAgents() => agents.Clear();

		protected List<TState> GetAgentPositions() => AgentPositions;

		protected virtual StateToken<TStateData> GenerateToken(TState state)
        {
            StateToken<TStateData> newToken = new StateToken<TStateData>(state);
            tokenToState.Add(newToken, state);
            return newToken;
        }

        protected virtual TransitionToken<TStateData, TTransitionData> GenerateToken(TTransition transition)
        {
            StateToken<TStateData> endStateToken = GenerateToken(transition.GetEndState());
            TransitionToken<TStateData, TTransitionData> newToken = new TransitionToken<TStateData, TTransitionData>(transition.GetData(), endStateToken);
            transitionToState.Add(newToken, transition);
            return newToken;
        }

        /// <summary>
        /// Returns the state cooresponding to stateToken. Removes stateToken from the state map
        /// </summary>
        /// <param name="stateToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual TState RedeemToken(StateToken<TStateData> stateToken)
        {
            if (tokenToState.ContainsKey(stateToken))
            {
                TState returnState = tokenToState[stateToken];
                //tokenToState.Remove(stateToken); Doesn't work unless I keep track of token's per agent. If not, if two agents have the same token after the first one gets successors the next one will throw!
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
        protected virtual TTransition RedeemToken(TransitionToken<TStateData, TTransitionData> transitionToken)
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
