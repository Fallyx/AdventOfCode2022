using System.Text.RegularExpressions;

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
        MatchCollection cargoLines = Regex.Matches(line, @"(\s{3}|\[[A-Z]\])\s?");
        int i = 0;

        foreach(Match match in cargoLines) 
        {
            i++;
            if (match.Value.Trim().Length == 0) continue;

            supplyStacksTask1.TryAdd(i, new Stack<char>());
            supplyStacksTask1[i].Push(match.Value[1]);

            supplyStacksTask2.TryAdd(i, new Stack<char>());
            supplyStacksTask2[i].Push(match.Value[1]);
        }
    }

    private static void MoveCargos(string line)
    {
        int[] operations = Regex.Split(line, @"\D+").Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray();
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
