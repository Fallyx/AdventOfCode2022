namespace AdventOfCode2022.Day12;

internal class Day12
{
    const string inputPath = @"Day12/Input.txt";
    private static readonly (int x, int y)[] adjacents = 
    {
        ( 0, -1),
        (-1,  0),
        (+1,  0),
        ( 0, +1)
    };
    public static void Task1and2()
    {
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        Dictionary<(int x, int y), ElevationNode> nodes = new Dictionary<(int x, int y), ElevationNode>();
        List<(int x, int y)> aElevation = new List<(int x, int y)>();

        (int x, int y) target = (-1, -1);
        (int x, int y) start = (-1, -1);

        for(int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                char c = lines[y][x];
                if (lines[y][x] == 'S')
                {
                    start = (x, y);
                    c = 'a';
                }
                else if (lines[y][x] == 'E')
                {
                    target = (x, y);
                    c = 'z';
                }
                
                if (c == 'a')
                    aElevation.Add((x, y));

                nodes.Add((x,y), new ElevationNode(x, y, c));
            }
        }

        int shortestPathTask2 = int.MaxValue;
        foreach((int x, int y) in aElevation)
        {
            ResetDistance(nodes);
            nodes[(x, y)].DistanceFromStart = 0;
            shortestPathTask2 = Math.Min(shortestPathTask2, ShortestPath((nodes), (x, y), target, lines[0].Length, lines.Count));
            if (x == start.x && y == start.y)
                Console.WriteLine($"Task 1: {nodes[target].DistanceFromStart}");
        }

        Console.WriteLine($"Task 2: {shortestPathTask2}");
    }

    private static int ShortestPath(Dictionary<(int x, int y), ElevationNode> nodes, (int x, int y) start, (int x, int y) target, int maxX, int maxY)
    {
        Queue<ElevationNode> queue = new Queue<ElevationNode>();
        queue.Enqueue(nodes[start]);

        while (queue.Count > 0)
        {
            ElevationNode currentNode = queue.Dequeue();

            if (currentNode.X == target.x && currentNode.Y == target.y)
                return currentNode.DistanceFromStart;

            for (int i = 0; i < adjacents.Length; i++)
            {
                int newY = currentNode.Y + adjacents[i].y;
                int newX = currentNode.X + adjacents[i].x;

                if (newY < 0 || newX < 0 || newY >= maxY || newX >= maxX) continue;

                ElevationNode nextNode = nodes[(newX, newY)];
                if (currentNode.Height + 1 < nextNode.Height) continue;

                int distanceToNext = currentNode.DistanceFromStart + 1;
                if (nextNode.DistanceFromStart > distanceToNext)
                {
                    nextNode.DistanceFromStart = distanceToNext;
                    queue.Enqueue(nextNode);
                }
            }
        }

        return nodes[target].DistanceFromStart;
    }

    private static void ResetDistance(Dictionary<(int x, int y), ElevationNode> nodes)
    {
        foreach (KeyValuePair<(int x, int y), ElevationNode> keyValuePair in nodes)
        {
            keyValuePair.Value.DistanceFromStart = int.MaxValue;
        }
    }

    private class ElevationNode
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Char Height { get; set; }
        public int DistanceFromStart { get; set; }

        public ElevationNode(int x, int y, char height)
        {
            X = x;
            Y = y;
            Height = height;
            DistanceFromStart = int.MaxValue;
        }
    }
}
