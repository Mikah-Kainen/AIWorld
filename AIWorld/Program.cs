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

            graph.AddEdge(n3_1, n3_2, 1);
            graph.AddEdge(n3_2, n5_1, 3);
            graph.AddEdge(n5_1, n10_1, 2);

            graph.AddEdge(n3_1, n4_1, 5);
            graph.AddEdge(n4_1, n10_1, 8);

            graph.AddEdge(5, 10, 3);

            List<IAgent<int, int>> agents = new();
            agents.Add(new BreathFirstAgent<int, int>(n3_1));
            agents.Add(new BreathFirstAgent<int, int>(n5_1));
            graph.FindPaths(agents, n10_1);
        }
    }
}