using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment.Environments
{
    public class Node<TNodeData, TEdgeData> : IStateMarker<TNodeData>
        where TNodeData : IComparable<TNodeData>
        where TEdgeData : IComparable<TEdgeData>
    {
        private TNodeData data;
        private HashSet<Edge<TNodeData, TEdgeData>> edges;

        public Node(TNodeData data)
        {
            this.data = data;
            edges = new HashSet<Edge<TNodeData, TEdgeData>>();
        }

        public void AddEdge(Node<TNodeData, TEdgeData> endNode, TEdgeData edgeData)
        {
            edges.Add(new Edge<TNodeData, TEdgeData>(this, endNode, edgeData));
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

        public int CompareTo(Node<TNodeData, TEdgeData> targetNode) { return CompareTo(targetNode); }
    }

    public class Edge<NodeData, EdgeData> : IStateTransition<NodeData, EdgeData>
        where NodeData : IComparable<NodeData>
        where EdgeData : IComparable<EdgeData>
    {
        private Node<NodeData, EdgeData> startNode;
        private Node<NodeData, EdgeData> endNode;
        private EdgeData data;

        public Edge(Node<NodeData, EdgeData> edgeStartNode, Node<NodeData, EdgeData> edgeEndNode, EdgeData edgeData)
        {
            startNode = edgeStartNode;
            endNode = edgeEndNode;
            data = edgeData;
        }
        public IStateMarker<NodeData> GetStartState() { return startNode; }
        public IStateMarker<NodeData> GetEndState() { return endNode; }
        public Node<NodeData, EdgeData> GetStartNode() { return startNode; }
        public Node<NodeData, EdgeData> GetEndNode() { return endNode; }
        public EdgeData GetData() { return data; }
        public int CompareTo(Edge<NodeData, EdgeData> targetEdge) { return CompareTo(targetEdge); }
    }

    public class Graph<NodeData, EdgeData> : IStateProvider<Node<NodeData, EdgeData>, NodeData, Edge<NodeData, EdgeData>, EdgeData>
        where NodeData : IComparable<NodeData>
        where EdgeData : IComparable<EdgeData>
    {
        private HashSet<Node<NodeData, EdgeData>> nodes;
        private HashSet<Edge<NodeData, EdgeData>> edges;

        public Graph()
        {
            nodes = new HashSet<Node<NodeData, EdgeData>>();
            edges = new HashSet<Edge<NodeData, EdgeData>>();
        }

        public int GetCount() { return nodes.Count; }
        public HashSet<Node<NodeData, EdgeData>> GetNodes() { return nodes; }
        public HashSet<Edge<NodeData, EdgeData>> GetEdges() { return edges; }
        public bool Contains(Node<NodeData, EdgeData> node) { return nodes.Contains(node); }
        public HashSet<Node<NodeData, EdgeData>> GetNodes(NodeData nodeData)
        {
            HashSet<Node<NodeData, EdgeData>> returnSet = new HashSet<Node<NodeData, EdgeData>>();
            foreach (Node<NodeData, EdgeData> node in nodes)
            {
                if (node.GetData().CompareTo(nodeData) == 0)
                {
                    returnSet.Add(node);
                }
            }
            return returnSet;
        }
        public int Contains(NodeData nodeData) { return GetNodes(nodeData).Count; }
        public HashSet<Edge<NodeData, EdgeData>> GetEdges(NodeData startNodeData, NodeData endNodeData)
        {
            HashSet<Edge<NodeData, EdgeData>> returnEdges = new HashSet<Edge<NodeData, EdgeData>>();
            HashSet<Node<NodeData, EdgeData>> startNodes = GetNodes(startNodeData);
            if (startNodes.Count == 0)
            {
                return returnEdges;
            }

            foreach (Node<NodeData, EdgeData> node in startNodes)
            {
                foreach (Edge<NodeData, EdgeData> edge in node.GetEdges())
                {
                    if (edge.GetEndNode().GetData().CompareTo(endNodeData) == 0)
                    {
                        returnEdges.Add(edge);
                    }
                }
            }
            return returnEdges;
        }
        public int AreConnected(NodeData startNodeData, NodeData endNodeData) { return GetEdges(startNodeData, endNodeData).Count(); }
        public Node<NodeData, EdgeData> AddNode(NodeData nodeData)
        {
            Node<NodeData, EdgeData> newNode = new Node<NodeData, EdgeData>(nodeData);
            nodes.Add(newNode);
            return newNode;
        }
        public Edge<NodeData, EdgeData> AddEdge(Node<NodeData, EdgeData> startNode, Node<NodeData, EdgeData> endNode, EdgeData edgeData)
        {
            if (!Contains(startNode))
            {
                throw new Exception("StartNode not in Graph");
            }
            if (!Contains(endNode))
            {
                throw new Exception("EndNode not in Graph");
            }
            Edge<NodeData, EdgeData> newEdge = new Edge<NodeData, EdgeData>(startNode, endNode, edgeData);
            edges.Add(newEdge);
            return newEdge;
        }
        public Edge<NodeData, EdgeData> AddEdge(NodeData startNodeData, NodeData endNodeData, EdgeData edgeData)
        {
            Node<NodeData, EdgeData> startNode = AddNode(startNodeData);
            Node<NodeData, EdgeData> endNode = AddNode(endNodeData);
            Edge<NodeData, EdgeData> newEdge = new Edge<NodeData, EdgeData>(startNode, endNode, edgeData);
            edges.Add(newEdge);
            return newEdge;
        }

        public HashSet<IStateTransition<NodeData, EdgeData>> GetSuccessors(IStateMarker<NodeData> stateMarker)
        {
            Node<NodeData, EdgeData> currentNode = (Node<NodeData, EdgeData>)stateMarker;
            HashSet<IStateTransition<NodeData, EdgeData>> returnSet = new HashSet<IStateTransition<NodeData, EdgeData>>();
            if (nodes.Contains(currentNode))
            {
                foreach (Edge<NodeData, EdgeData> edge in currentNode.GetEdges())
                {
                    returnSet.Add(edge);
                }
            }
            return returnSet;
        }

        public HashSet<Edge<NodeData, EdgeData>> GetSuccessors(Node<NodeData, EdgeData> stateMarker)
        {
            if (nodes.Contains(stateMarker))
            {
                return stateMarker.GetEdges();
            }
            return new HashSet<Edge<NodeData, EdgeData>>();
        }
    }
}
