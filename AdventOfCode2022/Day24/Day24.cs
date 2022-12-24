using System.Numerics;

namespace AdventOfCode2022.Day24;

internal class Day24
{
    const string inputPath = @"Day24/Input.txt";
    public static void Task1()
    {
        List<string> lines = File.ReadAllLines(inputPath).ToList();
        HashSet<Vector2> map = new HashSet<Vector2>();
        List<Blizzard> blizzards = new List<Blizzard>();

        for(int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#')
                    continue;
                
                map.Add(new Vector2(x, y));

                if (lines[y][x] != '.')
                    blizzards.Add(new Blizzard(x, y, lines[y][x]));
            }
        }

        Console.WriteLine($"Task 1: {ShortestPath(map, blizzards)}");
    }

    private static int ShortestPath(HashSet<Vector2> map, List<Blizzard> blizzards)
    {
        Queue<(Vector2 elfPos, int minute)> queue = new Queue<(Vector2 elfPos, int minute)>();
        HashSet<(Vector2 elfPos, int min)> visited = new HashSet<(Vector2 elfPos, int min)>();
        Dictionary<int, List<Blizzard>> blizzardStates = new Dictionary<int, List<Blizzard>>();
        Vector2[] moves = new Vector2[5] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0), Vector2.Zero };
        Vector2 target = new Vector2();
        target.X = map.Max(m => m.X);
        target.Y = map.Max(m => m.Y);

        blizzardStates.Add(0, blizzards);
        queue.Enqueue((new Vector2(1, 0), 0));

        while(queue.Count > 0)
        {
            (Vector2 elfPos, int minute) = queue.Dequeue();

            if (elfPos == target)
                return minute;
                
            List<Blizzard> bs;
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
                if (bs.Any(b => b.Position.X == elfNextPos.X && b.Position.Y == elfNextPos.Y))
                    continue;
                if (visited.Add((elfNextPos, minute + 1)))
                    queue.Enqueue((elfNextPos, minute + 1));
            }
        }

        return int.MaxValue;
    }

    private static void NextMove(HashSet<Vector2> map, List<Blizzard> blizzards, HashSet<(Vector2 elfPos, int min)> visited, Vector2 elfPos, int minute)
    {
        Vector2[] moves = new Vector2[5] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0), Vector2.Zero };

        for (int i = 0; i < moves.Length; i++)
        {
            Vector2 elfNextPos = elfPos + moves[i];

            if (!map.Contains(elfNextPos))
                continue;
            if (blizzards.Any(b => b.Position.X == elfNextPos.X && b.Position.Y == elfNextPos.Y))
                continue;
            
        }
    }

    private static Vector2 WrapPos(HashSet<Vector2> map, Blizzard blizzard)
    {
        Vector2 nextPos = new Vector2(blizzard.Position.X, blizzard.Position.Y);

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

    private static List<Blizzard> GenerateBlizzardState(HashSet<Vector2> map, List<Blizzard> blizzards)
    {
        List<Blizzard> newBs = new List<Blizzard>();
        foreach (Blizzard b in blizzards)
        {
            Blizzard newB = new Blizzard((int) b.Position.X, (int) b.Position.Y, b.Character);
            Vector2 nextPos = newB.NextPos();
            if (map.Contains(nextPos))
                newB.Position = nextPos;
            else
                newB.Position = WrapPos(map, newB);

            newBs.Add(newB);
        }

        return newBs;
    }

    private static void PrintMap(HashSet<Vector2> map, List<Blizzard> blizzards)
    {
        int minX = (int) map.Min(v => v.X) - 1;
        int maxX = (int) map.Max(v => v.X) + 1;
        int minY = (int) map.Min(v => v.Y);
        int maxY = (int) map.Max(v => v.Y);

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                List<Blizzard> bs = blizzards.Where(b => b.Position.X == x && b.Position.Y == y).ToList();
                if (bs.Count == 1)
                    Console.Write(bs.First().Character);
                else if (bs.Count > 1)
                    Console.Write(bs.Count);
                else if (map.Contains(new Vector2(x, y)))
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

        public Vector2 Position { get; set; }
        public Direction Dir { get; set; }
        public char Character { get; set; }

        public Blizzard(int x, int y, char c)
        {
            Position = new Vector2(x, y);
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
                return Position + new Vector2(0, -1);
            else if (Dir == Direction.Down)
                return Position + new Vector2(0, 1);
            else if (Dir == Direction.Left)
                return Position + new Vector2(-1 , 0);
            else
                return Position + new Vector2(1, 0);
        }
    }
}
