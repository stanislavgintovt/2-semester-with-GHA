namespace RoutersConfigure
{
    public class Program
    {
        /// <summary>
        /// Function for parsing graph from file
        /// Format: (node1 (number)): (node2 (number)) (MaxSpeed), ...
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Graph of net of routers</returns>
        public static Routers.Graph ReadGraph(string path)
        {
            string text = File.ReadAllText(path);
            return ReadGraphFromString(text);
        }
        public static Routers.Graph ReadGraphFromString(string graphInString)
        {
            string[] lines = graphInString.Split("\n");
            Routers.Graph graph = new();
            foreach (string line in lines)
            {
                try
                {
                    string[] partition = line.Split(": ");
                    int node1 = int.Parse(partition[0]);
                    partition = partition[1].Split(", ");
                    foreach (string part in partition)
                    {
                        string[] partsOfPart = part.Split(" ");
                        int node2 = int.Parse(partsOfPart[0]);
                        partsOfPart[1] = partsOfPart[1].Replace("(", "");
                        partsOfPart[1] = partsOfPart[1].Replace(")", "");
                        int maxSpeed = int.Parse(partsOfPart[1]);
                        graph.AddEdge(node1, node2, maxSpeed);
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Warning: graph could be readed wrong");
                    File.AppendAllText("log.txt", $"[{DateTime.Now}]: Format error: {e.Message}\n");
                    Console.Error.WriteLine($"Format error: {e.Message}");
                }
            }
            return graph;
        }
        static void Main()
        {
            string path = "example.txt";
            Routers.Graph graph = ReadGraph(path);
            graph.SortEdgesByNode1();
            if (!graph.IsConnective())
            {
                File.AppendAllText("log.txt", $"[{DateTime.Now}]: Error: graph is not connective\n");
                Console.Error.WriteLine("Error: graph is not connective");
            }
            File.WriteAllText(path + ".out.txt", graph.ConstructTree().ToString());
        }
    }
}