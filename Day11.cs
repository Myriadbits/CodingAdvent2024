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
      
        public override void Assignment2()
        {
            // Infinity stones
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            string line = lines[0].Trim();
            List<long> output = new List<long>();
            long[] numbers = line.Split(' ').Select(a => long.Parse(a)).ToArray();
            foreach (long l in numbers) output.Add(l);

            List<long> unevenNumbers = new List<long>();
            long maxValue = 0;
            for (int n = 0; n < 25; n++)
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
                        output[j] = long.Parse(textNum.Substring(0, textNum.Length / 2));
                        output.Add(long.Parse(textNum.Substring(textNum.Length / 2)));
                    }
                    else
                    {
                        if (!unevenNumbers.Contains(output[j]))
                            unevenNumbers.Add(output[j]);
                        output[j] = output[j] * 2024;
                    }

                    if (output[j] > maxValue)
                    {
                        maxValue = output[j];
                    }
                }
               
            }
            unevenNumbers.Sort();

            LogAnswer(2, $"{string.Join(',', unevenNumbers)}");
        }
    }
}
