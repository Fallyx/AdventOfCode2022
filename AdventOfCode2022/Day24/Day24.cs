using System.Numerics;

namespace AdventOfCode2022.Day24;

internal class Day24
{
    const string inputPath = @"Day24/Input.txt";
    public static void Task1()
    {
        List<string> lines = File.ReadAllLines(inputPath).ToList();
        HashSet<Vector2> map = new HashSet<Vector2>();
        Dictionary<Vector2, List<Blizzard>> blizzards = new Dictionary<Vector2, List<Blizzard>>();

        for(int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#')
                    continue;
                
                map.Add(new Vector2(x, y));

                if (lines[y][x] != '.')
                {
                    Vector2 pos = new Vector2(x, y);
                    if (!blizzards.ContainsKey(pos))
                        blizzards.Add(pos, new List<Blizzard>());
                        
                    blizzards[pos].Add(new Blizzard(lines[y][x])); 
                }
                    
            }
        }

        Vector2 start = new Vector2(1, 0);
        Vector2 target = new Vector2();
        target.X = map.Max(m => m.X);
        target.Y = map.Max(m => m.Y);

        Console.WriteLine($"Task 1: {ShortestPath(map, blizzards, start, target).Item1}");

        int sumMins = 0;
        Dictionary<Vector2, List<Blizzard>> bs = blizzards;
        for (int i = 0; i < 3; i++)
        {
            int mins = 0;
            if (i % 2 == 0)
                (mins, bs) = ShortestPath(map, bs, start, target);
            else
                (mins, bs) = ShortestPath(map, bs, target, start);
            sumMins += mins;
        }

        Console.WriteLine($"Task 2: {sumMins}");
    }

    private static (int, Dictionary<Vector2, List<Blizzard>>) ShortestPath(HashSet<Vector2> map, Dictionary<Vector2, List<Blizzard>> blizzards, Vector2 start, Vector2 target)
    {
        Queue<(Vector2 elfPos, int minute)> queue = new Queue<(Vector2 elfPos, int minute)>();
        HashSet<(Vector2 elfPos, int min)> visited = new HashSet<(Vector2 elfPos, int min)>();
        Dictionary<int, Dictionary<Vector2, List<Blizzard>>> blizzardStates = new Dictionary<int, Dictionary<Vector2, List<Blizzard>>>();
        Vector2[] moves = new Vector2[5] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0), Vector2.Zero };

        blizzardStates.Add(0, blizzards);
        queue.Enqueue((start, 0));

        while(queue.Count > 0)
        {
            (Vector2 elfPos, int minute) = queue.Dequeue();

            if (elfPos == target)
                return (minute, blizzardStates[minute]);
                
            Dictionary<Vector2, List<Blizzard>> bs;
            if (blizzardStates.ContainsKey(minute + 1))
                bs = blizzardStates[minute + 1];
            else
            {
                bs = GenerateBlizzardState(map, blizzardStates[minute]);
                blizzardStates.Add(minute + 1, bs);
            }

            for (int i = 0; i < moves.Length; i++)
            {
                Vector2 elfNextPos = elfPos + moves[i];
                if (!map.Contains(elfNextPos))
                    continue;
                if (bs.ContainsKey(elfNextPos))
                    continue;
                if (visited.Add((elfNextPos, minute + 1)))
                    queue.Enqueue((elfNextPos, minute + 1));
            }
        }

        return (int.MaxValue, new Dictionary<Vector2, List<Blizzard>>());
    }

    private static Vector2 WrapPos(HashSet<Vector2> map, Blizzard blizzard, Vector2 blizzardPos)
    {
        Vector2 nextPos = blizzardPos;

        if (blizzard.Dir == Blizzard.Direction.Up)
            nextPos.Y = map.Max(m => m.Y) - 1;
        else if (blizzard.Dir == Blizzard.Direction.Down)
            nextPos.Y = map.Min(m => m.Y) + 1;
        else if (blizzard.Dir == Blizzard.Direction.Left)
            nextPos.X = map.Max(m => m.X);
        else
            nextPos.X = map.Min(m => m.X);

        return nextPos;
    }

    private static Dictionary<Vector2, List<Blizzard>> GenerateBlizzardState(HashSet<Vector2> map, Dictionary<Vector2, List<Blizzard>> blizzards)
    {
        Dictionary<Vector2, List<Blizzard>> newBs = new Dictionary<Vector2, List<Blizzard>>();
        foreach (KeyValuePair<Vector2, List<Blizzard>> entry in blizzards)
        {
            foreach(Blizzard b in entry.Value)
            {
                Blizzard newB = new Blizzard(b.Character);
                Vector2 newPos = entry.Key + newB.NextPos();

                if(!map.Contains(newPos))
                    newPos = WrapPos(map, b, entry.Key);

                if (!newBs.ContainsKey(newPos))
                    newBs.Add(newPos, new List<Blizzard>());

                newBs[newPos].Add(newB);
            }
        }

        return newBs;
    }

    private static void PrintMap(HashSet<Vector2> map, Dictionary<Vector2, List<Blizzard>> blizzards)
    {
        int minX = (int) map.Min(v => v.X) - 1;
        int maxX = (int) map.Max(v => v.X) + 1;
        int minY = (int) map.Min(v => v.Y);
        int maxY = (int) map.Max(v => v.Y);

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                Vector2 pos = new Vector2(x, y);
                List<Blizzard> bs = blizzards[pos];
                if (bs.Count == 1)
                    Console.Write(bs.First().Character);
                else if (bs.Count > 1)
                    Console.Write(bs.Count);
                else if (map.Contains(pos))
                    Console.Write('.');
                else
                    Console.Write('#');
            }
            Console.WriteLine();
        }

        Console.WriteLine("\n");
    }

    private class Blizzard
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        };

        public Direction Dir { get; set; }
        public char Character { get; set; }

        public Blizzard(char c)
        {
            Character = c;
            if (c == '^')
                Dir = Direction.Up;
            else if (c == 'v')
                Dir = Direction.Down;
            else if (c == '<')
                Dir = Direction.Left;
            else 
                Dir = Direction.Right;
        }

        public Vector2 NextPos()
        {
            if (Dir == Direction.Up)
                return new Vector2(0, -1);
            else if (Dir == Direction.Down)
                return new Vector2(0, 1);
            else if (Dir == Direction.Left)
                return new Vector2(-1 , 0);
            else
                return new Vector2(1, 0);
        }
    }
}
