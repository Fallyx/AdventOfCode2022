namespace AdventOfCode2022.Day08;

internal class Day08
{
    const string inputPath = @"Day08/Input.txt";

    public static void Task1()
    {
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        int countVisible = 0;

        int[,] forest = new int[lines[0].Length, lines.Count];

        for (int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (x == 0 || x == lines[y].Length - 1 || y == 0 || y == lines.Count - 1)
                    countVisible++;

                forest[x, y] = int.Parse(lines[y][x].ToString());
            }
        }

        (int countVisibility, int maxScenicScore) result = CalculateVisibilityAndMaxScenicScore(forest);
        Console.WriteLine($"Task 1: {result.countVisibility + countVisible}");
        Console.WriteLine($"Task 2: {result.maxScenicScore}");
    }

    private static (int countVisibility, int maxScenicScore) CalculateVisibilityAndMaxScenicScore(int[,] forest)
    {
        int countVisibility = 0;
        int maxScenicScore = 0;

        for (int y = 1; y < forest.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < forest.GetLength(0) - 1; x++)
            {
                int currentHeight = forest[x, y];
                int[] rows = GetRow(forest, y);
                int[] cols = GetColumn(forest, x);

                if (rows.Take(x).All(r => r < currentHeight)
                    || rows.Skip(x + 1).All(r => r < currentHeight)
                    || cols.Take(y).All(c => c < currentHeight)
                    || cols.Skip(y + 1).All(c => c < currentHeight))
                    countVisibility++;

                int left = GetScenicScoreDirection(currentHeight, rows.Take(x).Reverse().ToArray());
                int right = GetScenicScoreDirection(currentHeight, rows.Skip(x + 1).ToArray());

                int up = GetScenicScoreDirection(currentHeight, cols.Take(y).Reverse().ToArray());
                int down = GetScenicScoreDirection(currentHeight, cols.Skip(y + 1).ToArray());

                maxScenicScore = Math.Max(maxScenicScore, left * right * up * down);
            }
        }

        return (countVisibility, maxScenicScore);
    }

    private static int[] GetRow(int[,] forest, int y)
    {
        return Enumerable.Range(0, forest.GetLength(0))
            .Select(f => forest[f, y]).ToArray();
    }

    private static int[] GetColumn(int[,] forest, int x)
    {
        return Enumerable.Range(0, forest.GetLength(1))
            .Select(f => forest[x, f]).ToArray();
    }

    private static int GetScenicScoreDirection(int currentHeight, int[] direction)
    {
        int score = 0;
        for (int i = 0; i < direction.Length; i++)
        {
            score++;
            if (direction[i] >= currentHeight) break;
        }
        return score;
    }
}
