namespace RoutersConfigure
{
    public class Routers
    {
        /// <summary>
        /// Class, representing graph
        /// </summary>
        public class Graph
        {
            /// <summary>
            /// Show count of unique vertices
            /// </summary>
            public int CountOfVertices
            {
                get
                {
                    List<int> vertices = [];
                    int count = 0;
                    for (int i = 0; i < edges.Count; i++)
                    {
                        if (!vertices.Contains(edges[i].Node1))
                        {
                            vertices.Add(edges[i].Node1);
                            count++;
                        }
                        if (!vertices.Contains(edges[i].Node2))
                        {
                            vertices.Add(edges[i].Node2);
                            count++;
                        }
                    }
                    return count;
                }
            }
            /// <summary>
            /// Graph is defined by his edges
            /// </summary>
            /// <param name="Node1">Number of node with smaller number</param>
            /// <param name="Node2">Number of node with bigger number</param>
            /// <param name="MaxSpeed">Traffic capacity of connection</param>
            public sealed record Edge(int Node1, int Node2, int MaxSpeed) : IComparable<Edge>
            {
                public override int GetHashCode()
                {
                    return (int) (Node1 ^ Node2) ^ MaxSpeed;
                }
                public bool Equals(Edge? other)
                {
                    if (other is null)
                    {
                        if (this is null)
                        {
                            return true;
                        }
                        return false;
                    }
                    return MaxSpeed == other.MaxSpeed;
                }
                public int CompareTo(Edge? other)
                {
                    if (other is null)
                    {
                        return 1;
                    }
                    return -MaxSpeed.CompareTo(other.MaxSpeed);
                }
                public static int CompareByNode1(Edge? edge1, Edge? edge2)
                {
                    if (edge1 is null)
                    {
                        if (edge2 is null)
                        {
                            return 0;
                        }
                        return 1;
                    }
                    if (edge2 is null)
                    {
                        return -1;
                    }
                    return edge1.Node1.CompareTo(edge2.Node2);
                }
            }
            readonly List<Edge> edges = [];
            /// <summary>
            /// Adds edge defined by it's parameters to graph
            /// </summary>
            /// <param name="node1"></param>
            /// <param name="node2"></param>
            /// <param name="maxSpeed"></param>
            public void AddEdge(int node1, int node2, int maxSpeed)
            {
                Edge edge = new(node1, node2, maxSpeed);
                edges.Add(edge);
            }
            /// <summary>
            /// Adds edge as object to graph
            /// </summary>
            /// <param name="edge"></param>
            public void AddEdge(Edge edge)
            {
                edges.Add(edge);
            }
            /// <summary>
            /// Removes edge in the end of list
            /// </summary>
            public void RemoveLastEdge()
            {
                edges.Remove(edges.Last());
            }
            /// <summary>
            /// Sorts edges by traffic capacity
            /// </summary>
            public void SortEdges()
            {
                edges.Sort();
            }
            /// <summary>
            /// Sorts edges by the first vertice
            /// </summary>
            public void SortEdgesByNode1()
            {
                edges.Sort(Edge.CompareByNode1);
            }
            /// <summary>
            /// Checks if graph is connective. 
            /// Uses bfs
            /// Worst complexity is O(E)
            /// </summary>
            /// <returns></returns>
            public bool IsConnective()
            {
                bool[] isInConnect = new bool[CountOfVertices];
                for (int i = 0; i < CountOfVertices; i++)
                {
                    isInConnect[i] = false;
                }
                isInConnect[0] = true;
                bool wasAnythingMarked = true;
                while (wasAnythingMarked)
                {
                    wasAnythingMarked = false;
                    for (int i = 0; i < edges.Count; i++)
                    {
                        if (isInConnect[edges[i].Node1 - 1] ^ isInConnect[edges[i].Node2 - 1])
                        {
                            wasAnythingMarked = true;
                            isInConnect[edges[i].Node1 - 1] = true;
                            isInConnect[edges[i].Node2 - 1] = true;
                        }
                    }
                }
                for (int i = 0; i < isInConnect.Length; i++)
                {
                    if (!isInConnect[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            /// <summary>
            /// Checks if graph has cycle
            /// Uses dfs
            /// Worst complexity in my realization is O(E^2)
            /// It could be better if graph was represented as adjacency matrix or as real graph 
            /// </summary>
            /// <returns></returns>
            public bool HasCycle(int currentNode = 1)
            {
                Stack<int> cycle = new();
                int[] colorOfNodes = new int[CountOfVertices];
                cycle.Push(currentNode);
                for (int i = 0; i < CountOfVertices; i++)
                {
                    colorOfNodes[i] = 0;
                }
                while (cycle.Count > 0)
                {
                    currentNode = cycle.Peek();
                    if (colorOfNodes[currentNode - 1] == 0)
                    {
                        colorOfNodes[currentNode - 1]++;
                        foreach (Edge edge in edges)
                        {
                            if (edge.Node1 == currentNode)
                            {
                                if (colorOfNodes[edge.Node2 - 1] == 0)
                                {
                                    cycle.Push(edge.Node2);
                                }
                            }
                            if (edge.Node2 == currentNode)
                            {
                                if (colorOfNodes[edge.Node1 - 1] == 0)
                                {
                                    cycle.Push(edge.Node1);
                                }
                            }
                        }
                    }
                    else if (colorOfNodes[currentNode - 1] == 1)
                    {
                        colorOfNodes[currentNode - 1]++;
                        cycle.Pop();
                    }
                    else
                    {
                        return true;
                    }

                }
                return false;
            }
            /// <summary>
            /// Construct tree with no cycles and with max sum of traffic capacity
            /// Complexity is O(E*(HasCycle complexity))
            /// </summary>
            /// <returns></returns>
            public Graph ConstructTree()
            {
                Graph graph = new Graph();
                SortEdges();
                while (edges.Count > 0)
                {
                    Edge edge = edges[0];
                    graph.AddEdge(edge);
                    edges.Remove(edge);
                    if (graph.HasCycle())
                    {
                        graph.RemoveLastEdge();
                    }
                }
                return graph;
            }
            /// <summary>
            /// Represents graph to the string format
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                string graph = $"{edges[0].Node1}: ";
                int node;
                SortEdgesByNode1();
                node = edges[0].Node1;
                foreach (Edge edge in edges)
                {
                    if (edge.Node1 != node)
                    {
                        graph = graph.Remove(graph.Length - 1);
                        node = edge.Node1;
                        graph += $"\n{node}: ";
                    }
                    graph += $"{edge.Node2} ({edge.MaxSpeed}), ";
                }
                graph = graph.Remove(graph.Length - 2);
                return graph;
            }
        }
    }
}