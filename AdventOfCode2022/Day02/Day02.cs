namespace AdventOfCode2022.Day02;
internal class Day02
{
    const string inputPath = @"Day02/Input.txt";

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
        int score = 0;

        if (rpc == "AX") score += 3;
        else if (rpc == "AY") score += 1 + 3;
        else if (rpc == "AZ") score += 2 + 6;
        else if (rpc == "BX") score += 1;
        else if (rpc == "BY") score += 2 + 3;
        else if (rpc == "BZ") score += 3 + 6;
        else if (rpc == "CX") score += 2;
        else if (rpc == "CY") score += 3 + 3;
        else if (rpc == "CZ") score += 1 + 6;

        return score;
    }
}
