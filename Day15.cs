using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day15 : DayBase
    {
        public Day15()
            : base(15)
        {
        }

        public bool CanBeMovedAndMove(Map2D map, Position pos)
        {
            char ch = map.GetValue(pos);
            if (ch == '#')
            {
                return false;
            }
            else if (ch == 'O')
            {
                if (CanBeMovedAndMove(map, pos.PeekMovePosition()))
                {
                    map.SetInBounds(pos.PeekMovePosition(), 'O');
                    map.SetInBounds(pos, '.');
                    return true;
                }
                return false;
            }
            return true;
        }
        
        public override void Assignment1()
        {
            long sum = 0;

            // Warehouse Woes
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            int mapEnd = lines.FindIndex(a => string.IsNullOrEmpty(a));
            string directions = string.Join(null, lines.TakeLast(lines.Count - mapEnd - 1));
            Map2D map = new Map2D(lines.Take(mapEnd).ToList());

            // Find the start postion
            Position? robotPos = map.FindFirstPosition('@');
            if (robotPos is not null)
            {
                // Remove start pos
                map.SetInBounds(robotPos.X, robotPos.Y, '.');

                foreach (char c in directions)
                {
                    robotPos.Direction = Position.CharToDirection(c);
                    Position nextPos = robotPos.PeekMovePosition();
                    if (CanBeMovedAndMove(map, robotPos.PeekMovePosition()))
                    {
                        robotPos.Move();
                    }
                }

                sum = map.FindAllPositions('O').Sum(a => a.Y * 100 + a.X);

                Log2DMap(map);
            }

            LogAnswer(1, $"{sum}");
        }

        /// <summary>
        /// Check if the entire tree can be moved
        /// </summary>
        /// <param name="map"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool CanBeMoved2(Map2D map, Position pos)
        {
            char ch = map.GetValue(pos);
            if (ch == '#')
            {
                return false;
            }
            else if (ch == '[' || ch == ']')
            {
                if (CanBeMoved2(map, pos.PeekMovePosition()))
                {
                    if (pos.Direction == EDirection.North || pos.Direction == EDirection.South)
                    {
                        Position pos2 = new Position(pos.X + ((ch == '[') ? 1 : -1), pos.Y, pos.Direction);
                        if (CanBeMoved2(map, pos2.PeekMovePosition()))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        public void MoveTree(Map2D map, Position pos)
        {
            char ch = map.GetValue(pos);
            if (ch == '[' || ch == ']')
            {
                MoveTree(map, pos.PeekMovePosition());
                map.SetInBounds(pos.PeekMovePosition(), ch);
                map.SetInBounds(pos, '.');
                if (pos.Direction == EDirection.North || pos.Direction == EDirection.South)
                {
                    Position pos2 = new Position(pos.X + ((ch == '[') ? 1 : -1), pos.Y, pos.Direction);
                    MoveTree(map, pos2.PeekMovePosition());
                    map.SetInBounds(pos2.PeekMovePosition(), ((ch == '[') ? ']' : '['));
                    map.SetInBounds(pos2, '.');
                }
            }
        }

        public override void Assignment2()
        {
            long sum = 0;

            // Warehouse Woes + everything twice as wide
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            int mapEnd = lines.FindIndex(a => string.IsNullOrEmpty(a));
            string directions = string.Join(null, lines.TakeLast(lines.Count - mapEnd - 1));

            // Expand map
            List<string> mapLines = lines.Take(mapEnd).ToList();
            List<string> newMapLines = new List<string>();
            foreach (string line in mapLines)
            {
                string newLine = "";
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '.')
                        newLine += "..";
                    else if (line[i] == '#')
                        newLine += "##";
                    else if (line[i] == '@')
                        newLine += "@.";
                    else if (line[i] == 'O')
                        newLine += "[]";
                }
                newMapLines.Add(newLine);
            }
            Map2D map = new Map2D(newMapLines);

            // Find the start postion
            Position? robotPos = map.FindFirstPosition('@');
            if (robotPos is not null)
            {
                // Remove start pos
                map.SetInBounds(robotPos.X, robotPos.Y, '.');

                foreach(char ch in directions)
                {
                    robotPos.Direction = Position.CharToDirection(ch);
                    Position nextPos = robotPos.PeekMovePosition();
                    if (CanBeMoved2(map, robotPos.PeekMovePosition()))
                    {
                        // Only move all the boxes when the ALL can be moved
                        MoveTree(map, robotPos.PeekMovePosition());
                        robotPos.Move();
                    }
                }

                sum = map.FindAllPositions('[').Sum(a => a.Y * 100 + a.X);
                map.SetInBounds(robotPos, '@');
                Log2DMap(map);
            }

            // Correct answer: 1437981
            LogAnswer(2, $"{sum}");
        }

    }
}
