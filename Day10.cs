using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day10 : DayBase
    {
        private long m_resultAssignment2 = 0;

        public Day10()
            : base(10)
        {
        }

        private bool CheckPositions(Map2D mapIn, List<Position> positions, Position pos, char value)
        {
            if (mapIn.IsPositionValue(pos.PeekMovePosition(EDirection.North), value))
                positions.Add(pos.PeekMovePosition(EDirection.North));
            if (mapIn.IsPositionValue(pos.PeekMovePosition(EDirection.East), value))
                positions.Add(pos.PeekMovePosition(EDirection.East));
            if (mapIn.IsPositionValue(pos.PeekMovePosition(EDirection.South), value))
                positions.Add(pos.PeekMovePosition(EDirection.South));
            if (mapIn.IsPositionValue(pos.PeekMovePosition(EDirection.West), value))
                positions.Add(pos.PeekMovePosition(EDirection.West));
            return true;
        }

        public (long, long) CombinedAssignment()
        {
            // Find the trails
            long sum1 = 0;
            long sum2 = 0;
            Map2D mapIn = new Map2D(m_filePath);

            for (int y = 0; y < mapIn.SizeY; y++)
            {
                for (int x = 0; x < mapIn.SizeX; x++)
                {
                    if (mapIn.Data[y][x] == '0')
                    {
                        // Find all routes to '9'
                        List<Position> positions = new List<Position>();
                        positions.Add(new Position(x, y));

                        for (char ch = '1'; ch <= '9'; ch++)
                        {
                            List<Position> newPositions = new List<Position>();
                            foreach (Position pos in positions)
                            {
                                CheckPositions(mapIn, newPositions, pos, ch);
                            }
                            positions = newPositions;
                        }

                        // Count the individual positions
                        int num1 = positions.Distinct().Count();
                        int num2 = positions.Count();
                        sum1 += num1;
                        sum2 += num2;

                        Log($"Found: {num1} - {num2}");
                    }
                }
            }
            return (sum1, sum2);
        }
       
        public override void Assignment1()
        {
            // Find the trails
            (long, long) result = CombinedAssignment();
            m_resultAssignment2 = result.Item2;
            LogAnswer(1, $"{result.Item1}");
        }
      
        public override void Assignment2()
        {
            LogAnswer(2, $"{m_resultAssignment2}");
        }
    }
}
