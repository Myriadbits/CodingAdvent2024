using AdventOfCodeHelpers;
using System.Diagnostics;

namespace CodingAdvent2024
{
    internal class Day18 : DayBase
    {
        public Day18()
            : base(18)
        {
        }

        public override void Assignment1()
        {
            // RAM Run
            int size = (_IsExecutingTest) ? 7 : 71;
            int bytes = (_IsExecutingTest) ? 12 : 1024;
            Map2D map = new Map2D(size, size);

            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            for(int i = 0; i < bytes; i++)
            {
                int[] coordinate = lines[i].Split(',').Select(a => int.Parse(a)).ToArray();
                map.SetInBounds(coordinate[0], coordinate[1], '#');
            }

            LengthTracker[] finished = map.FindShortestPathsLengthOnly(new Position(0, 0, EDirection.South), new Position(size - 1, size - 1), '.').ToArray();
            Array.Sort(finished, delegate(LengthTracker x, LengthTracker y) { return x.Length.CompareTo(y.Length); });

            // Correct answer: 454
            LogAnswer(1, $"{finished[0].Length}");
        }

        public override void Assignment2()
        {
            // RAM Run, when is the path blocked
            int size = (_IsExecutingTest) ? 7 : 71;
            int bytes = (_IsExecutingTest) ? 12 : 1024;
            Map2D map = new Map2D(size, size);
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();            

            // Do a 2 side higher/lower comparision
            int lowBoundary = bytes;
            int highBoundary = lines.Count - 1;
            int guess = 0;

            int lastCorrect = bytes;
            while(highBoundary - lowBoundary > 1)
            {
                guess = lowBoundary + ((highBoundary - lowBoundary) / 2);

                map.Clear();
                for (int i = 0; i < guess; i++)
                {
                    int[] coordinate = lines[i].Split(',').Select(a => int.Parse(a)).ToArray();
                    map.SetInBounds(coordinate[0], coordinate[1], '#');
                }

                Stopwatch sw = Stopwatch.StartNew();
                LengthTracker[] finished = map.FindShortestPathsLengthOnly(new Position(0, 0, EDirection.South), new Position(size - 1, size - 1), '.').ToArray();

                Log($"{guess}: Found {finished.Length} paths (in {sw.ElapsedMilliseconds} ms)");
                if (finished.Length > 0)
                {
                    lowBoundary = guess;
                }
                else
                {
                    highBoundary = guess;
                }
            }

            // Correct answer: 8,51
            LogAnswer(2, $"{lines[guess - 1]}");
        }

    }
}
