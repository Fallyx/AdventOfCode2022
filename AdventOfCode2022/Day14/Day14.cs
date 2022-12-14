using System.Numerics;

namespace AdventOfCode2022.Day14;

internal class Day14
{
    const string inputPath = @"Day14/Input.txt";
    private static Vector2 sandSource = new Vector2(500, 0);

    public static void Task1and2()
    {
        Dictionary<Vector2, char> mapTask1 = new Dictionary<Vector2, char>();
        Dictionary<Vector2, char> mapTask2 = new Dictionary<Vector2, char>();
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

                FillRocks(mapTask1, new Vector2(start[0], start[1]), new Vector2(end[0], end[1]));
                FillRocks(mapTask2, new Vector2(start[0], start[1]), new Vector2(end[0], end[1]));
            }
        }

        SandFall(maxY, mapTask1);
        Console.WriteLine($"Task 1: {mapTask1.Values.Count(c => c == 'o')}");

        SandFall(maxY + 2, mapTask2, true);
        Console.WriteLine($"Task 2: {mapTask2.Values.Count(c => c == 'o')}");
    }

    private static void FillRocks(Dictionary<Vector2, char> map, Vector2 start, Vector2 end) 
    {
        int diffX = (int) Math.Abs(start.X - end.X);
        int diffY = (int) Math.Abs(start.Y - end.Y);
        int lowerX = (int) Math.Min(start.X, end.X);
        int lowerY = (int) Math.Min(start.Y, end.Y);

        for (int y = 0; y <= diffY; y++)
        {
            for (int x = 0; x <= diffX; x++)
            {
                map.TryAdd(new Vector2(lowerX + x, lowerY + y), '#');
            }
        }
    }

    private static void SandFall(int maxY, Dictionary<Vector2, char> map, bool isTask2 = false)
    {
        Vector2 sandPos = sandSource;

        while (sandPos.Y <= maxY + 1)
        {
            if (isTask2 & map.ContainsKey(sandSource))
                return;

            if (isTask2 & sandPos.Y == maxY)
            {
                map.Add(new Vector2(sandPos.X, sandPos.Y - 1), 'o');
                sandPos = sandSource;
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
                sandPos = sandSource;
            }
        }
    }
}
