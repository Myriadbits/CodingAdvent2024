using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day4 : DayBase
    {
        public Day4()
            : base(4)
        {
        }
       
        public override void Assignment1()
        {
            // Find the XMAS
            long sum = 0;

            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            int sizeY = lines.Count;
            int sizeX = lines[0].Length;

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX - 3; x++)
                {
                    if (lines[y][x] == 'X' && lines[y][x + 1] == 'M' && lines[y][x + 2] == 'A' && lines[y][x + 3] == 'S')
                        sum++;
                    if (lines[y][x] == 'S' && lines[y][x + 1] == 'A' && lines[y][x + 2] == 'M' && lines[y][x + 3] == 'X')
                        sum++;
                }
            }

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY - 3; y++)
                {
                    if (lines[y][x] == 'X' && lines[y + 1][x] == 'M' && lines[y + 2][x] == 'A' && lines[y + 3][x] == 'S')
                        sum++;
                    if (lines[y][x] == 'S' && lines[y + 1][x] == 'A' && lines[y + 2][x] == 'M' && lines[y + 3][x] == 'X')
                        sum++;
                }
            }

            for (int y = 0; y < sizeY - 3; y++)
            {
                for (int x = 0; x < sizeX - 3; x++)
                {
                    if (lines[y][x] == 'X' && lines[y + 1][x + 1] == 'M' && lines[y + 2][x + 2] == 'A' && lines[y + 3][x + 3] == 'S')
                        sum++;
                    if (lines[y][x] == 'S' && lines[y + 1][x + 1] == 'A' && lines[y + 2][x + 2] == 'M' && lines[y + 3][x + 3] == 'X')
                        sum++;
                }
            }

            for (int y = 0; y < sizeY - 3; y++)
            {
                for (int x = 0; x < sizeX - 3; x++)
                {
                    if (lines[y][x + 3] == 'X' && lines[y + 1][x + 2] == 'M' && lines[y + 2][x + 1] == 'A' && lines[y + 3][x] == 'S')
                        sum++;
                    if (lines[y][x + 3] == 'S' && lines[y + 1][x + 2] == 'A' && lines[y + 2][x + 1] == 'M' && lines[y + 3][x] == 'X')
                        sum++;
                }
            }

            LogAnswer(1, $"{sum}");
        }

        public override void Assignment2()
        {
            // Find the M M  or M S or S S or S M
            //           A       A      A      A
            //          S S     M S    M M    S M
            long sum = 0;

            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            int sizeY = lines.Count;
            int sizeX = lines[0].Length;

            char[][] outputs = Create2DCA(sizeX, sizeY, '.');

            for (int y = 1; y < sizeY - 1; y++)
            {
                for (int x = 1; x < sizeX - 1; x++)
                {
                    if (lines[y][x] == 'A')
                    {
                        bool found = false;
                        if (lines[y - 1][x - 1] == 'M' && lines[y - 1][x + 1] == 'M' &&
                            lines[y + 1][x - 1] == 'S' && lines[y + 1][x + 1] == 'S')
                            found = true;
                        else if (lines[y - 1][x - 1] == 'M' && lines[y - 1][x + 1] == 'S' &&
                            lines[y + 1][x - 1] == 'M' && lines[y + 1][x + 1] == 'S')
                            found = true;
                        else if (lines[y - 1][x - 1] == 'S' && lines[y - 1][x + 1] == 'S' &&
                            lines[y + 1][x - 1] == 'M' && lines[y + 1][x + 1] == 'M')
                            found = true;
                        else if (lines[y - 1][x - 1] == 'S' && lines[y - 1][x + 1] == 'M' &&
                            lines[y + 1][x - 1] == 'S' && lines[y + 1][x + 1] == 'M')
                            found = true;

                        if (found)
                        {
                            sum++;
                            outputs[y][x] = 'A';
                        }
                    }
                }
            }
            Log2DCA(outputs);
            LogAnswer(2, $"{sum}");
        }
    }
}
