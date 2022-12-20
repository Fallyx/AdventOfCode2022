namespace AdventOfCode2022.Day20;

internal class Day20
{
    const string inputPath = @"Day20/Input.txt";
    public static void Task1()
    {
        List<string> lines = File.ReadAllLines(inputPath).ToList();
        List<Node> doubleLinkedList = PrepareDoubleLinkedList(lines);
        AddRightToLinkedList(doubleLinkedList, lines.Count - 1);
        Mixing(doubleLinkedList);

        Console.WriteLine($"Task 1: {CalculateGroveCoords(doubleLinkedList)}");
    }

    public static void Task2()
    {
        List<string> lines = File.ReadAllLines(inputPath).ToList();
        int decryptionKey = 811589153;
        List<Node> doubleLinkedList = PrepareDoubleLinkedList(lines, decryptionKey);
        AddRightToLinkedList(doubleLinkedList, lines.Count - 1);

        for(int i = 0; i < 10; i++)
        {
            Mixing(doubleLinkedList);
        }

        Console.WriteLine($"Task 2: {CalculateGroveCoords(doubleLinkedList)}");
    }

    private static List<Node> PrepareDoubleLinkedList(List<string> lines, int decryptionKey = 1)
    {
        List<Node> doubleLinkedList = new List<Node>();
        doubleLinkedList.Add(new Node(long.Parse(lines[0]) * decryptionKey));

        for(int i = 1; i < lines.Count; i++)
        {
            Node n = new Node(long.Parse(lines[i]) * decryptionKey);
            n.Left = doubleLinkedList.ElementAt(i - 1);
            doubleLinkedList.Add(n);
        }

        return doubleLinkedList;
    }

    private static void AddRightToLinkedList(List<Node> doubleLinkedList, int idxLastElement)
    {
        for (int i = 0; i < idxLastElement; i++)
        {
            Node current = doubleLinkedList.ElementAt(i);
            Node next = doubleLinkedList.ElementAt(i+1);
            current.Right = next;
        }

        Node first = doubleLinkedList.ElementAt(0);
        Node last = doubleLinkedList.ElementAt(idxLastElement);
        first.Left = last;
        last.Right = first;
    }

    private static void Mixing(List<Node> doubleLinkedList)
    {
        foreach(Node current in doubleLinkedList)
        {
            current.Left.Right = current.Right;
            current.Right.Left = current.Left;

            long steps = Math.Abs(current.Number) % (doubleLinkedList.Count - 1);

            if (current.Number < 0)
                MoveLeft(current, steps);
            else
                MoveRight(current, steps);
        }
    }

    private static long CalculateGroveCoords(List<Node> doubleLinkedList)
    {
        Node findNumber = doubleLinkedList.First(n => n.Number == 0); 
        long sumNumbers = 0;

        for (int i = 1; i <= 3; i++)
        {
            for (int m = 0; m < 1000; m++)
            {
                findNumber = findNumber.Right;
            }
            sumNumbers += findNumber.Number;
        }

        return sumNumbers;
    }

    private static void MoveLeft(Node current, long steps)
    {
        Node left = current.Left;
        Node right = current.Right;

        for(long i = 0; i < steps; i++)
        {
            left = left.Left;
            right = right.Left;
        }

        current.Left = left;
        current.Right = right;
        left.Right = current;
        right.Left = current;
    }

    private static void MoveRight(Node current, long steps)
    {
        Node left = current.Left;
        Node right = current.Right;

        for (long i = 0; i < steps; i++)
        {
            left = left.Right;
            right = right.Right;
        }

        current.Left = left;
        current.Right = right;
        left.Right = current;
        right.Left = current;
    }

    private static void PrintList(List<Node> doubleLinkedList)
    {
        Node n1 = doubleLinkedList.First();
        for (int i = 0; i < doubleLinkedList.Count; i++)
        {
            Console.Write($"{n1.Number}, ");
            n1 = n1.Right;
        }

        Console.WriteLine("\n");
    }

    private class Node
    {
        public long Number { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node(long number)
        {
            Number = number;
        }
    }
}
