using System.Text.RegularExpressions;
namespace AdventOfCode2022.Day16;

internal class Day16
{
    const string inputPath = @"Day16/Input.txt";
    public static void Task1()
    {
        List<string> lines = File.ReadAllLines(inputPath).ToList();
        string pattern = @"Valve (\w\w) has flow rate=(\d+); tunnels? leads? to valves? (.*)";
        Dictionary<string, Valve> valvesMap = new Dictionary<string, Valve>();
        HashSet<string> notVisitedValves = new HashSet<string>();
        Dictionary<string, int> paths = new Dictionary<string, int>();

        foreach (string line in lines)
        {
            foreach(Match m in Regex.Matches(line, pattern))
            {
                Valve v = new Valve(m.Groups[1].Value, int.Parse(m.Groups[2].Value));
                v.TunnelPaths = m.Groups[3].Value.Split(", ").ToList();
                valvesMap.TryAdd(v.Name, v);
                if (v.FlowRate != 0) notVisitedValves.Add(m.Groups[1].Value);
            }
        }

        foreach (KeyValuePair<string, Valve> from in valvesMap)
        {
            foreach (KeyValuePair<string, Valve> to in valvesMap)
            {
                if (from.Key == to.Key || to.Value.FlowRate == 0 || from.Value.FlowRate == 0 && from.Key != "AA")
                    continue;
                paths.Add(from.Key + " " + to.Key, ShortestPath(valvesMap, from.Value, to.Value););
            }
        }

        int maxPressureReleased = FindMostPressureRelease(paths, valvesMap, notVisitedValves, "AA", 30);
        Console.WriteLine($"Task 1: {maxPressureReleased}");

        IEnumerable<string>[] combinations = GetKCombs(notVisitedValves, notVisitedValves.Count / 2).ToArray();
        maxPressureReleased = 0;
        for (int i = 0; i < combinations.Count(); i++)
        {
            IEnumerable<string> elefantValves = notVisitedValves.Except(combinations[i]);
            int maxPressureReleasedMe = FindMostPressureRelease(paths, valvesMap, combinations[i].ToHashSet(), "AA", 26);
            int maxPressureReleasedElefant = FindMostPressureRelease(paths, valvesMap, elefantValves.ToHashSet(), "AA", 26);
            maxPressureReleased = Math.Max(maxPressureReleased, maxPressureReleasedMe + maxPressureReleasedElefant);
        }

        Console.WriteLine($"Task 2: {maxPressureReleased}");
    }

    private static int ShortestPath(Dictionary<string, Valve> valves, Valve start, Valve end)
    {
        Queue<(Valve v, List<string> visited, int distance)> queue = new Queue<(Valve v, List<string> visited, int distance)>();
        queue.Enqueue((valves[start.Name], new List<string>(), 0));

        while (queue.Count > 0)
        {
            (Valve currentValve, List<string> visited, int distance) = queue.Dequeue();

            if (currentValve.Name == end.Name)
                return distance;
            
            foreach(string valve in currentValve.TunnelPaths.Where(v => !visited.Contains(v)))
            {
                Valve nextValve = valves[valve];
                visited.Add(valve);
                queue.Enqueue((nextValve, visited, distance + 1));
            }
        }

        return int.MaxValue;
    }

    private static int FindMostPressureRelease(Dictionary<string, int> paths, Dictionary<string, Valve> valves, HashSet<string> notVisitedValves, string currentValve, int minutesLeft)
    {
        if (minutesLeft <= 0)
            return 0;

        int pressureRelease = valves[currentValve].FlowRate * minutesLeft;
        HashSet<string> newNotVisitedValves = notVisitedValves.Select(v => v).Where(v => v != currentValve).ToHashSet();
        int pressureReleaseChilds = 0;
        foreach (string next in newNotVisitedValves)
        {
            int timeToNext = paths[currentValve + " " + next] + 1;
            if (timeToNext < minutesLeft)
            {
                int result = FindMostPressureRelease(paths, valves, newNotVisitedValves, next, minutesLeft - timeToNext);
                pressureReleaseChilds = Math.Max(pressureReleaseChilds, result);
            }
        }

        return pressureRelease + pressureReleaseChilds;
    }

    static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
    {
        if (length == 1) return list.Select(t => new T[] { t });
        return GetKCombs(list, length - 1)
            .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0), 
                (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    private class Valve
    {
        public String Name { get; set; }
        public Int32 FlowRate { get; set; }
        public List<String> TunnelPaths { get; set; }

        public Valve(string name, int flowRate)
        {
            Name = name;
            FlowRate = flowRate;
            TunnelPaths = new List<String>();
        }
    }
}
