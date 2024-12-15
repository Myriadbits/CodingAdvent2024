using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day12 : DayBase
    {
        public Day12()
            : base(12)
        {
        }

        public override void Assignment1()
        {
            // Fences around areas
            long sum1 = 0;
            long sum2 = 0;
            Map2D mapIn = new Map2D(m_filePath);
            Map2D mapOriginal = new Map2D(m_filePath);

            for (int y = 0; y < mapIn.SizeY; y++)
            {
                for (int x = 0; x < mapIn.SizeX; x++)
                {
                    char ch = mapIn.Data[y][x];
                    if (ch != '-')
                    {
                        var data = mapIn.FloodFillFast(x, y, ch, mapOriginal, '-');
                        //Log($"Area[{ch}] = {data.area} - {data.circumfence} - {data.corners}");
                        sum1 += (data.area * data.circumfence);
                        sum2 += (data.area * data.corners);
                    }
                }
            }
            LogAnswer(1, $"{sum1}");
            LogAnswer(2, $"{sum2}");

            // Answer 1: 1486324
            // Answer 2: 898684
        }

        public override void Assignment2()
        {
            // See answer 1
        }
    }
}
