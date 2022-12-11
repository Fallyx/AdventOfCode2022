namespace AdventOfCode2022.Day11;

internal class Day11
{
    const string inputPath = @"Day11/Input.txt";
    public static void Task1()
    {
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        List<Monkey> monkeysTask1 = new List<Monkey>();
        List<Monkey> monkeysTask2 = new List<Monkey>();

        for (int i = 0; i < lines.Count(); i += 7)
        {
            monkeysTask1.Add(ParseLines(lines.Skip(i).Take(6).ToList()));
            monkeysTask2.Add(ParseLines(lines.Skip(i).Take(6).ToList()));
        }

        long productOfAllDivisibleBy = monkeysTask1.Select(m => m.Divisible).Aggregate((l, r) => l * r);

        MonkeyRounds(monkeysTask1, 20, 3, productOfAllDivisibleBy);
        List<long> inspectedItems = monkeysTask1.Select(m => m.InspectedItems).OrderByDescending(i => i).ToList();
        Console.WriteLine($"Task 1: {inspectedItems[0] * inspectedItems[1]}");

        MonkeyRounds(monkeysTask2, 10000, 1, productOfAllDivisibleBy);
        List<long> inspectedItems2 = monkeysTask2.Select(m => m.InspectedItems).OrderByDescending(i => i).ToList();
        Console.WriteLine($"Task 2: {inspectedItems2[0] * inspectedItems2[1]}");
    }

    private static void MonkeyRounds(List<Monkey> monkeys, int maxRounds, long dividedBy, long productOfAllDivisibleBy)
    {
        for (int i = 0; i < maxRounds; i++)
        {
            foreach (Monkey monkey in monkeys)
            {
                foreach (long item in monkey.Items)
                {
                    long worryLevel = monkey.GetNewWorryLevel(item) / dividedBy;
                    worryLevel %= productOfAllDivisibleBy;
                    bool isDivisible = worryLevel % monkey.Divisible == 0;
                    if (isDivisible)
                        monkeys[monkey.IfTrue].Items.Add(worryLevel);
                    else
                        monkeys[monkey.IfFalse].Items.Add(worryLevel);
                }
                monkey.InspectedItems += monkey.Items.Count();
                monkey.Items.Clear();
            }
        }
    }

    private static Monkey ParseLines(List<string> lines)
    {
        string name = lines[0].Split(' ')[1].Substring(0, 1);
        List<long> items = lines[1].Substring(18).Split(", ").Select(long.Parse).ToList();
        bool isAddition = lines[2].Contains("+");
        string operation = lines[2].Split(' ').Last();
        long divisible = long.Parse(lines[3].Split(' ').Last());
        int ifTrue = int.Parse(lines[4].Split(' ').Last());
        int ifFalse = int.Parse(lines[5].Split(' ').Last());

        return new Monkey(name, items, isAddition, operation, divisible, ifTrue, ifFalse);
    }

    private class Monkey
    {
        public String Name { get; set; }
        public List<long> Items { get; set; }
        public bool IsAddition { get; set; }
        public String Operation { get; set; }
        public long Divisible { get; set; }
        public int IfTrue { get; set; }
        public int IfFalse { get; set; }
        public long InspectedItems { get; set; }

        public Monkey(string name, List<long> items, bool isAddition, string operation, long divisible, int ifTrue, int ifFalse)
        {
            Name = name;
            Items = items;
            IsAddition = isAddition;
            Operation = operation;
            Divisible = divisible;
            IfTrue = ifTrue;
            IfFalse = ifFalse;
            InspectedItems = 0;
        }

        public long GetNewWorryLevel(long item) 
        {
            if (Operation == "old") 
                return (IsAddition ? item + item : item * item);
            
            long opertionBy = long.Parse(Operation);
            return (IsAddition ? item + opertionBy : item * opertionBy);
        }
    }
}