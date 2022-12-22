using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day22;

internal class Day22
{
    const string inputPath = @"Day22/Input.txt";
    public static void Task1()
    {
        Dictionary<Vector2, Char> map = new Dictionary<Vector2, char>();
        List<string> lines = File.ReadAllLines(inputPath).ToList();
        List<string> descriptions = new List<string>();
        string pattern = @"(\d+)([LR])?";
        Vector2[] direction = new Vector2[4] { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };
        int dirIdx = 0;

        for (int linesIdx = 0; linesIdx < lines.Count; linesIdx++)
        {
            if (string.IsNullOrEmpty(lines[linesIdx]))
                break;

            for(int i = 0; i < lines[linesIdx].Length; i++)
            {
                if (lines[linesIdx][i] != ' ')
                    map.Add(new Vector2(i, linesIdx), lines[linesIdx][i]);
            }
        }

        foreach (Match m in Regex.Matches(lines.Last(), pattern))
        {
            descriptions.Add(m.Groups[1].Value);
            descriptions.Add(m.Groups[2].Value);
        }

        descriptions.RemoveAt(descriptions.Count - 1);

        Vector2 currentPos = map.First().Key;

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

                    bool? success = WalkOneStep(map, nextPos);
                    if (success == null)
                    {
                        if (dirIdx == 0)
                        {
                            int X = (int) map.Where(m => m.Key.Y == currentPos.Y).Min(m => m.Key.X);
                            nextPos.X = X;
                            bool? success2 = WalkOneStep(map, nextPos);
                            if (success2! == true)
                                currentPos = nextPos;
                            else if (success2! == false)
                                break;
                        }
                        else if (dirIdx == 2)
                        {
                            int X = (int) map.Where(m => m.Key.Y == currentPos.Y).Max(m => m.Key.X);
                            nextPos.X = X;
                            bool? success2 = WalkOneStep(map, nextPos);
                            if (success2! == true)
                                currentPos = nextPos;
                            else if (success2! == false)
                                break;
                        }
                        else if (dirIdx == 1)
                        {
                            int Y = (int) map.Where(m => m.Key.X == currentPos.X).Min(m => m.Key.Y);
                            nextPos.Y = Y;
                            bool? success2 = WalkOneStep(map, nextPos);
                            if (success2! == true)
                                currentPos = nextPos;
                            else if (success2! == false)
                                break;
                        }
                        else if (dirIdx == 3)
                        {
                            int Y = (int) map.Where(m => m.Key.X == currentPos.X).Max(m => m.Key.Y);
                            nextPos.Y = Y;
                            bool? success2 = WalkOneStep(map, nextPos);
                            if (success2! == true)
                                currentPos = nextPos;
                            else if (success2! == false)
                                break;
                        }
                    }
                    else if (success! == true)
                        currentPos = nextPos;
                    else if (success! == false)
                        break;
                }
            }
        }

        Console.WriteLine($"Task 1: {(currentPos.Y + 1) * 1000 + (currentPos.X + 1) * 4 + dirIdx}");
    }

    private static bool? WalkOneStep(Dictionary<Vector2, Char> map, Vector2 nextPos)
    {
        if (map.ContainsKey(nextPos) && map[nextPos] == '.')
            return true;
        else if (map.ContainsKey(nextPos) && map[nextPos] == '#')
            return false;

        return null;
    }

    private static void PrintMap(Dictionary<Vector2, Char> map)
    {
        int maxY = (int) map.Keys.Max(k => k.Y);
        int maxX = (int) map.Keys.Max(k => k.X);

        for(int y = 0; y <= maxY; y++)
        {
            for(int x = 0; x <= maxX; x++)
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
