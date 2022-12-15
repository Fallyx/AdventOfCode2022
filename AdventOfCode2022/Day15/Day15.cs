using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day15;

internal class Day15
{
    const string inputPath = @"Day15/Input.txt";

    public static void Task1and2()
    {
        List<String> lines = File.ReadAllLines(inputPath).ToList();
        HashSet<Vector2> beacons = new HashSet<Vector2>();
        Dictionary<Vector2, int> sensors = new Dictionary<Vector2, int>();
        string coords = @"(x=-?\d+), (y=-?\d+).*(x=-?\d+), (y=-?\d+)";
        int minX = int.MaxValue;
        int maxX = int.MinValue;

        foreach(string line in lines)
        {
            foreach(Match m in Regex.Matches(line, coords))
            {
                int sX = CoordStringToInt(m.Groups[1].Value);
                int sY = CoordStringToInt(m.Groups[2].Value);
                int bX = CoordStringToInt(m.Groups[3].Value);
                int bY = CoordStringToInt(m.Groups[4].Value);

                Vector2 sensorCoord = new Vector2(sX, sY);
                Vector2 beaconCoord = new Vector2(bX, bY);
                int distance = ManhattanDistance(sensorCoord, beaconCoord);

                sensors.Add(sensorCoord, distance);
                beacons.Add(beaconCoord);

                minX = (int) Math.Min(minX, sensorCoord.X - distance);
                maxX = (int) Math.Max(maxX, sensorCoord.X + distance);
            }
        }

        int countNoBeacon = 0;
        int yLine = 2000000;

        for (int x = minX; x <= maxX; x++)
        {
            Vector2 currentPos = new Vector2(x, yLine);
            foreach (KeyValuePair<Vector2, int> sensor in sensors)
            {
                int d2 = ManhattanDistance(sensor.Key, currentPos);
                if (d2 <= sensor.Value)
                {
                    countNoBeacon++;
                    break;
                }
            }
        }

        Console.WriteLine($"Task 1: {countNoBeacon - beacons.Where(b => b.Y == yLine).Count()}");

        HashSet<Vector2> possibleBeaconPos = new HashSet<Vector2>();
        foreach (KeyValuePair<Vector2, int> sensor in sensors)
        {
            int ringMinY = Math.Max(0, (int) sensor.Key.Y - sensor.Value - 1);
            int ringMaxY = Math.Min(yLine * 2, (int) sensor.Key.Y + sensor.Value + 1);

            for(int y = ringMinY; y <= ringMaxY; y++)
            {
                int ringXLeft = (int) sensor.Key.X + Math.Abs((int) sensor.Key.Y - y) - sensor.Value - 1;
                ringXLeft = Math.Max(0, ringXLeft);
                int ringXRight = (int) sensor.Key.X + ((int) sensor.Key.X - ringXLeft);
                ringXRight = Math.Min(yLine * 2, ringXRight);
                possibleBeaconPos.Add(new Vector2(ringXLeft, y));
                possibleBeaconPos.Add(new Vector2(ringXRight, y));
            }
        }

        foreach(Vector2 v in possibleBeaconPos)
        {
            bool found = true;
            foreach(KeyValuePair<Vector2, int> otherSensors in sensors) 
            {
                int distanceOfPosToSensor = ManhattanDistance(otherSensors.Key, new Vector2(v.X, v.Y));

                if (distanceOfPosToSensor <= otherSensors.Value)
                {
                    found = false;
                    break;
                }
            }

            if (found)
            {
                long freq = (long) v.X * (yLine * 2) + (long) v.Y;
                Console.WriteLine($"Task 2: {freq}");
                break;
            }
        }
    }

    private static int ManhattanDistance(Vector2 v1, Vector2 v2)
    {
        return (int) Math.Abs(v1.X - v2.X) + (int) Math.Abs(v1.Y - v2.Y);
    }

    private static int CoordStringToInt(string coord)
    {
        return int.Parse(coord.Substring(2));
    }
}
