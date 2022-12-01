using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day01
{
    internal class Day01
    {
        const string inputPath = @"Day01/Input.txt";

        public static void Task1and2()
        {
            List<String> lines = File.ReadAllLines(inputPath).ToList();

            List<int> calories = new List<int>();

            int currentCalories = 0;
            foreach (String line in lines)
            {
                if (line.Length == 0)
                {
                    calories.Add(currentCalories);
                    currentCalories = 0;
                    continue;
                }

                currentCalories += int.Parse(line);
            }

            calories.Add(currentCalories);

            calories.Sort();
            calories.Reverse();
            Console.WriteLine($"Task 1: {calories.First()}");
            Console.WriteLine($"Task 2: {calories.Take(3).Sum()}");
        }
    }
}
