using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day25 : DayBase
    {
        public Day25()
            : base(25)
        {
        }

        public override void Assignment1()
        {
            // Code Chronicle 
            long sum = 0;
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            List<List<int>> locks = new List<List<int>>();
            List<List<int>> keys = new List<List<int>>();
            for (int n = 0; n < lines.Count(); n += 8)
            {
                Map2D map = new Map2D(lines.Take(new Range(n, n + 7)).ToList());
                char ch = map.Data[0][0];
                List<int> code = new List<int>();
                for (int x = 0; x < map.SizeX; x++)
                {
                    for (int y = 0; y < map.SizeY; y++)
                    {
                        if (!map.IsValue(x, y, ch))
                        {
                            code.Add(y - 1);
                            break;
                        }
                    }
                }
                if (ch == '#')
                    locks.Add(code);
                else
                    keys.Add(code);
            }

            foreach(var key in keys)
                Log($"Key: {string.Join(',', key)}");
            foreach (var lo in locks)
                Log($"Lock: {string.Join(',', lo)}");

            for (int i = 0; i < keys.Count(); i++)
            {
                for(int j = 0; j < locks.Count(); j++)
                {
                    bool match = true;
                    for(int k = 0; k < keys[i].Count(); k++)
                    {
                        if (keys[i][k] < locks[j][k])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                    {
                        sum++;
                        Log($"Match lock: {string.Join(',', locks[j])} - key: {string.Join(',', keys[i])}");
                    }
                }
            }

            LogAnswer(1, $"{sum}");
        }

        public override void Assignment2()
        {
            long sum = 0;

            LogAnswer(2, $"{sum}");
        }

    }
}
