using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodingAdvent
{
    internal class Day5 : DayBase
    {
        public Day5()
            : base(5)
        {
        }
       
        public override void Assignment1()
        {
            // Find the incorrect ordered pages
            long sum = 0;

            Dictionary<int, List<int>> breakrules = new Dictionary<int, List<int>>();
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            foreach (var line in lines)
            {
                if (line.Contains('|'))
                {
                    int[] numbers = line.Split('|').Select(a => int.Parse(a)).ToArray();
                    if (!breakrules.ContainsKey(numbers[1]))
                    {
                        breakrules[numbers[1]] = new List<int>();
                    }
                    breakrules[numbers[1]].Add(numbers[0]);
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    int[] numbers = line.Split(',').Select(a => int.Parse(a)).ToArray();
                    // Check numbers against rules
                    bool valid = true;
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        if (breakrules.ContainsKey(numbers[i]))
                        {
                            bool found = false;
                            foreach (int num in breakrules[numbers[i]])
                            {
                                for (int j = i + 1; j < numbers.Length; j++)
                                {
                                    if (numbers[j] == num)
                                    {
                                        found = true;
                                        Log($"{line} breaks for rule: {num}|{numbers[i]}");
                                        break;
                                    }
                                }
                                if (found)
                                    break;
                            }
                            if (found)
                            {
                                // Rule fails
                                valid = false;
                                break;
                            }
                        }
                    }
                    if (valid)
                    {
                        sum += numbers[(numbers.Length / 2)];
                    }
                }
            }

            LogAnswer(1, $"{sum}");
        }

        public override void Assignment2()
        {
            // Order the incorrect ordered pages
            long sum = 0;

            Dictionary<int, List<int>> breakrules = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> rules = new Dictionary<int, List<int>>();
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            foreach (var line in lines)
            {
                if (line.Contains('|'))
                {
                    int[] numbers = line.Split('|').Select(a => int.Parse(a)).ToArray();
                    if (!breakrules.ContainsKey(numbers[1]))
                        breakrules[numbers[1]] = new List<int>();
                    breakrules[numbers[1]].Add(numbers[0]);

                    if (!rules.ContainsKey(numbers[0]))
                        rules[numbers[0]] = new List<int>();
                    rules[numbers[0]].Add(numbers[1]);
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    int[] numbers = line.Split(',').Select(a => int.Parse(a)).ToArray();
                    // Check numbers against rules
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        if (breakrules.ContainsKey(numbers[i]))
                        {
                            bool found = false;
                            foreach (int num in breakrules[numbers[i]])
                            {
                                for (int j = i + 1; j < numbers.Length; j++)
                                {
                                    if (numbers[j] == num)
                                    {
                                        found = true;
                                        Log($"{line} breaks for rule: {num}|{numbers[i]}");
                                        break;
                                    }
                                }
                                if (found)
                                    break;
                            }
                            if (found)
                            {
                                // Found an error
                                // Now order these pages according to rules
                                int[] tempnumbers = new int[numbers.Length];
                                numbers.CopyTo(tempnumbers, 0);
                                for (int i1 = 0; i1 < numbers.Length - 1; i1++)
                                {
                                    for (int i2 = 0; i2 < numbers.Length - i1 - 1; i2++)
                                    {
                                        if (breakrules.ContainsKey(tempnumbers[i2]))
                                        {
                                            if (breakrules[tempnumbers[i2]].Contains(tempnumbers[i2 + 1]))
                                            {
                                                int tempnum = tempnumbers[i2];
                                                tempnumbers[i2] = tempnumbers[i2 + 1];
                                                tempnumbers[i2 + 1] = tempnum;
                                            }
                                        }
                                    }
                                }
                                Log($"{string.Join(',', tempnumbers)}");
                                sum += tempnumbers[(tempnumbers.Length / 2)];
                                break;
                            }
                        }
                    }
                }
            }

            LogAnswer(2, $"{sum}");
        }
    }
}
