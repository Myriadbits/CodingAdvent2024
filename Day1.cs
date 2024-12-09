using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodingAdvent
{
    internal class Day1 : DayBase
    {
        public Day1()
            : base(1)
        {
        }

        public override void Assignment1()
        {
            List<int> leftData = new List<int>();
            List<int> rightData = new List<int>();
            foreach(string line in System.IO.File.ReadLines(m_filePath).ToList())
            {
                string[] data = Regex.Split(line, @"\D+");
                if (data.Length == 2)
                {
                    leftData.Add(int.Parse(data[0]));
                    rightData.Add(int.Parse(data[1]));
                }
            }

            leftData.Sort();
            rightData.Sort();

            long sum = 0;
            for (int i = 0; i < leftData.Count; i++)
            {
                sum += Math.Abs(leftData[i] - rightData[i]);
            }

            LogAnswer(1, $"{sum}");
        }

        public override void Assignment2()
        {
            Dictionary<int, int> leftData = new Dictionary<int, int>();
            Dictionary<int, int> rightData = new Dictionary<int, int>();
            foreach (string line in System.IO.File.ReadLines(m_filePath).ToList())
            {
                string[] data = Regex.Split(line, @"\D+");
                if (data.Length == 2)
                {
                    int[] numbers = data.Select(a => int.Parse(a)).ToArray();

                    if (leftData.ContainsKey(numbers[0]))
                        leftData[numbers[0]]++;
                    else
                        leftData[numbers[0]] = 1;

                    if (rightData.ContainsKey(numbers[1]))
                        rightData[numbers[1]]++;
                    else
                        rightData[numbers[1]] = 1;
                }
            }

            long sum = 0;
            foreach (int key in leftData.Keys)
            {
                if (rightData.ContainsKey(key))
                {
                    sum += key * rightData[key] * leftData[key];
                }
            }
            LogAnswer(1, $"{sum}");
        }
    }
}
