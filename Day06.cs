using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day06 : DayBase
    {
        char[][]? m_outputsA1 = null;

        public Day06()
            : base(6)
        {
        }
       
        public override void Assignment1()
        {
            // Guard moves in labyrinth
            long sum = 0;

            Position pos = new Position();

            Dictionary<int, List<int>> breakrules = new Dictionary<int, List<int>>();
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            int sizeX = lines[0].Length;
            int sizeY = lines.Count;

            Position.SetMaxes(lines[0].Length, lines.Count);

            for (int y = 0; y < sizeY; y++)
            {
                string line = lines[y];
                for (int x = 0; x < sizeX; x++)
                { 
                    if (line[x] == '^')
                    {
                        pos.X = x;
                        pos.Y = y;
                        pos.Direction = EDirection.North;
                        break;
                    }
                }
            }

            m_outputsA1 = Create2DCA(sizeX, sizeY, '.');
            while (pos.Move())
            {
                m_outputsA1[pos.Y][pos.X] = 'X';
                Position newPos = pos.PeekMovePosition(pos.Direction); // The next position
                if (lines[newPos.Y][newPos.X] == '#')
                {
                    pos.RotateRight();
                }
            }


            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (m_outputsA1[y][x] == 'X')
                        sum++;
                }
            }
            //Log2DCA(m_outputsA1);


            LogAnswer(1, $"{sum}");
        }

        public override void Assignment2()
        {
            // Let the guard move in circles in labyrinth
            long sum = 0;

            Position pos = new Position();

            Dictionary<int, List<int>> breakrules = new Dictionary<int, List<int>>();
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            int sizeX = lines[0].Length;
            int sizeY = lines.Count;

            char[][] orgInput = Create2DCA(sizeX, sizeY, '.');

            Position startPos = new Position();
            for (int y = 0; y < sizeY; y++)
            {
                string line = lines[y];
                for (int x = 0; x < sizeX; x++)
                {
                    if (line[x] == '^')
                    {
                        startPos.X = x;
                        startPos.Y = y;
                        startPos.Direction = EDirection.North;
                    }
                    else if (line[x] == '#')
                    {
                        orgInput[y][x] = '#';
                    }
                }
            }

            Position.SetMaxes(0, sizeX, 0, sizeY);
           

            // Should be 2162
            for (int y1 = 0; y1 < sizeY; y1++)
            {
                for (int x1 = 0; x1 < sizeX; x1++)
                {
                    char[][] route = Create2DCA(sizeX, sizeY, '\0');
                    if (y1 == startPos.Y && x1 == startPos.X)
                    {
                        // Skip guard start pos
                    }
                    else if (m_outputsA1 != null && m_outputsA1[y1][x1] == 'X') // Valid positions are only those that are visited by the guard in part 1
                    {
                        // Add a new turning point
                        orgInput[y1][x1] = '#'; // Add extra obstacle

                        // Start moving
                        pos = new Position(startPos);
                        while (true)
                        {
                            if (HasVisitedWIthSameDirection(route, pos))
                            {
                                sum++;
                                break;
                            }
                            MarkPosition(route, pos);
                            
                            Position newPos = pos.PeekMovePosition(); // The next position
                            if (orgInput[newPos.Y][newPos.X] == '#')
                            {
                                pos.RotateRight();
                                
                                // We might be moving to another obstacle!
                                newPos = pos.PeekMovePosition(); // The next position
                                if (orgInput[newPos.Y][newPos.X] == '#')
                                {
                                    pos.RotateRight();
                                }
                            }

                            if (!pos.Move())
                            {
                                break;
                            }
                        }

                        orgInput[y1][x1] = '.'; // Remove the obstacle
                    }
                }
            }

            LogAnswer(2, $"{sum}");
        }
    }
}
