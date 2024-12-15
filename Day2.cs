using System.Text.RegularExpressions;
using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day2 : DayBase
    {
        public Day2()
            : base(2)
        {
        }

        private bool IsReportSafe(int[] numbers, bool log)
        {
            bool valid = true;
            bool increasing = (numbers[1] - numbers[0]) > 0;
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                int diff = numbers[i + 1] - numbers[i];

                if (increasing && (diff < 1 || diff > 3))
                {
                    valid = false;
                    if (log) Log($"Invalid (inc): {string.Join(',', numbers)}");
                    break;
                }
                else if (!increasing && (diff < -3 || diff > -1))
                {
                    valid = false;
                    if (log) Log($"Invalid (dec): {string.Join(',', numbers)}");
                    break;
                }
            }
            return valid;
        }

        public override void Assignment1()
        {
            int validRecords = 0;
            foreach (string line in System.IO.File.ReadLines(m_filePath).ToList())
            {
                string[] data = Regex.Split(line, @"\D+");
                int[] numbers = data.Select(a => int.Parse(a)).ToArray();
                
                if (IsReportSafe(numbers, true)) validRecords++;
            }
            LogAnswer(1, $"{validRecords}");
        }

        public override void Assignment2()
        {
            int validRecords = 0;
            foreach (string line in System.IO.File.ReadLines(m_filePath).ToList())
            {
                string[] data = Regex.Split(line, @"\D+");
                int[] numbers = data.Select(a => int.Parse(a)).ToArray();

                if (!IsReportSafe(numbers, false))
                {
                    for(int i = 0; i < numbers.Length; i++)
                    {
                        List<int> numbersNew = new List<int>(numbers);                        
                        numbersNew.RemoveAt(i);

                        if (IsReportSafe(numbersNew.ToArray(), false))
                        {
                            validRecords++;
                            Log($"Valid: {line} => {string.Join(',', numbersNew)}");
                            break;
                        }
                    }
                }
                else
                    validRecords++;
            }
            LogAnswer(2, $"{validRecords}");
        }
    }
}
