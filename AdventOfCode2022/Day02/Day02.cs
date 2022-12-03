namespace AdventOfCode2022.Day02;
internal class Day02
{
    const string inputPath = @"Day02/Input.txt";
    private static Dictionary<string, int> outcomes = new Dictionary<string, int>()
    {
        { "AX", 3 }, { "AY", 1 + 3 }, { "AZ", 2 + 6 }, { "BX", 1 }, { "BY", 2 + 3 }, { "BZ", 3 + 6 }, { "CX", 2 }, { "CY", 3 + 3 }, { "CZ", 1 + 6 }
    };

    public static void Task1and2()
    {
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        int pointsTask1 = 0;
        int pointsTask2 = 0;

        foreach (String line in lines)
        {
            String rpc = line.Replace(" ", "");

            pointsTask1 += PlayTask1(rpc);
            pointsTask2 += PlayTask2(rpc);
        }

        Console.WriteLine($"Task 1: {pointsTask1}");
        Console.WriteLine($"Task 2: {pointsTask2}");
    }

    private static int PlayTask1(string rpc)
    {
        int score = 0;
        if (rpc[1] == 'X') score += 1;
        else if (rpc[1] == 'Y') score += 2;
        else if (rpc[1] == 'Z') score += 3;

        if (rpc == "AX" || rpc == "BY" || rpc == "CZ") score += 3;
        else if (rpc == "CX" || rpc == "AY" || rpc == "BZ") score += 6;

        return score;
    }

    private static int PlayTask2(string rpc)
    {
        return outcomes[rpc];
    }
}
