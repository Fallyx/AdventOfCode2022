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
            string[] pairs = pair.Split(',');

            int[] ids1 = pairs[0].Split('-').Select(int.Parse).ToArray();
            int[] ids2 = pairs[1].Split('-').Select(int.Parse).ToArray();

            if ((ids1[0] <= ids2[0] && ids1[1] >= ids2[1]) || (ids2[0] <= ids1[0] && ids2[1] >= ids1[1])) fullyContains++;
            int overlapsCount = Math.Max(0, Math.Min(ids1[1], ids2[1]) - Math.Max(ids1[0], ids2[0]) + 1);
            overlaps += (overlapsCount > 0 ? 1 : 0 );
        }

        Console.WriteLine($"Task 1: {fullyContains}");
        Console.WriteLine($"Task 2: {overlaps}");
    }
}
