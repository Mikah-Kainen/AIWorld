using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld
{
    public class Node<NodeData, EdgeData> : IStateMarker<NodeData>
        where NodeData : IComparable<NodeData>
        where EdgeData : IComparable<EdgeData>
    {
        private NodeData data;
        private List<Edge<NodeData, EdgeData>> edges;

        public Node(NodeData data)
        {
            this.data = data;
            this.edges = new List<Edge<NodeData, EdgeData>>();
        }

        public void AddEdge(Node<NodeData, EdgeData> endNode, EdgeData edgeData)
        {
            edges.Add(new Edge<NodeData, EdgeData>(this, endNode, edgeData));
        }

        public NodeData GetData() { return data; }

        public List<Edge<NodeData, EdgeData>> GetEdges() { return edges; }

        public bool IsConnected(Node<NodeData, EdgeData> targetNode)
        {
            for (int i = 0; i < edges.Count; i ++)
            {
                if (edges[i].GetEndNode() == targetNode)
                {
                    return true;
                }
            }
            return false;
        }

        public int CompareTo(Node<NodeData, EdgeData> targetNode) { return this.CompareTo(targetNode); } 
    }

    public class Edge<NodeData, EdgeData> : IStateTransition<EdgeData>
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

        public Node<NodeData, EdgeData> GetStartNode() { return startNode; }
        public Node<NodeData, EdgeData> GetEndNode() {  return endNode; }
        public EdgeData GetData() { return data; }
        public int CompareTo(Edge<NodeData, EdgeData> targetEdge) { return this.CompareTo(targetEdge); }
    }

    public class Graph<NodeData, EdgeData> : IStateProvider<Node<NodeData, EdgeData>, NodeData, Edge<NodeData, EdgeData>, EdgeData>
        where NodeData : IComparable<NodeData>
        where EdgeData : IComparable<EdgeData>
    {
        private List<Node<NodeData, EdgeData>> nodes;
        private List<Edge<NodeData, EdgeData>> edges;

        public Graph()
        {
            nodes = new List<Node<NodeData, EdgeData>>();
            edges = new List<Edge<NodeData, EdgeData>>();
        }

        public int GetCount() { return nodes.Count; }
        public List<Node<NodeData, EdgeData>> GetNodes() { return nodes; }
        public List<Edge<NodeData, EdgeData>> GetEdges() { return edges; }
        public bool Contains(Node<NodeData, EdgeData> node) { return nodes.Contains(node); }
        public List<Node<NodeData, EdgeData>> GetNodes(NodeData nodeData)
        {
            List<Node<NodeData, EdgeData>> returnList = new List<Node<NodeData, EdgeData>>();
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].GetData().CompareTo(nodeData) == 0)
                {
                    returnList.Add(nodes[i]);
                }
            }
            return returnList;
        }
        public int Contains(NodeData nodeData) { return GetNodes(nodeData).Count; }
        public List<Edge<NodeData, EdgeData>> GetEdges(NodeData startNodeData, NodeData endNodeData)
        {
            List<Edge<NodeData, EdgeData>> returnEdges = new List<Edge<NodeData, EdgeData>>();
            List<Node<NodeData, EdgeData>> startNodes = GetNodes(startNodeData);
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
            if(!Contains(startNode))
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

        public List<IStateTransition<EdgeData>> GetSuccessors(IStateMarker<NodeData> stateMarker)
        {
            Node<NodeData, EdgeData> currentNode = (Node<NodeData, EdgeData>)stateMarker;
            List<IStateTransition<EdgeData>> returnList = new();
            if (nodes.Contains(currentNode))
            {
                foreach (Edge<NodeData, EdgeData> edge in currentNode.GetEdges())
                {
                    returnList.Add(edge);
                }    
            }
            return returnList;
        }

        public List<Edge<NodeData, EdgeData>> GetSuccessors(Node<NodeData, EdgeData> stateMarker)
        {
            if (nodes.Contains(stateMarker))
            {
                return stateMarker.GetEdges();
            }
            return new List<Edge<NodeData, EdgeData>>();
        }
    }
}
