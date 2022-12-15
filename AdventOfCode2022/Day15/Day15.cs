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
                if (d2 <= sensor.Value && !beacons.Contains(currentPos))
                {
                    countNoBeacon++;
                    break;
                }
            }
        }

        Console.WriteLine($"Task 1: {countNoBeacon}");
        yLine *= 2;

        foreach (KeyValuePair<Vector2, int> sensor in sensors)
        {
            int ringMinY = Math.Max(0, (int) sensor.Key.Y - sensor.Value - 1);
            int ringMaxY = Math.Min(yLine, (int) sensor.Key.Y + sensor.Value + 1);

            for(int y = ringMinY; y <= ringMaxY; y++)
            {
                int xLeft = (int) sensor.Key.X + Math.Abs((int) sensor.Key.Y - y) - sensor.Value - 1;
                xLeft = Math.Max(0, xLeft);
                int xRight = (int) sensor.Key.X + ((int) sensor.Key.X - xLeft);
                xRight = Math.Min(yLine, xRight);
                Vector2[] possibleBeaconPos = { new Vector2(xLeft, y), new Vector2(xRight, y) };

                for (int i = 0; i < possibleBeaconPos.Length; i++)
                {
                    if (FindBeacon(sensors, possibleBeaconPos[i]))
                    {
                        long freq = (long) possibleBeaconPos[i].X * (yLine) + (long) possibleBeaconPos[i].Y;
                        Console.WriteLine($"Task 2: {freq}");
                        return;
                    }
                }
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

    private static bool FindBeacon(Dictionary<Vector2, int> sensors, Vector2 possibleBeaconPos)
    {
        bool found = true;
        foreach(KeyValuePair<Vector2, int> sensor in sensors) 
        {
            int distanceOfPosToSensor = ManhattanDistance(sensor.Key, possibleBeaconPos);

            if (distanceOfPosToSensor <= sensor.Value)
            {
                found = false;
                break;
            }
        }

        return found;
    }
}
