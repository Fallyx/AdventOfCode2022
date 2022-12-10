using System.Numerics;

namespace AdventOfCode2022.Day09;
internal class Day09
{
    const string inputPath = @"Day09/Input.txt";

    public static void Task1and2()
    {
        Console.WriteLine($"Task 1: {Ropes(2)}");
        Console.WriteLine($"Task 2: {Ropes(10)}");
    }

    private static int Ropes(int ropeLength)
    {
        Dictionary<String, Vector2> moveDirections = new Dictionary<string, Vector2>()
        {
            { "U", new Vector2(0, 1) },
            { "D", new Vector2(0, -1) },
            { "L", new Vector2(-1, 0) },
            { "R", new Vector2(1, 0) }
        };
        HashSet<Vector2> visitedTail = new HashSet<Vector2> { new Vector2() };
        Vector2[] ropes = new Vector2[ropeLength];

        List<String> lines = File.ReadAllLines(inputPath).ToList();

        foreach (String line in lines)
        {
            string[] move = line.Split(' ');
            Vector2 moveDirection = moveDirections[move[0]];
            int steps = int.Parse(move[1]);
            
            for(int i = 0; i < steps; i++)
            {
                ropes[0] += moveDirection;

                for(int j = 1; j < ropes.Length; j++)
                {
                    ropes[j] += MoveTail(ropes[j - 1], ropes[j]);
                }

                visitedTail.Add(ropes[ropes.Length - 1]);
            }
        }

        return visitedTail.Count;
    }

    private static Vector2 MoveTail(Vector2 head, Vector2 tail )
    {
        Vector2 move = new Vector2();
        float xDisteance = head.X - tail.X;
        float yDistance = head.Y - tail.Y;

        if (xDisteance <= 1 && xDisteance >= -1 && yDistance <= 1 && yDistance >= -1)
            return move;

        move.X = Math.Clamp(xDisteance, -1, 1);
        move.Y = Math.Clamp(yDistance, -1, 1);

        return move;
    }
}
