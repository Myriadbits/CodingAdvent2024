using System;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodingAdvent
{
    internal class Day11 : DayBase
    {
        public Day11()
            : base(11)
        {
        }

        public override void Assignment1()
        {
            // Infinity stones
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            string line = lines[0].Trim();
            List<long> output = new List<long>();
            long[] numbers = line.Split(' ').Select(a => long.Parse(a)).ToArray();
            foreach (long l in numbers) output.Add(l);

            for (int n = 0; n < 25; n++)
            {
                int count = output.Count;
                for(int j = 0; j < count; j++)
                {
                    if (output[j] == 0)
                    {
                        output[j] = 1;
                    }
                    else if ((output[j].ToString().Length % 2) == 0)
                    {
                        string textNum = output[j].ToString();
                        output[j] = long.Parse(textNum.Substring(0, textNum.Length / 2));
                        output.Add(long.Parse(textNum.Substring(textNum.Length / 2)));
                    }
                    else
                    {
                        output[j] = output[j] * 2024;
                    }
                }
            }

            LogAnswer(1, $"{output.Count}");
        }

        public void Calculate(long input, Dictionary<long, List<long>> knownNumbers)
        {
            List<long> output = new List<long>();
            output.Add(input);
            if (knownNumbers.ContainsKey(input))
                return;
            knownNumbers.Add(input, new List<long>());
            knownNumbers[input].Add(1);
            for (int n = 0; n < 15; n++)
            {
                int count = output.Count;
                for (int j = 0; j < count; j++)
                {
                    if (output[j] == 0)
                    {
                        output[j] = 1;
                    }
                    else if ((output[j].ToString().Length % 2) == 0)
                    {
                        string textNum = output[j].ToString();
                        long num1 = long.Parse(textNum.Substring(0, textNum.Length / 2));
                        long num2 = long.Parse(textNum.Substring(textNum.Length / 2));
                        output[j] = num1;
                        output.Add(num2);
                    }
                    else
                    {
                        output[j] = output[j] * 2024;
                    }
                }
                knownNumbers[input].Add(output.Count);
            }

            for (int i = 0; i < output.Count; i++)
            {
                if (!knownNumbers.ContainsKey(output[i]))
                {
                    Calculate(output[i], knownNumbers);
                }
            }
        }



        public override void Assignment2()
        {
            // Infinity stones
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            string line = lines[0].Trim();
            List<long> output = new List<long>();
            long[] numbers = line.Split(' ').Select(a => long.Parse(a)).ToArray();
            foreach (long l in numbers) output.Add(l);

            Dictionary<long, List<long>> knownNumbers = new Dictionary<long, List<long>>();
            for (int j = 0; j < 10; j++)
            {
                Calculate(j, knownNumbers);
            }
            foreach (long l in numbers)
            {
                Calculate(l, knownNumbers);
            }

            //for (int n = 0; n < 75; n++)
            //{
            //    int count = output.Count;
            //    for (int j = 0; j < count; j++)
            //    {
            //        Calculate(output[j], knownNumbers);
            //        if (output[j] == 0)
            //        {
            //            output[j] = 1;
            //        }
            //        else if ((output[j].ToString().Length % 2) == 0)
            //        {
            //            string textNum = output[j].ToString();
            //            output[j] = long.Parse(textNum.Substring(0, textNum.Length / 2));
            //            long newNum = long.Parse(textNum.Substring(textNum.Length / 2));
            //            Calculate(newNum, knownNumbers);
            //            output.Add(newNum);
            //        }
            //        else
            //        {
            //            output[j] = output[j] * 2024;
            //        }                    
            //    }
            //}

            //3929933
            //Log($"{string.Join(',', unevenNumbers)}");

            LogAnswer(2, $"{output.Count}");
        }
    }
}
