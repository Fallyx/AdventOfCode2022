using System.Numerics;
namespace AdventOfCode2022.Day18;

internal class Day18
{
    const string inputPath = @"Day18/Input.txt";
    private static readonly Vector3[] adjacents = 
    {
        new Vector3( 1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3( 0, 1, 0),
        new Vector3( 0,-1, 0),
        new Vector3( 0, 0, 1),
        new Vector3( 0, 0,-1)
    };
    public static void Task1and2()
    {
        List<string> lines = File.ReadAllLines(inputPath).ToList();
        List<Vector3> cubes = new List<Vector3>();

        foreach (string line in lines)
        {
            int[] coords = line.Split(',').Select(int.Parse).ToArray();
            cubes.Add(new Vector3(coords[0], coords[1], coords[2]));
        }

        int exposedSurfaces = 0;
        foreach (Vector3 cube in cubes)
        {
            int cubeExposedSurfaces = 6;
            for (int i = 0; i < adjacents.Length; i++)
            {
                Vector3 otherCube = cube + adjacents[i];
                if (cubes.Contains(otherCube))
                    cubeExposedSurfaces--;
            }
            exposedSurfaces += cubeExposedSurfaces;
        }

        Console.WriteLine($"Task 1: {exposedSurfaces}");
        Console.WriteLine($"Task 2: {OutterExposedSurfaces(cubes)}");
    }

    private static int OutterExposedSurfaces(List<Vector3> cubes)
    {
        int exposedSurfaces = 0;
        HashSet<Vector3> visited = new HashSet<Vector3>();
        Queue<Vector3> queue = new Queue<Vector3>();
        int maxX = (int) cubes.Max(c => c.X) + 1;
        int minX = (int) cubes.Min(c => c.X) - 1;
        int maxY = (int) cubes.Max(c => c.Y) + 1;
        int minY = (int) cubes.Min(c => c.Y) - 1;
        int maxZ = (int) cubes.Max(c => c.Z) + 1;
        int minZ = (int) cubes.Min(c => c.Z) - 1;
        queue.Enqueue(new Vector3(minX, minY, minZ));

        while (queue.Count > 0)
        {
            Vector3 currentPos = queue.Dequeue();
            for (int i = 0; i < adjacents.Length; i++)
            {
                Vector3 otherPos = currentPos + adjacents[i];
                if (otherPos.X < minX || otherPos.X > maxX
                    || otherPos.Y < minY || otherPos.Y > maxY
                    || otherPos.Z < minZ || otherPos.Z > maxZ)
                    continue;

                if (cubes.Contains(otherPos))
                    exposedSurfaces++;
                else if (visited.Add(otherPos))
                    queue.Enqueue(otherPos);
            }
        }

        return exposedSurfaces;
    }
}
