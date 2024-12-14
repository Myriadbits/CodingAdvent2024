using CodingAdvent2024;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodingAdvent
{
    internal class Day14 : DayBase
    {
        public Day14()
            : base(14)
        {
        }

        (long startX, long startY, long deltaX, long deltaY) GetData(string line)
        {
            // p=0,4 v=3,-3
            Regex exp = new Regex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");
            Match mA = exp.Match(line);
            return (long.Parse(mA.Groups[1].ToString()), long.Parse(mA.Groups[2].ToString()), long.Parse(mA.Groups[3].ToString()), long.Parse(mA.Groups[4].ToString()));
        }

        public override void Assignment1()
        {
            // Restroom Redoubt
            long sum = 0;
            long steps = 100;
            List<string> lines = File.ReadLines(m_filePath).ToList();

            Map2D map;
            if (_IsExecutingTest)
                map = new Map2D(11, 7);
            else
                map = new Map2D(101, 103);
            int[] quadrants = new int[4];
            for (int i = 0; i < lines.Count; i++)
            {
                var data = GetData(lines[i]);
                int x = (int)(data.startX + steps * data.deltaX) % map.SizeX;
                int y = (int)(data.startY + steps * data.deltaY) % map.SizeY;
                if (x < 0) x += map.SizeX;
                if (y < 0) y += map.SizeY;

                if (x < map.SizeX / 2 && y < map.SizeY / 2) quadrants[0]++;
                if (x > map.SizeX / 2 && y < map.SizeY / 2) quadrants[1]++;

                if (x < map.SizeX / 2 && y > map.SizeY / 2) quadrants[2]++;
                if (x > map.SizeX / 2 && y > map.SizeY / 2) quadrants[3]++;

                if (!map.SetInBounds(x, y, 'X'))
                {
                    Log($"Something is wrong");
                }
            }
            //map.Print(14, 1);

            sum = quadrants[0] * quadrants[1] * quadrants[2] * quadrants[3];
            LogAnswer(1, $"{sum}");
        }



        // Hmm: Smaller than 1000000000
        // Hmm: Smaller than 1000000
        // Issue: Christmas tree was not centered and there where snowflakes in the rest of the image (noise)
        //        I assumed it would be symmetrical around the x=50 axis => it was not.
        //        I assumed it would be centered vertically => it was not
        public override void Assignment2()
        {
            // Restroom Redoubt => christmas tree?
            // Brute force approach, not going to work I guess
            List<string> lines = File.ReadLines(m_filePath).ToList();

            List<(long startX, long startY, long deltaX, long deltaY)> robots = new List<(long startX, long startY, long deltaX, long deltaY)>();
            for (int i = 0; i < lines.Count; i++)
            {
                var data = GetData(lines[i]);
                robots.Add(data);
            }

            Map2D map = new Map2D(101, 103);
            int halfMapX = map.SizeX / 2;
            int[] points = new int[map.SizeY];
            for (long j = 0; j < 1000000; j++)
            {
                bool print = false;
                Array.Fill(points, 0);
                map.Clear();

                int checkY = -1; 
                foreach (var robot in robots)
                {
                    int x = ((int)(robot.startX + j * robot.deltaX) % map.SizeX + map.SizeX) % map.SizeX;
                    int y = ((int)(robot.startY + j * robot.deltaY) % map.SizeY + map.SizeY) % map.SizeY;
                    map.SetInBounds(x, y, 'x');
                    points[y]++;
                    if (points[y] >= 20) // Find the y value with a lot of pixels
                    {
                        checkY = y;
                    }
                }

                if (checkY != -1) // Check of the found y line, count the number of consequetive pixels
                {
                    int countX = 0;
                    for (int x = 0; x < halfMapX; x++)
                    {
                        if (map.IsValue(x, checkY, 'x'))
                        {
                            countX = 0;
                            for (int x2 = x + 1; x2 < map.SizeX; x2++)
                            {
                                if (!map.IsValue(x2, checkY, 'x'))
                                    break;
                                countX++;
                            }
                            if (countX > 10) // Hmm, a lot of pixels next to each other, let's show it
                            {
                                print = true;
                                break;
                            }
                        }
                    }
                }

                if (print)
                {
                    map.Clear();
                    foreach (var robot in robots)
                    {
                        int x = ((int)(robot.startX + j * robot.deltaX) % map.SizeX + map.SizeX) % map.SizeX;
                        int y = ((int)(robot.startY + j * robot.deltaY) % map.SizeY + map.SizeY) % map.SizeY;
                        map.SetInBounds(x, y, 'X');
                    }

                    map.Print(14, 1);
                    LogAnswer(2, $"{j}");
                    break;
                }
            }
        }

    }
}
