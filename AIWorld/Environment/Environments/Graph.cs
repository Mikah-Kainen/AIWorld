using AIWorld.Agent;
using AIWorld.Agent.EnvironmentTokens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment.Environments
{
    public class Node<TNodeData, TEdgeData> : IState<TNodeData>
    {
        private TNodeData data;
        private HashSet<Edge<TNodeData, TEdgeData>> edges;

        public Node(TNodeData data)
        {
            this.data = data;
            edges = new HashSet<Edge<TNodeData, TEdgeData>>();
        }

        public Edge<TNodeData, TEdgeData> AddEdge(Node<TNodeData, TEdgeData> endNode, TEdgeData edgeData)
        {
            Edge<TNodeData, TEdgeData> newEdge = new Edge<TNodeData, TEdgeData>(this, endNode, edgeData);
            edges.Add(newEdge);
            return newEdge;
        }
        public Edge<TNodeData, TEdgeData> AddEdge(Edge<TNodeData, TEdgeData> edge)
        {
            if (edge.GetStartNode() != this)
            {
                return null;
            }
            edges.Add(edge);
            return edge;
        }

        public TNodeData GetData() { return data; }

        public HashSet<Edge<TNodeData, TEdgeData>> GetEdges() { return edges; }

        public bool IsConnected(Node<TNodeData, TEdgeData> targetNode)
        {
            foreach (Edge<TNodeData, TEdgeData> edge in edges)
            {
                if (edge.GetEndNode() == targetNode)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Edge<TNodeData, TEdgeData> : ITransition<TNodeData, TEdgeData>
    {
        private Node<TNodeData, TEdgeData> startNode;
        private Node<TNodeData, TEdgeData> endNode;
        private TEdgeData data;

        public Edge(Node<TNodeData, TEdgeData> edgeStartNode, Node<TNodeData, TEdgeData> edgeEndNode, TEdgeData edgeData)
        {
            startNode = edgeStartNode;
            endNode = edgeEndNode;
            data = edgeData;
        }
        public IState<TNodeData> GetStartState() { return startNode; }
        public IState<TNodeData> GetEndState() { return endNode; }
        public Node<TNodeData, TEdgeData> GetStartNode() { return startNode; }
        public Node<TNodeData, TEdgeData> GetEndNode() { return endNode; }
        public TEdgeData GetData() { return data; }
    }

    public class Graph<TNodeData, TEdgeData> : StateProvider<Node<TNodeData, TEdgeData>, TNodeData, Edge<TNodeData, TEdgeData>, TEdgeData>
    {
        private HashSet<Node<TNodeData, TEdgeData>> nodes;
        private HashSet<Edge<TNodeData, TEdgeData>> edges;

        public Graph()
        {
            nodes = new HashSet<Node<TNodeData, TEdgeData>>();
            edges = new HashSet<Edge<TNodeData, TEdgeData>>();
        }

        public int GetCount() { return nodes.Count; }
        public HashSet<Node<TNodeData, TEdgeData>> GetNodes() { return nodes; }
        public HashSet<Edge<TNodeData, TEdgeData>> GetEdges() { return edges; }
        public bool Contains(Node<TNodeData, TEdgeData> node) { return nodes.Contains(node); }
        public HashSet<Node<TNodeData, TEdgeData>> GetNodes(TNodeData nodeData)
        {
            HashSet<Node<TNodeData, TEdgeData>> returnSet = new HashSet<Node<TNodeData, TEdgeData>>();
            foreach (Node<TNodeData, TEdgeData> node in nodes)
            {
                if (node.GetData().Equals(nodeData))
                {
                    returnSet.Add(node);
                }
            }
            return returnSet;
        }
        public int Contains(TNodeData nodeData) { return GetNodes(nodeData).Count; }
        public HashSet<Edge<TNodeData, TEdgeData>> GetEdges(TNodeData startNodeData, TNodeData endNodeData)
        {
            HashSet<Edge<TNodeData, TEdgeData>> returnEdges = new HashSet<Edge<TNodeData, TEdgeData>>();
            HashSet<Node<TNodeData, TEdgeData>> startNodes = GetNodes(startNodeData);
            if (startNodes.Count == 0)
            {
                return returnEdges;
            }

            foreach (Node<TNodeData, TEdgeData> node in startNodes)
            {
                foreach (Edge<TNodeData, TEdgeData> edge in node.GetEdges())
                {
                    if (edge.GetEndNode().GetData().Equals(endNodeData))
                    {
                        returnEdges.Add(edge);
                    }
                }
            }
            return returnEdges;
        }
        public int AreConnected(TNodeData startNodeData, TNodeData endNodeData) { return GetEdges(startNodeData, endNodeData).Count(); }
        public Node<TNodeData, TEdgeData> AddNode(TNodeData nodeData)
        {
            Node<TNodeData, TEdgeData> newNode = new Node<TNodeData, TEdgeData>(nodeData);
            nodes.Add(newNode);
            return newNode;
        }

        /// <summary>
        /// Returns a new edge connecting startNode and endNode with edgeData
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        /// <param name="edgeData"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Edge<TNodeData, TEdgeData> AddEdge(Node<TNodeData, TEdgeData> startNode, Node<TNodeData, TEdgeData> endNode, TEdgeData edgeData)
        {
            if (!Contains(startNode))
            {
                throw new Exception("StartNode not in Graph");
            }
            if (!Contains(endNode))
            {
                throw new Exception("EndNode not in Graph");
            }
            Edge<TNodeData, TEdgeData> newEdge = new Edge<TNodeData, TEdgeData>(startNode, endNode, edgeData);
            edges.Add(newEdge);
            startNode.AddEdge(newEdge);
            return newEdge;
        }

        /// <summary>
        /// Creates two new nodes with startNodeData and endNodeData. Returns a new edge connecting them with edgeData
        /// </summary>
        /// <param name="startNodeData"></param>
        /// <param name="endNodeData"></param>
        /// <param name="edgeData"></param>
        /// <returns></returns>
        public Edge<TNodeData, TEdgeData> AddEdge(TNodeData startNodeData, TNodeData endNodeData, TEdgeData edgeData)
        {
            Node<TNodeData, TEdgeData> startNode = AddNode(startNodeData);
            Node<TNodeData, TEdgeData> endNode = AddNode(endNodeData);
            Edge<TNodeData, TEdgeData> newEdge = new Edge<TNodeData, TEdgeData>(startNode, endNode, edgeData);
            edges.Add(newEdge);
            startNode.AddEdge(newEdge);
            return newEdge;
        }

        //public HashSet<IStateTransition<TNodeData, TEdgeData>> GetSuccessors(IStateMarker<TNodeData> stateMarker)
        //{
        //    Node<TNodeData, TEdgeData> currentNode = (Node<TNodeData, TEdgeData>)stateMarker;
        //    HashSet<IStateTransition<TNodeData, TEdgeData>> returnSet = new HashSet<IStateTransition<TNodeData, TEdgeData>>();
        //    if (nodes.Contains(currentNode))
        //    {
        //        foreach (Edge<TNodeData, TEdgeData> edge in currentNode.GetEdges())
        //        {
        //            returnSet.Add(edge);
        //        }
        //    }
        //    return returnSet;
        //}

        public override HashSet<TransitionToken<TNodeData, TEdgeData>> GetSuccessors(StateToken<TNodeData> stateMarker)
        {
            Node<TNodeData, TEdgeData> currentNode = RedeemToken(stateMarker);
            HashSet<TransitionToken<TNodeData, TEdgeData>> returnSet = new HashSet<TransitionToken<TNodeData, TEdgeData>>();
            if (nodes.Contains(currentNode))
            {
                foreach (Edge<TNodeData, TEdgeData> edge in currentNode.GetEdges())
                {
                    returnSet.Add(GenerateToken(edge));
                }
            }
            return returnSet;
        }


        public List<List<StateToken<TNodeData>>> FindPaths(List<IAgent<TNodeData, TEdgeData>> agents, IState<TNodeData> targetState)
        {
            List<List<StateToken<TNodeData>>> returnLists = new();
            for (int i = 0; i < agents.Count; i ++)
            {
                returnLists.Add(null);
            }
            bool[] agentsComplete = new bool[agents.Count];
            bool finished = false;
            while (!finished)
            {
                Console.Clear();
                for(int i = 0; i < agents.Count; i ++)
                {
                    if (agentsComplete[i])
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    agents[i].Display();
                    Console.Write("\n");
                    if (agents[i].GetState() != targetState) {
                        HashSet<TransitionToken<TNodeData, TEdgeData>> successors = GetSuccessors(agents[i].GetState());
                        agents[i].SelectMove(successors);
                    }
                    else if (agentsComplete[i] != true)
                    {
                        agentsComplete[i] = true;
                        returnLists[i] = agents[i].GetCurrentPath();
                    }
                    Console.Write("\n\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.ReadLine();

                finished = true;
                for (int i = 0; i < agentsComplete.Length; i ++)
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
    }
}
