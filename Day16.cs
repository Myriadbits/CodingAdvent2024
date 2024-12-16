using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day16 : DayBase
    {
        private long m_answer1 = 0;

        public Day16()
            : base(16)
        {
        }

        public override void Assignment1()
        {
            // Reindeer Maze
            Map2D map = new Map2D(m_filePath);
            Position? posStart = map.FindFirstPosition('S');
            Position? posFinish = map.FindFirstPosition('E');

            if (posStart is null || posFinish is null) return;

            map.SetInBounds(posStart, '.');
            map.SetInBounds(posFinish, '.');

            posStart.Direction = EDirection.East;
            List<PositionTracker> paths = map.FindPaths(posStart, posFinish);

            long minSum = 999999999;
            PositionTracker? posMin = null;
            foreach (PositionTracker pt in paths)
            {
                if (pt.WeightPerDirectionSwitch < minSum)
                {
                    minSum = pt.WeightPerDirectionSwitch;
                    posMin = pt;
                }
            }

            map.DrawPathDirection(posMin);
            Log2DMap(map);
            m_answer1 = minSum;

            //m_answer1 = 91464;
            // Correct answer: 91464
            LogAnswer(1, $"{m_answer1}");
        }

        public override void Assignment2()
        {
            // Reindeer Maze best positions
            Map2D map = new Map2D(m_filePath);
            Position? posStart = map.FindFirstPosition('S');
            Position? posFinish = map.FindFirstPosition('E');

            if (posStart is null || posFinish is null) return;

            map.SetInBounds(posStart, '.');
            map.SetInBounds(posFinish, '.');

            posStart.Direction = EDirection.East;
            List<PositionTracker> paths = map.FindPaths(posStart, posFinish, m_answer1);

            long minSum = 999999999;
            PositionTracker? posMin = null;
            foreach (PositionTracker pt in paths)
            {
                if (pt.WeightPerDirectionSwitch < minSum)
                {
                    minSum = pt.WeightPerDirectionSwitch;
                    posMin = pt;
                }
            }

            List<PositionTracker> shortestPaths = paths.Where(a => a.WeightPerDirectionSwitch == minSum).ToList();
            foreach (PositionTracker pt in shortestPaths)
            {
                map.DrawPath(pt, 'O');
            }
            Log2DMap(map);

            int sum = map.Count('O');

            // Answer = 464 in 7 minutes (waaaay too long)
            LogAnswer(1, $"{sum}");
        }

    }
}
