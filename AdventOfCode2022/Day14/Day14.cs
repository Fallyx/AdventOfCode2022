using System.Numerics;

namespace AdventOfCode2022.Day14;

internal class Day14
{
    const string inputPath = @"Day14/Input.txt";
    public static void Task1()
    {
        Dictionary<Vector2, char> map = new Dictionary<Vector2, char>();
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        int maxY = int.MinValue;

        foreach(string line in lines)
        {
            string[] coords = line.Split(" -> ");

            for(int i = 0; i < coords.Length - 1; i++)
            {
                int[] start = coords[i].Split(',').Select(int.Parse).ToArray();
                int[] end = coords[i + 1].Split(',').Select(int.Parse).ToArray();

                maxY = Math.Max(maxY, Math.Max(start[1], end[1]));

                if (start[0] == end[0])
                    FillRock(map, start[1], end[1], start[0], true);
                else
                    FillRock(map, start[0], end[0], start[1], false);
            }
        }

        map.Add(new Vector2(500, 0), '+');

        SandFall(new Vector2(500, 1), maxY, map);

    //    PrintMap(map, maxY, new Vector2());

        Console.WriteLine($"Task 1: {map.Values.Count(c => c == 'o')}");
    }

    public static void Task2()
    {
        Dictionary<Vector2, char> map = new Dictionary<Vector2, char>();
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        int maxY = int.MinValue;

        foreach(string line in lines)
        {
            string[] coords = line.Split(" -> ");

            for(int i = 0; i < coords.Length - 1; i++)
            {
                int[] start = coords[i].Split(',').Select(int.Parse).ToArray();
                int[] end = coords[i + 1].Split(',').Select(int.Parse).ToArray();

                maxY = Math.Max(maxY, Math.Max(start[1], end[1]));

                if (start[0] == end[0])
                    FillRock(map, start[1], end[1], start[0], true);
                else
                    FillRock(map, start[0], end[0], start[1], false);
            }
        }

    //    map.Add(new Vector2(500, 0), '+');

    //    SandFall(new Vector2(500, 1), maxY, map);

        SandFall2(new Vector2(500, 0), maxY + 2, map);

        PrintMap(map, maxY + 2, new Vector2());

        Console.WriteLine($"Task 2: {map.Values.Count(c => c == 'o')}");
    }

    private static void FillRock(Dictionary<Vector2, char> map, int start, int end, int constant, bool isConstantX)
    {
        int dif = Math.Abs(start - end);
        int lower = Math.Min(start, end);
        for(int i = 0; i <= dif; i++)
        {
            Vector2 coord = (isConstantX ? new Vector2(constant, lower + i) : new Vector2(lower + i, constant));
            map.TryAdd(coord, '#');
        }
    }

    private static void SandFall(Vector2 start, int maxY, Dictionary<Vector2, char> map)
    {
        Vector2 sandPos = new Vector2(start.X, start.Y);

        while (sandPos.Y <= maxY + 1)
        {
            /*
            Console.WriteLine("\n");
            Console.WriteLine($"sandPos={sandPos}");
            PrintMap(map, maxY, sandPos);
            */

            if (!map.ContainsKey(sandPos))
            {
                sandPos.Y++;
                continue;
            }      

            Vector2 newPos = new Vector2(sandPos.X - 1, sandPos.Y);
            if (!map.ContainsKey(newPos))
            {
                sandPos = new Vector2(newPos.X, newPos.Y);
                continue;
            }

            newPos = new Vector2(sandPos.X + 1, sandPos.Y);
            if (!map.ContainsKey(newPos))
            {
                sandPos = new Vector2(newPos.X, newPos.Y);
                continue;
            }

            newPos = new Vector2(sandPos.X, sandPos.Y - 1);
            if (!map.ContainsKey(newPos))
            {
                map.Add(newPos, 'o');
                sandPos = new Vector2(500, 1);
            }
        }
    }

    private static void SandFall2(Vector2 start, int maxY, Dictionary<Vector2, char> map)
    {
        Vector2 sandPos = new Vector2(start.X, start.Y);

        while (sandPos.Y <= maxY + 1)
        {
        //    Console.WriteLine("\n");
        //    Console.WriteLine($"sandPos={sandPos}");
        //    PrintMap(map, maxY, sandPos);

            if (map.ContainsKey(new Vector2(500, 0)))
                return;

            if (sandPos.Y == maxY)
            {
                map.Add(new Vector2(sandPos.X, sandPos.Y - 1), 'o');
                sandPos = new Vector2(500, 0);
                continue;
            }     

            if (!map.ContainsKey(sandPos))
            {
                sandPos.Y++;
                continue;
            } 

            Vector2 newPos = new Vector2(sandPos.X - 1, sandPos.Y);
            if (!map.ContainsKey(newPos))
            {
                sandPos = new Vector2(newPos.X, newPos.Y);
                continue;
            }

            newPos = new Vector2(sandPos.X + 1, sandPos.Y);
            if (!map.ContainsKey(newPos))
            {
                sandPos = new Vector2(newPos.X, newPos.Y);
                continue;
            }

            newPos = new Vector2(sandPos.X, sandPos.Y - 1);
            if (!map.ContainsKey(newPos))
            {
                map.Add(newPos, 'o');
                sandPos = new Vector2(500, 0);
            }
        }
    }

    private static void PrintMap(Dictionary<Vector2, char> map, int maxY, Vector2 pos)
    {
        for(int y = 0; y <= maxY; y++)
        {
            //for(int x = 420; x < 510; x++)
            for(int x = 493; x <= 503; x++)
            {
                if (pos.X == x && pos.Y == y)
                {
                    Console.Write('X');
                    continue;
                }

                Char c;
                if (!map.TryGetValue(new Vector2(x, y), out c))
                    c = '.';
                
                Console.Write(c);
            }

            Console.WriteLine();
        }
    }
}
