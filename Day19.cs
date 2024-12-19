using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day19 : DayBase
    {
        public Day19()
            : base(19)
        {
        }

        // Test data:
        // r(1), g(1), b(1), w, u
        // u in only 1 combination

        // In the real data
        // w(1), u(1), b, r(1), g(1)
        // Number of b's = 411

        public override void Assignment1()
        {
            // Linen Layout
            long sum = 0;

            char chUsefull = 'b';
            if (_IsExecutingTest)
                chUsefull = 'w';

            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            string[] allCombinations = lines[0].Split(", ");
            string[] combinations = allCombinations.Where(a => a.Contains(chUsefull)).ToArray();

            for(int n = 2; n < lines.Count; n++)
            {
                string line = lines[n];
                bool found = false;
                if (line.Contains(chUsefull))
                {
                    foreach (string combi in combinations)
                    {
                        if (line.Contains(combi))
                        {
                            found = true;
                            break;
                        }
                    }
                }
                else
                {
                    found = true;
                }
                if (!found)
                {
                    Log($"{n}: {line} = INCORRECT");
                    sum++;
                }
                else
                {
                    Log($"{n}: {line} = Correct");
                }

            }

            LogAnswer(1, $"{sum}");
        }

        public override void Assignment2()
        {
            // Linen Layout
            long sum = 0;

            LogAnswer(2, $"{sum}");
        }

    }
}
