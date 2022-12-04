namespace AdventOfCode2022.Day04;

internal class Day04
{
    const string inputPath = @"Day04/Input.txt";

    public static void Task1and2()
    {
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        int fullyContains = 0;
        int overlaps = 0;

        foreach (string pair in lines)
        {
            int[] ids = pair.Split(new char[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            if ((ids[0] <= ids[2] && ids[1] >= ids[3]) || (ids[2] <= ids[0] && ids[3] >= ids[1])) fullyContains++;
            int overlapsCount = Math.Max(0, Math.Min(ids[1], ids[3]) - Math.Max(ids[0], ids[2]) + 1);
            overlaps += (overlapsCount > 0 ? 1 : 0 );
        }

        Console.WriteLine($"Task 1: {fullyContains}");
        Console.WriteLine($"Task 2: {overlaps}");
    }
}
