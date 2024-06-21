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
        private HashSet<Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>> edges;

        public Node(TNodeData data)
        {
            this.data = data;
            edges = new HashSet<Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>>();
        }

        public Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData> AddEdge(TEdgeData edgeData, Node<TNodeData, TEdgeData> endNode)
        {
            Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData> newEdge = new Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>(edgeData, this, endNode);
            edges.Add(newEdge);
            return newEdge;
        }
        public Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData> AddEdge(Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData> edge)
        {
            if (edge.GetStartNode() != this)
            {
                return null;
            }
            edges.Add(edge);
            return edge;
        }

        public TNodeData GetData() { return data; }

        public HashSet<Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>> GetEdges() { return edges; }

        public bool IsConnected(Node<TNodeData, TEdgeData> targetNode)
        {
            foreach (Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData> edge in edges)
            {
                if (edge.GetEndNode() == targetNode)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Edge<TNode, TNodeData, TEdgeData> : ITransition<TNode, TNodeData, TEdgeData>
        where TNode : IState<TNodeData>
    {
        private TEdgeData data;
        private TNode startNode;
        private TNode endNode;

        public Edge(TEdgeData edgeData, TNode edgeStartNode, TNode edgeEndNode)
        {
            data = edgeData;
            startNode = edgeStartNode;
            endNode = edgeEndNode;
        }
        public TEdgeData GetData() { return data; }
        public TNode GetStartState() { return startNode; }
        public TNode GetEndState() { return endNode; }

        public TNode GetStartNode() => GetStartState();
        public TNode GetEndNode() => GetEndState();
    }

    public class Graph<TNodeData, TEdgeData> : Environment<Node<TNodeData, TEdgeData>, TNodeData, Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>, TEdgeData>
    {
        private HashSet<Node<TNodeData, TEdgeData>> nodes;
        private HashSet<Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>> edges;

        public Graph()
        {
            nodes = new HashSet<Node<TNodeData, TEdgeData>>();
            edges = new HashSet<Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>>();
        }

        public int GetCount() { return nodes.Count; }
        public HashSet<Node<TNodeData, TEdgeData>> GetNodes() { return nodes; }
        public HashSet<Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>> GetEdges() { return edges; }
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
        public HashSet<Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>> GetEdges(TNodeData startNodeData, TNodeData endNodeData)
        {
            HashSet<Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>> returnEdges = new HashSet<Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>>();
            HashSet<Node<TNodeData, TEdgeData>> startNodes = GetNodes(startNodeData);
            if (startNodes.Count == 0)
            {
                return returnEdges;
            }

            foreach (Node<TNodeData, TEdgeData> node in startNodes)
            {
                foreach (Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData> edge in node.GetEdges())
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
        public Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData> AddEdge(TEdgeData edgeData, Node<TNodeData, TEdgeData> startNode, Node<TNodeData, TEdgeData> endNode)
        {
            if (!Contains(startNode))
            {
                throw new Exception("StartNode not in Graph");
            }
            if (!Contains(endNode))
            {
                throw new Exception("EndNode not in Graph");
            }
            Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData> newEdge = new Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>(edgeData, startNode, endNode);
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
        public Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData> AddEdge(TEdgeData edgeData, TNodeData startNodeData, TNodeData endNodeData)
        {
            Node<TNodeData, TEdgeData> startNode = AddNode(startNodeData);
            Node<TNodeData, TEdgeData> endNode = AddNode(endNodeData);
            Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData> newEdge = new Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData>(edgeData, startNode, endNode);
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
        //        foreach (Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData> edge in currentNode.GetEdges())
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
                foreach (Edge<Node<TNodeData, TEdgeData>, TNodeData, TEdgeData> edge in currentNode.GetEdges())
                {
                    returnSet.Add(GenerateToken(edge));
                }
            }
            return returnSet;
        }
    }
}
