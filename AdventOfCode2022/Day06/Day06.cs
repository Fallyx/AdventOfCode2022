namespace AdventOfCode2022.Day06;
internal class Day06
{
    const string inputPath = @"Day06/Input.txt";

    public static void Task1and2()
    {
        String datastream = File.ReadLines(inputPath).First();
        int startMarkerIdx = -1;
        int msgMarkerIdx = -1;

        for (int i = 0; i < datastream.Length - 4; i++)
        {
            startMarkerIdx = (startMarkerIdx == -1 && datastream.Skip(i).Take(4).Distinct().Count() == 4 ? i + 4 : startMarkerIdx);
            msgMarkerIdx = (msgMarkerIdx == -1 && datastream.Skip(i).Take(14).Distinct().Count() == 14 ? i + 14 : msgMarkerIdx);
            if (startMarkerIdx >= 0 && msgMarkerIdx >= 0) break;
        }

        Console.WriteLine($"Task 1: {startMarkerIdx}");
        Console.WriteLine($"Task 2: {msgMarkerIdx}");
    }
}
