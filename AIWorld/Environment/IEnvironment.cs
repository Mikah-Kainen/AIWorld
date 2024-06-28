using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    public interface IEnvironment<TPermissions, TAgentID, TStateID, TTransitionID, TState, TStateData, TTransition, TTransitionData>
		where TState : IState<TStateData>
		where TTransition : ITransition<TState, TStateData, TTransitionData>

		where TAgentID : IEquatable<TAgentID>
		where TStateID : IEquatable<TStateID>
		where TTransitionID : IEquatable<TTransitionID>
    {

		TAgentID GetNextAgentID();
		TStateID GetNextStateID();
		TTransitionID GetNextTransitionID();

        //      Environment API:
        /// <summary>
        /// RegisterAgent:
        ///		-Returns: The ID of the agent
        ///		-Parameters: The starting data of the agent, The permissions of the agent
        ///		-Requirements: The starting data is valid
        /// </summary>
        TAgentID RegisterAgent(TStateData agentStartingData, TPermissions agentStartingPermissions);

        /// <summary>
        ///  GetMoves:
        ///-Returns: The moves available to an agent
        ///-Parameters: Agent
        ///-Requirements: Agent is in the environment
        /// </summary>
        List<TTransitionID> GetMoves(TAgentID agentID);

		//  GetMoves:
		//-Returns: The moves available from a state
		//-Parameters: Agent, State
		//-Requirements: Agent is in the environment, Agent has permission to view future states, State is valid
		List<TTransitionID> GetMoves(TAgentID agentID, TStateID stateID);

		//  MakeMove:
		//-Returns: The new state of an agent
		//-Parameters: Agent, Move
		//-Requirements: Agent is in the environment, Move is valid, Agent has permission to make the move
		TStateID MakeMove(TAgentID agentID, TTransitionID transitionID);

        //  GetPermissions:
        //-Returns: The permissions of an Agent
        //-Parameters: Agent
        //-Requirements: Agent is in the environment
        TPermissions GetPermissions(TAgentID agent);

        //  Sense(See, Scan, Read, Etc...):
        //-Returns: The sensed information about the agent's current state
        //-Parameters: Agent, Sense
        //-Requirements: Agent is in the environment, Agent has permission to use the sense


        //  Sense(See, Scan, Read, Etc...) :
        //-Returns: The sensed information about a state
        //-Parameters: Agent, Sense, State
        //-Requirements: Agent is in the environment, State is valid, Agent has permission to use the sense, Agent has permission to view future states


        //  Sense(See, Scan, Read, Etc...):
        //-Returns: The sensed information about a move
        //-Parameters: Agent, Sense, Move
        //-Requirements: Agent is in the environment, Move is valid, Agent has permission to use the sense
    }
}
