using AdventOfCodeHelpers;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;

namespace CodingAdvent2024
{
    internal class Day20 : DayBase
    {
        public Day20()
            : base(20)
        {
        }

        public override void Assignment1()
        {
            long sum = 0;

            // Race Condition
            Map2D map = new Map2D(m_filePath);
            Position? posStart = map.FindFirstPosition('S');
            Position? posFinish = map.FindFirstPosition('E');
            map.SetInBounds(posStart!, '.');
            map.SetInBounds(posFinish!, '.');
            posStart!.Direction = EDirection.North;
            PositionTracker[] finished = map.FindShortestPathsDirection(posStart!, posFinish!, '.').ToArray();

            //if (finished.Length > 0)
            //    map.DrawPath(finished[0], 'O');

            //Log2DMap(map);
            Log($"Length = {finished[0].History.Count - 1}");

            // Calculate the cheats: Now go over the whole path, see if 2 points are within 2 pixels of each other
            PositionTracker pt = finished[0];
            Dictionary<int, int> pathLengths = new Dictionary<int, int>();
            for (int i = 0; i < pt.History.Count; i++)
            {
                for (int j = i + 1; j < pt.History.Count; j++)
                {
                    int dx = Math.Abs(pt.History[i].X - pt.History[j].X);
                    int dy = Math.Abs(pt.History[i].Y - pt.History[j].Y);
                    if ((dx + dy) < 3 && (dx + dy) > 0)
                    {
                        int deltaLife = Math.Abs(i - j) - 2;
                        if (deltaLife > 1)
                        {
                            if (deltaLife >= 100)
                                sum++;

                            if (pathLengths.ContainsKey(deltaLife))
                                pathLengths[deltaLife]++;
                            else
                                pathLengths.Add(deltaLife, 1);
                        }
                    }
                }
            }

            //Dictionary<int, int> sortedPathLengths = pathLengths.OrderBy(a => a.Key).ToDictionary();
            //foreach(var data in sortedPathLengths)
            //{
            //    Log($"{data.Key} = {data.Value}");
            //}

            LogAnswer(1, $"{sum}");
        }

        public override void Assignment2()
        {
            long sum = 0;

            // Race Condition (cheats up to 20 steps)
            Map2D map = new Map2D(m_filePath);
            Position? posStart = map.FindFirstPosition('S');
            Position? posFinish = map.FindFirstPosition('E');
            map.SetInBounds(posStart!, '.');
            map.SetInBounds(posFinish!, '.');
            posStart!.Direction = EDirection.North;
            PositionTracker[] finished = map.FindShortestPathsDirection(posStart!, posFinish!, '.').ToArray();                      

            Log($"Length = {finished[0].History.Count - 1}");

            // Calculate the cheats: Now go over the whole path, see if 2 points are within X pixels of each other
            long maxCheatLength = 20;
            int minimalDifference = 100;
            PositionTracker pt = finished[0];
            Dictionary<int, int> pathLengths = new Dictionary<int, int>();
            for (int i = 0; i < pt.History.Count; i++)
            {
                for (int j = i + 1; j < pt.History.Count; j++)
                {
                    int dx = Math.Abs(pt.History[i].X - pt.History[j].X);
                    int dy = Math.Abs(pt.History[i].Y - pt.History[j].Y);
                    if ((dx + dy) <= maxCheatLength && (dx + dy) > 0)
                    {
                        int deltaLife = Math.Abs(i - j) - (dx + dy);
                        if (deltaLife > 1)
                        {
                            if (deltaLife >= minimalDifference)
                                sum++;

                            if (pathLengths.ContainsKey(deltaLife))
                                pathLengths[deltaLife]++;
                            else
                                pathLengths.Add(deltaLife, 1);
                        }
                    }
                }
            }

            //Dictionary<int, int> sortedPathLengths = pathLengths.OrderBy(a => a.Key).ToDictionary();
            //foreach (var data in sortedPathLengths)
            //{
            //    if (data.Key >= minimalDifference)
            //        Log($"There are {data.Value} cheats that save {data.Key} picoseconds");
            //}

            // 1050608 is too high
            LogAnswer(2, $"{sum}");
        }

    }
}
