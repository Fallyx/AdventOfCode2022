using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day19;

internal class Day19
{
    const string inputPath = @"Day19/Input.txt";
    public static void Task1()
    {
        List<string> lines = File.ReadAllLines(inputPath).ToList();
        string pattern = @"Blueprint (\d+): Each ore robot costs (\d+) ore\. Each clay robot costs (\d+) ore\. Each obsidian robot costs (\d+) ore and (\d+) clay\. Each geode robot costs (\d+) ore and (\d+) obsidian\.";
        int qualityLevel = 0;
        int geodesGathered = 1;

        foreach(string line in lines)
        {
            Vector4[] robotCost = new Vector4[4];
            foreach(Match m in Regex.Matches(line, pattern))
            {
                int idx = int.Parse(m.Groups[1].Value);
                robotCost[0] = new Vector4(int.Parse(m.Groups[2].Value), 0, 0, 0);
                robotCost[1] = new Vector4 (int.Parse(m.Groups[3].Value), 0, 0, 0);
                robotCost[2] = new Vector4 (int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value), 0, 0);
                robotCost[3] = new Vector4 (int.Parse(m.Groups[6].Value), 0 , int.Parse(m.Groups[7].Value), 0);

                qualityLevel += idx * HighestGeode(robotCost, 24);
            }
        }

        Console.WriteLine($"Task 1: {qualityLevel}");

        for(int i = 0; i < 3; i++)
        {
            Vector4[] robotCost = new Vector4[4];
            foreach(Match m in Regex.Matches(lines[i], pattern))
            {
                int idx = int.Parse(m.Groups[1].Value);
                robotCost[0] = new Vector4(int.Parse(m.Groups[2].Value), 0, 0, 0);
                robotCost[1] = new Vector4 (int.Parse(m.Groups[3].Value), 0, 0, 0);
                robotCost[2] = new Vector4 (int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value), 0, 0);
                robotCost[3] = new Vector4 (int.Parse(m.Groups[6].Value), 0 , int.Parse(m.Groups[7].Value), 0);

                geodesGathered *= HighestGeode(robotCost, 32);;
            }
        }

        Console.WriteLine($"Task 2: {geodesGathered}");
    }

    private static int HighestGeode(Vector4[] robotCost, int maxMins)
    {
        int maxGeode = 0;
        int maxOreBots = (int) robotCost.Select(v => v.X).Max();
        int maxClayBots = (int) robotCost.Select(v => v.Y).Max();
        int maxObsidianBots = (int) robotCost.Select(v => v.Z).Max();

        // First Vec4 is resources, second Vec4 is robos
        HashSet<(Vector4, Vector4)> visited = new HashSet<(Vector4, Vector4)>();
        Queue<(Vector4 resources, Vector4 robots, int minute)> queue = new Queue<(Vector4 resources, Vector4 robots, int minute)>();
        queue.Enqueue((new Vector4(2, 0, 0, 0), new Vector4(1, 0, 0, 0), 2));

        while (queue.Count > 0)
        {
            (Vector4 resources, Vector4 robots, int minute) currentStep = queue.Dequeue();
            if (currentStep.minute > maxMins)
                continue;

            if (!visited.Add((currentStep.resources, currentStep.robots)))
                continue;

            Vector4 collected = currentStep.robots;
            maxGeode = Math.Max(maxGeode, (int) currentStep.resources.W);
            if (currentStep.minute + 1 <= maxMins && currentStep.resources.X >= robotCost[3].X && currentStep.resources.Z >= robotCost[3].Z)
            {
                Vector4 newRobots = currentStep.robots;
                newRobots.W++;
                Vector4 newResources = currentStep.resources - robotCost[3];
                queue.Enqueue((newResources + collected, newRobots, currentStep.minute + 1));
                continue;
            }
            
            if (currentStep.minute + 1 <= maxMins && currentStep.robots.Z + 1 <= maxObsidianBots && currentStep.resources.X >= robotCost[2].X && currentStep.resources.Y >= robotCost[2].Y)
            {
                Vector4 newRobots = currentStep.robots;
                newRobots.Z++;
                Vector4 newResources = currentStep.resources - robotCost[2];
                queue.Enqueue((newResources + collected, newRobots, currentStep.minute + 1));
            }

            if (currentStep.minute + 1 <= maxMins && currentStep.robots.Y + 1 <= maxClayBots && currentStep.resources.X >= robotCost[1].X)
            {
                Vector4 newRobots = currentStep.robots;
                newRobots.Y++;
                Vector4 newResources = currentStep.resources - robotCost[1];
                queue.Enqueue((newResources + collected, newRobots, currentStep.minute + 1));
            }

            if (currentStep.minute + 1 <= maxMins && currentStep.robots.X + 1 <= maxOreBots && currentStep.resources.X >= robotCost[0].X)
            {
                Vector4 newRobots = currentStep.robots;
                newRobots.X++;
                Vector4 newResources = currentStep.resources - robotCost[0];
                queue.Enqueue((newResources + collected, newRobots, currentStep.minute + 1));
            }

            if (currentStep.minute + 1 <= maxMins)
                queue.Enqueue((currentStep.resources + collected, currentStep.robots, currentStep.minute + 1));
        }

        return maxGeode;
    }
}
