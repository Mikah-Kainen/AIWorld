using AIWorld.Agent;
using AIWorld.Agent.Agents;
using AIWorld.Environment.Environments;

namespace AIWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph<int, int> graph = new Graph<int, int>();
            var n3_1 = graph.AddNode(3);
            var n3_2 = graph.AddNode(3);
            var n4_1 = graph.AddNode(4);
            var n5_1 = graph.AddNode(5);
            var n10_1 = graph.AddNode(10);

            graph.AddEdge(1, n3_1, n3_2);
            graph.AddEdge(2, n3_2, n5_1);
            graph.AddEdge(3, n5_1, n10_1);

            graph.AddEdge(12, n3_1, n4_1);
            graph.AddEdge(13, n4_1, n10_1);

            graph.AddEdge(100, 10, 3);

            List<IAgent<int, int>> agents = new();
            agents.Add(new BreathFirstAgent<int, int>());
            agents.Add(new BreathFirstAgent<int, int>());
            graph.PlaceAgents(agents, n3_1);
            graph.FindPaths(10);
            //graph.FindPaths(agents, n10_1);
        }
    }
}