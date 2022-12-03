namespace AdventOfCode2022.Day03;

internal class Day03
{
    const string inputPath = @"Day03/Input.txt";

    public static void Task1()
    {
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        int sumPrio = 0;

        foreach(String line in lines)
        {
            string compartment1 = line.Substring(0, line.Length / 2);
            string compartment2 = line.Substring(line.Length / 2);

            List<char> commons = compartment1.Intersect(compartment2).ToList();

            sumPrio += CalculatePrio(commons);
        }

        Console.WriteLine($"Task 1: {sumPrio}");
    }

    public static void Task2()
    {
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        int sumPrio = 0;

        for (int i = 0; i < lines.Count; i+= 3) 
        {
            string rucksack1 = lines[i];
            string rucksack2 = lines[i+1];
            string rucksack3 = lines[i+2];

            List<char> commons = rucksack1.Intersect(rucksack2).Intersect(rucksack3).ToList();

            sumPrio += CalculatePrio(commons);
        }

        Console.WriteLine($"Task 2: {sumPrio}");
    }

    private static int CalculatePrio(List<char> commons)
    {
        int sumPrio = 0;

        foreach(char c in commons)
        {
            if (c >= 'a' && c <= 'z') sumPrio += c - 96;
            else if (c >= 'A' && c <= 'Z') sumPrio += c - 64 + 26;
        }

        return sumPrio;
    }
}
