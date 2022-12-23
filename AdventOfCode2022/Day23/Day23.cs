using System.Numerics;

namespace AdventOfCode2022.Day23;

internal class Day23
{
    const string inputPath = @"Day23/Input.txt";
    public static void Task1()
    {
        List<string> lines = File.ReadAllLines(inputPath).ToList();
        HashSet<Vector2> elves = new HashSet<Vector2>();

        for (int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#')
                    elves.Add(new Vector2(x, y));
            }
        }

        SimulateRounds(elves);
    }

    private static void SimulateRounds(HashSet<Vector2> elves)
    {
        int adjacentsIdx = 0;
        Vector2[] adjacents = new Vector2[12] {
            new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, -1),
            new Vector2(0, 1), new Vector2(1, 1), new Vector2(-1, 1),
            new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(-1, 1),
            new Vector2(1, 0), new Vector2(1, -1), new Vector2(1, 1)
        };
        Dictionary<Vector2, List<Vector2>> proposedMoves = new Dictionary<Vector2, List<Vector2>>();
        int rounds = 1;

        while(true)
        {
            foreach(Vector2 elf in elves)
            {
                if (!ElfNeedsToMove(elves, adjacents, elf))
                    continue;

                ProposedMove(elves, proposedMoves, adjacents, elf, adjacentsIdx);
            }

            if (proposedMoves.Count == 0)
            {
                Console.WriteLine($"Task 2: {rounds}");
                break;
            }

            foreach(KeyValuePair<Vector2, List<Vector2>> keyValuePair in proposedMoves)
            {
                if (keyValuePair.Value.Count > 1)
                    continue;
                    
                elves.Remove(keyValuePair.Value.First());
                elves.Add(keyValuePair.Key);
            }
            proposedMoves.Clear();
            adjacentsIdx = (adjacentsIdx + 3) % adjacents.Length;

            if (rounds == 10)
                Console.WriteLine($"Task 1: {CountEmptyTiles(elves)}");

            rounds++;
        }        
    }

    private static bool ElfNeedsToMove(HashSet<Vector2> elves, Vector2[] adjacents, Vector2 elf)
    {
        bool needsToMove = false;
        for (int adjI = 0; adjI < adjacents.Length; adjI++)
        {
            if (elves.Contains(elf + adjacents[adjI]))
            {
                needsToMove = true;
                break;
            }
        }

        return needsToMove;
    }

    private static void ProposedMove(HashSet<Vector2> elves, Dictionary<Vector2, List<Vector2>> proposedMoves, Vector2[] adjacents, Vector2 elf, int adjacentsIdx)
    {
        for (int dir = 0; dir < 12; dir+=3)
        {
            bool canMoveDir = true;
            for (int subDir = 0; subDir < 3; subDir++)
            {
                int idx = (adjacentsIdx + dir + subDir) % adjacents.Length;
                if (elves.Contains(elf + adjacents[idx]))
                {
                    canMoveDir = false;
                    break;
                }
            }

            if (canMoveDir)
            {
                Vector2 proposedPos = elf + adjacents[(adjacentsIdx + dir) % adjacents.Length];
                proposedMoves.TryAdd(proposedPos, new List<Vector2>());
                proposedMoves[proposedPos].Add(elf);
                break;
            }
        }
    }

    private static int CountEmptyTiles(HashSet<Vector2> elves)
    {
        int emptyTiles = 0;

        int minX = (int) elves.Min(e => e.X);
        int minY = (int) elves.Min(e => e.Y);
        int maxX = (int) elves.Max(e => e.X);
        int maxY = (int) elves.Max(e => e.Y);

        for(int y = minY; y <= maxY; y++)
        {
            for(int x = minX; x <= maxX; x++)
            {
                if(!elves.Contains(new Vector2(x, y)))
                    emptyTiles++;
            }
        }

        return emptyTiles;
    }

    private static void PrintMap(HashSet<Vector2> elves)
    {
        int minX = (int) elves.Min(e => e.X);
        int minY = (int) elves.Min(e => e.Y);
        int maxX = (int) elves.Max(e => e.X);
        int maxY = (int) elves.Max(e => e.Y);

        for(int y = minY; y <= maxY; y++)
        {
            for(int x = minX; x <= maxX; x++)
            {
                if(elves.Contains(new Vector2(x, y)))
                    Console.Write('#');
                else
                    Console.Write('.');
            }
            Console.WriteLine();
        }

        Console.WriteLine("\n");
    }
}
