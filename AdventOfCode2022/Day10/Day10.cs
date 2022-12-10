namespace AdventOfCode2022.Day10;

internal class Day10
{
    const string inputPath = @"Day10/Input.txt";
    public static void Task1and2()
    {
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        int maxCycle = 240;
        List<int> signalStrengthCycles = new List<int> { 20, 60, 100, 140, 180, 220 };
        int linesIdx = 0;
        bool firstCycle = true;
        int X = 1;
        int signalStrength = 0;

        Console.WriteLine("Task 2:");

        for (int i = 1; i <= maxCycle; i++)
        {
            if (signalStrengthCycles.Contains(i))
                signalStrength += i * X;

            int pixel = (i - 1) % 40;
            if ( (X - 1) <= pixel && pixel <= (X + 1) )
                Console.Write("#");
            else
                Console.Write(" ");

            if (i % 40 == 0)
                Console.WriteLine();

            if (firstCycle && lines[linesIdx].StartsWith("addx"))
            {
                firstCycle = false;
                continue;
            }
            else if (!firstCycle && lines[linesIdx].StartsWith("addx"))
            {
                firstCycle = true;
                X += int.Parse(lines[linesIdx].Split(' ')[1]);
            }

            linesIdx++;
        }

        Console.WriteLine($"\nTask 1: {signalStrength}");
    }
}
