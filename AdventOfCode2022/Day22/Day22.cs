using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day22;

internal class Day22
{
    const string inputPath = @"Day22/Input.txt";
    private static Dictionary<(Vector2, int), (Vector2, int)> wrapMap = new Dictionary<(Vector2, int), (Vector2, int)>();
    public static void Task1and2()
    {
        Dictionary<Vector2, Char> map = new Dictionary<Vector2, char>();
        List<string> lines = File.ReadAllLines(inputPath).ToList();
        List<string> descriptions = new List<string>();
        string pattern = @"(\d+)([LR])?";
        wrapMap = CreateWrapMap();

        for (int linesIdx = 0; linesIdx < lines.Count; linesIdx++)
        {
            if (string.IsNullOrEmpty(lines[linesIdx]))
                break;

            for(int i = 0; i < lines[linesIdx].Length; i++)
            {
                if (lines[linesIdx][i] != ' ')
                    map.Add(new Vector2(i + 1, linesIdx + 1), lines[linesIdx][i]);
            }
        }

        foreach (Match m in Regex.Matches(lines.Last(), pattern))
        {
            descriptions.Add(m.Groups[1].Value);
            descriptions.Add(m.Groups[2].Value);
        }

        descriptions.RemoveAt(descriptions.Count - 1);
        
        (Vector2 currentPos, int dirIdx) = ApplyDescription(map, descriptions);
        Console.WriteLine($"Task 1: {(currentPos.Y) * 1000 + (currentPos.X) * 4 + dirIdx}");

        (currentPos, dirIdx) = ApplyDescription(map, descriptions, false);
        Console.WriteLine($"Task 2: {(currentPos.Y) * 1000 + (currentPos.X) * 4 + dirIdx}");
    }

    private static (Vector2 currentPos, int dirIdx) ApplyDescription(Dictionary<Vector2, Char> map, List<string> descriptions, bool isTask1 = true)
    {
        Vector2 currentPos = map.First().Key;
        Vector2[] direction = new Vector2[4] { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };
        int dirIdx = 0;

        foreach(string desc in descriptions)
        {
            if (desc == "L")
                dirIdx = ((dirIdx - 1) % direction.Length + direction.Length) % direction.Length;
            else if (desc == "R")
                dirIdx = (dirIdx + 1) % direction.Length;
            else
            {
                int steps = int.Parse(desc);
                for(int i = 0; i < steps; i++)
                {
                    Vector2 nextPos = currentPos + direction[dirIdx];
                    int success = WalkOneStep(map, nextPos);
                    if (success == -1)
                    {
                        (nextPos, int newDirIdx) = NextPosWrap(map, currentPos, nextPos, dirIdx, isTask1);
                        if (WalkOneStepOtherSide(map, nextPos))
                        {
                            currentPos = nextPos;
                            dirIdx = newDirIdx;
                        }
                        else
                            break;
                    }
                    else if (success == 1)
                        currentPos = nextPos;
                    else if (success == 0)
                        break;
                }
            }
        }

        return (currentPos, dirIdx);
    }

    private static (Vector2, int) NextPosWrap(Dictionary<Vector2, Char> map, Vector2 currentPos, Vector2 posToWrap, int dirIdx, bool isTask1)
    {
        if (isTask1)
            return NextPosWrapTask1(map, currentPos, posToWrap, dirIdx);
        else
            return NextPosToWrapTask2(posToWrap, dirIdx);            
    }

    private static (Vector2, int) NextPosWrapTask1(Dictionary<Vector2, Char> map, Vector2 currentPos, Vector2 posToWrap, int dirIdx)
    {
        if (dirIdx == 0)
        {
            int X = (int) map.Where(m => m.Key.Y == currentPos.Y).Min(m => m.Key.X);
            posToWrap.X = X;
        }
        else if (dirIdx == 1)
        {
            int Y = (int) map.Where(m => m.Key.X == currentPos.X).Min(m => m.Key.Y);
            posToWrap.Y = Y;
        }
        else if (dirIdx == 2)
        {
            int X = (int) map.Where(m => m.Key.Y == currentPos.Y).Max(m => m.Key.X);
            posToWrap.X = X;
        }
        else if (dirIdx == 3)
        {
            int Y = (int) map.Where(m => m.Key.X == currentPos.X).Max(m => m.Key.Y);
            posToWrap.Y = Y;
        }

        return (posToWrap, dirIdx);
    }

