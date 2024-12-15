using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day11 : DayBase
    {
        public Day11()
            : base(11)
        {
        }

        public override void Assignment1()
        {
            // Infinity stones 25 depth
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            string line = lines[0].Trim();
            List<long> output = new List<long>();
            long[] numbers = line.Split(' ').Select(a => long.Parse(a)).ToArray();
            foreach (long l in numbers) output.Add(l);

            // Naive, brute force approach:
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
            }
            LogAnswer(1, $"{output.Count}");
        }


        public List<long> Calculate(long input, int depth)
        {
            List<long> output = new List<long>();
            output.Add(input);
            for (int n = 0; n < depth; n++)
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
                        // Speed of this can be improved, but it is now already very fast...
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
            }
            return output;
        }

        // Speed things up by storing known values
        Dictionary<(int depth, long value), long> m_knownValues = new Dictionary<(int depth, long value), long>();

        // Recursive call this method
        public long SubCalculate(int depth, int depthStep, List<long> results)
        {
            long totalCount = 0;
            foreach (long r1 in results)
            {
                if (!m_knownValues.ContainsKey((depth: depth, value: r1)))
                {
                    long subCount = 0;
                    List<long> results2 = Calculate(r1, depthStep);
                    if (depth > 0)
                    {
                        subCount = SubCalculate(depth - 1, depthStep, results2);
                    }
                    else
                    {
                        subCount = results2.Count;
                    }
                    m_knownValues.Add((depth: depth, value: r1), subCount);
                    totalCount += subCount;
                }
                else
                {
                    totalCount += m_knownValues[(depth: depth, value: r1)];
                }
            }
            return totalCount;
        }

        public override void Assignment2()
        {
            // Infinity stones 75 depth
            // Answer: 225404711855335

            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            string line = lines[0].Trim();
            List<long> numbers = line.Split(' ').Select(a => long.Parse(a)).ToList();

            long totalCount = SubCalculate(15 - 1, 5,numbers); // 15 * 5 = 75
            Log($"Known values: {m_knownValues.Count}");
            LogAnswer(2, $"{totalCount}");
        }
    }
}
