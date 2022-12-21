namespace AdventOfCode2022.Day21;

internal class Day21
{
    const string inputPath = @"Day21/Input.txt";
    private static Dictionary<string, Monkey> monkeys = new Dictionary<string, Monkey>();
    public static void Task1and2()
    {
        List<string> lines = File.ReadAllLines(inputPath).ToList();

        foreach (string line in lines)
        {
            string[] l = line.Split(": ");
            monkeys.Add(l[0], new Monkey(l[1]));
        }

        Console.WriteLine($"Task 1: {monkeys["root"].Calculate()}");

        monkeys["root"].Op = Monkey.Operation.sub;
        for (int i = 1000000000; i > 0; i/= 10)
        {
            if(FindHumnNumber(i))
                break;
        }

        Console.WriteLine($"Task 2: {monkeys["humn"].Number}");
    }

    private static bool FindHumnNumber(int inc)
    {
        long res = monkeys["root"].Calculate();
        while (res > 0)
        {
            monkeys["humn"].Number += inc;
            res = monkeys["root"].Calculate();
        }

        if (res < 0)
            monkeys["humn"].Number -= inc;

        return res == 0;
    }

    private class Monkey
    {
        public enum Operation
        {
            add,
            sub,
            mult,
            div,
            number
        }

        public Operation Op { get; set; }
        public String Left { get; set; }
        public String Right { get; set; }
        public Int64 Number { get; set; }

        public Monkey(string line)
        {
            long value;

            if (long.TryParse(line, out value))
            {
                Number = value;
                Op = Operation.number;
            }
            else
            {
                string[] ops = line.Split(' ');
                Left = ops[0];
                Right = ops[2];

                if (ops[1] == "+")
                    Op = Operation.add;
                else if (ops[1] == "-")
                    Op = Operation.sub;
                else if (ops[1] == "*")
                    Op = Operation.mult;
                else if (ops[1] == "/")
                    Op = Operation.div;
            }
        }

        public long Calculate()
        {
            switch(Op)
            {
                case Operation.number:
                    return Number;
                case Operation.add:
                    return monkeys[Left].Calculate() + monkeys[Right].Calculate();
                case Operation.sub:
                    return monkeys[Left].Calculate() - monkeys[Right].Calculate();
                case Operation.mult:
                    return monkeys[Left].Calculate() * monkeys[Right].Calculate();
                case Operation.div:
                    return monkeys[Left].Calculate() / monkeys[Right].Calculate();
            }

            return int.MinValue;
        }
    }
}
