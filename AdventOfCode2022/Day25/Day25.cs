namespace AdventOfCode2022.Day25;

internal class Day25
{
    const string inputPath = @"Day25/Input.txt";
    public static void Task1()
    {
        List<string> lines = File.ReadAllLines(inputPath).ToList();
        long decSum = 0;

        foreach (string line in lines)
        {
            long lineNum = 0;
            for(int i = 0; i < line.Length; i++)
            {
                int lineIdx = line.Length - 1 - i;
                lineNum += (long) Math.Pow(5, i) * DecodeSNAFU(line[lineIdx]);
            }

            decSum += lineNum;
        }

        Console.WriteLine($"Task 1: {EncodeSNAFU(decSum)}");
    }

    private static int DecodeSNAFU(char c)
    {
        if (c == '=')
            return -2;
        else if (c == '-')
            return -1;
        else if (c == '0')
            return 0;
        else if (c == '1')
            return 1;
        else if (c == '2')
            return 2;

        return int.MinValue;
    }

    private static String EncodeSNAFU(long num)
    {
        string SNAFU = "";
        while (num > 0)
        {
            long x = num % 5;
            if (x == 3)
            {
                x = -2;
                SNAFU = "=" + SNAFU;
            }
            else if (x == 4)
            {
                x = -1;
                SNAFU = "-" + SNAFU;
            }
            else
            {
                SNAFU = x + SNAFU;
            }

            num = (num - x) / 5;
        }

        return SNAFU;
    }
}