    private static (Vector2, int) NextPosToWrapTask2(Vector2 posToWrap, int dirIdx)
    {
        return wrapMap[(posToWrap, dirIdx)];
    }

    private static int WalkOneStep(Dictionary<Vector2, Char> map, Vector2 nextPos)
    {
        if (map.ContainsKey(nextPos) && map[nextPos] == '.')
            return 1;
        else if (map.ContainsKey(nextPos) && map[nextPos] == '#')
            return 0;

        return -1;
    }

    private static bool WalkOneStepOtherSide(Dictionary<Vector2, Char> map, Vector2 nextPos)
    {
        if (map.ContainsKey(nextPos) && map[nextPos] == '.')
            return true;
        else if (map.ContainsKey(nextPos) && map[nextPos] == '#')
            return false;

        return false;
    }

    //  21
    //  3
    // 54
    // 6
    // > 0, v 1, < 2, ^ 3
    private static Dictionary<(Vector2, int), (Vector2, int)> CreateWrapMap()
    {
        Dictionary<(Vector2, int), (Vector2, int)> wrapMap = new Dictionary<(Vector2, int), (Vector2, int)>();

        for(int i = 1; i <= 50; i++)
        {
            wrapMap.Add((new Vector2(100 + i, 0), 3), (new Vector2(i, 200), 3)); // 1U --> 6D
            wrapMap.Add((new Vector2(151, i), 0), (new Vector2(100, 151 - i), 2)); // 1R --> 4R
            wrapMap.Add((new Vector2(100 + i, 51), 1), (new Vector2(100, 50 + i), 2)); // 1D --> 3R

            wrapMap.Add((new Vector2(50 + i, 0), 3), (new Vector2(1, 150 + i), 0)); // 2U --> 6L
            wrapMap.Add((new Vector2(50, i), 2), (new Vector2(1, 151 - i), 0)); // 2L --> 5L

            wrapMap.Add((new Vector2(101, 50 + i), 0), (new Vector2(100 + i, 50), 3)); // 3R --> 1D
            wrapMap.Add((new Vector2(50, 50 + i), 2), (new Vector2(i, 101), 1)); // 3L --> 5U

            wrapMap.Add((new Vector2(101, 100 + i), 0), (new Vector2(150, 51 - i), 2)); // 4R --> 1R
            wrapMap.Add((new Vector2(50 + i, 151), 1), (new Vector2(50, 150 + i), 2)); // 4D --> 6R

            wrapMap.Add((new Vector2(i, 100), 3), (new Vector2(51, 50 + i), 0)); // 5U --> 3L
            wrapMap.Add((new Vector2(0, 100 + i), 2), (new Vector2(51, 51 - i), 0)); // 5L --> 2L

            wrapMap.Add((new Vector2(51, 150 + i), 0), (new Vector2(50 + i, 150), 3)); // 6R --> 4D
            wrapMap.Add((new Vector2(i, 201), 1), (new Vector2(100 + i, 1), 1)); // 6D --> 1U
            wrapMap.Add((new Vector2(0, 150 + i), 2), (new Vector2(50 + i, 1), 1)); // 6L --> 2U
        }

        return wrapMap;
    }

    private static void PrintMap(Dictionary<Vector2, Char> map)
    {
        int maxY = (int) map.Keys.Max(k => k.Y);
        int maxX = (int) map.Keys.Max(k => k.X);

        for(int y = 1; y <= maxY; y++)
        {
            for(int x = 1; x <= maxX; x++)
            {
                Vector2 pos = new Vector2(x, y);
                if (map.ContainsKey(pos))
                    Console.Write(map[pos]);
                else
                    Console.Write(" ");
            }
            Console.WriteLine("");
        }
        Console.WriteLine("\n\n");
    }
}
