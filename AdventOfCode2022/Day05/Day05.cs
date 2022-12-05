namespace AdventOfCode2022.Day05;
internal class Day05
{
    const string inputPath = @"Day05/Input.txt";

    private static Dictionary<int, Stack<char>> supplyStacksTask1 = new Dictionary<int, Stack<char>>();
    private static Dictionary<int, Stack<char>> supplyStacksTask2 = new Dictionary<int, Stack<char>>();

    public static void Task1and2()
    {
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        int emptyLineIdx = lines.FindIndex(l => l.Trim().Length == 0);
        
        for (int i = emptyLineIdx - 2; i >= 0; i--)
        {
            ParseCargos(lines[i]);
        }

        for (int i = emptyLineIdx + 1; i < lines.Count; i++)
        {
            MoveCargos(lines[i]);
        }

        Console.Write($"Task 1: ");

        foreach (KeyValuePair<int, Stack<char>> entry in supplyStacksTask1)
        {
            Console.Write(entry.Value.Peek());
        }
        Console.WriteLine();

        Console.Write($"Task 2: ");

        foreach (KeyValuePair<int, Stack<char>> entry in supplyStacksTask2)
        {
            Console.Write(entry.Value.Peek());
        }
        Console.WriteLine();
    }

    private static void ParseCargos(string line)
    {
        for (int i = 0; i * 4 + 1 < line.Length; i++)
        {
            int charIdx = i * 4 + 1;
            if (line[charIdx] == ' ') continue;

            supplyStacksTask1.TryAdd(i + 1, new Stack<char>());
            supplyStacksTask1[i + 1].Push(line[charIdx]);

            supplyStacksTask2.TryAdd(i + 1, new Stack<char>());
            supplyStacksTask2[i + 1].Push(line[charIdx]);
        }
    }

    private static void MoveCargos(string line)
    {
        int[] operations = line.Split(' ').Where((item, index) => index % 2 != 0).Select(int.Parse).ToArray();

        Stack<char> tmp = new Stack<char>();

        for (int i = 0; i < operations[0]; i++)
        {
            supplyStacksTask1[operations[2]].Push(supplyStacksTask1[operations[1]].Pop());
            tmp.Push(supplyStacksTask2[operations[1]].Pop());
        }

        for (int i = 0; i < operations[0]; i++)
        {
            supplyStacksTask2[operations[2]].Push(tmp.Pop());
        }
    }
}
