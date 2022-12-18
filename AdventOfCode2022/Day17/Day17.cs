using System.Numerics;
namespace AdventOfCode2022.Day17;

internal class Day17
{
    const string inputPath = @"Day17/Input.txt";

    public static void Task1()
    {
        string jetPattern = File.ReadAllLines(inputPath).First();
        int[,] shape1 = new int[4,1] { { 1 }, { 1 }, { 1 }, { 1 } };
        int[,] shape2 = new int[3,3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
        int[,] shape3 = new int[3,3] { { 0, 0, 1 }, { 0, 0, 1 }, { 1, 1, 1 } };
        int[,] shape4 = new int[1,4] { { 1, 1, 1, 1 } };
        int[,] shape5 = new int[2,2] { { 1, 1 }, { 1, 1 } };

        List<int[,]> shapes = new List<int[,]>();
        shapes.Add(shape1);
        shapes.Add(shape2);
        shapes.Add(shape3);
        shapes.Add(shape4);
        shapes.Add(shape5);

        (int height, _, _) = PlayTetris(jetPattern, shapes, 2022);

        Console.WriteLine($"Task 1: {height}");

        (_, List<Pattern> patterns, int cycleIdx) = PlayTetris(jetPattern, shapes, 15000);

    //    var aaaaa = FindPattern(patterns);

        long elefantRockStops = 1000000000000L;

        int heightBeforePattern = patterns.Take(cycleIdx).Select(p => p.HeightDiff).Sum();
        int heightPattern = patterns.Skip(cycleIdx).Select(p => p.HeightDiff).Sum();
        int rocksPlayedCycle = patterns.Skip(cycleIdx).Count();
        int rocksPlayedBeforeCycle = patterns.Take(cycleIdx).Count();

        elefantRockStops -= rocksPlayedBeforeCycle;
        long remaining = elefantRockStops % rocksPlayedCycle;
        long abc = elefantRockStops / rocksPlayedCycle;

        long asdf = patterns.Skip(cycleIdx).Take((int)remaining).Select(p => p.HeightDiff).Sum();

        long result = heightBeforePattern + heightPattern * abc + asdf;
        Console.WriteLine($"Task 2: {result}");
    }

    private static (int height, List<Pattern> patterns, int cycleIdx) PlayTetris(string jetPattern, List<int[,]> shapes, int loopCount)
    {
        HashSet<Vector2> chamber = new HashSet<Vector2>();
        List<Pattern> patterns = new List<Pattern>();
        patterns.Add(new Pattern(0, 0, 0));
        int cycleIdx = 0;
        bool cycleFound = false;
        int currentHeight = 0;
        int jetIdx = 0;
        int shapeIdx = 0;

        for (int i = 0; i < loopCount; i++)
        {
            int shapeStartingPosY = currentHeight + 3 + shapes[shapeIdx].GetLength(1) - 1;
            Vector2 nextPos = new Vector2(2, shapeStartingPosY);
            while(true)
            {
                int direction = jetPattern[jetIdx] == '<' ? -1 : 1;
                // Jet movement
                nextPos.X += direction;
                if (HasCollision(chamber, shapes[shapeIdx], nextPos))
                {
                    nextPos.X -= direction;
                }

                // Fall movement
                nextPos.Y--;
                if (HasCollision(chamber, shapes[shapeIdx], nextPos)) 
                {
                    nextPos.Y++;
                    for(int y = 0; y < shapes[shapeIdx].GetLength(1); y++)
                    {
                        for(int x = 0; x < shapes[shapeIdx].GetLength(0); x++)
                        {
                            if (shapes[shapeIdx][x,y] == 0)
                                continue;

                            Vector2 rockPos = new Vector2(nextPos.X + x, nextPos.Y - y);
                            chamber.Add(rockPos);
                        }
                    }
                    jetIdx = (jetIdx + 1) % jetPattern.Length;
                    break;
                }

                jetIdx = (jetIdx + 1) % jetPattern.Length;
            }

        //    PrintChamber(chamber);

            int newHeight = (int) chamber.Select(v => v.Y).Max() + 1;
            //patterns.Add(new Pattern(newHeight - currentHeight, jetIdx, shapeIdx));
            
            if (!cycleFound && patterns.FindIndex(p => p.HeightDiff == newHeight - currentHeight && p.JetIdx == jetIdx && p.ShapeIdx == shapeIdx) < 0)
            {
                patterns.Add(new Pattern(newHeight - currentHeight, jetIdx, shapeIdx));
            }
            else if (!cycleFound)
            {
                cycleFound = true;
                cycleIdx = patterns.FindIndex(p => p.HeightDiff == newHeight - currentHeight && p.JetIdx == jetIdx && p.ShapeIdx == shapeIdx);
            }

            currentHeight = newHeight;
            shapeIdx = (shapeIdx + 1) % shapes.Count;
        }

        return (currentHeight, patterns, cycleIdx);
    }

    private static bool HasCollision(HashSet<Vector2> chamber, int[,] shape, Vector2 nextPos)
    {
        int chamberWidth = 7;

        if (nextPos.X < 0 || nextPos.Y < 0 || nextPos.X + shape.GetLength(0) > chamberWidth)
            return true;

        for(int y = 0; y < shape.GetLength(1); y++)
        {
            for(int x = 0; x < shape.GetLength(0); x++)
            {
                if (shape[x,y] == 0)
                    continue;
                
                Vector2 rockPos = new Vector2(nextPos.X + x, nextPos.Y - y);
                if (chamber.Contains(rockPos))
                    return true;
            }
        }

        return false;
    }

    /*
    private static int FindPattern(List<Pattern> patterns, int minLength = 10)
    {
        for(int i = 0; i < patterns.Count - 2 * minLength; i++)
        {
            bool patternFound = true;
            for (int j = 0; j <= patterns.Count; j++)
            {
                if (i == 14)
                {
                    Console.Write("");
                }

                Pattern p1 = patterns[i + j];
                Pattern p2 = patterns[i + j + minLength];

                if (p1.HeightDiff != p2.HeightDiff)
                {
                    patternFound = false;
                    break;
                }
            }

            if (patternFound)
                return i;
        }

        return -1;
    }
    */

    private static void PrintChamber(HashSet<Vector2> chamber)
    {
        int maxY = (int) chamber.Select(v => v.Y).Max();
        for(int y = maxY; y >= 0; y--)
        {
            for (int x = 0; x < 7; x++)
            {
                if (chamber.Contains(new Vector2(x, y)))
                    Console.Write("#");
                else
                    Console.Write(".");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\n");
    }

    private class Pattern
    {
        public int HeightDiff { get; set; }
        public int JetIdx { get; set; }
        public int ShapeIdx { get; set; }

        public Pattern(int heightDiff, int jetIdx, int shapeIdx)
        {
            HeightDiff = heightDiff;
            JetIdx = jetIdx;
            ShapeIdx = shapeIdx;
        }

        public int Score()
        {
            return HeightDiff + JetIdx + ShapeIdx;
        }
    }
}
