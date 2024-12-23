using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day21 : DayBase
    {
        public Day21()
            : base(21)
        {
        }

        private int[,] keypadPaths = new int[10,9];

        public string[,] InitializeKeypad(int level)
        {
            Position[] keys = new Position[] { new Position(1, 3), 
                new Position(0, 2), new Position(1, 2), new Position(2, 2), 
                new Position(0, 1), new Position(1, 1), new Position(2, 1), 
                new Position(0, 0), new Position(1, 0), new Position(2, 0), 
                new Position(2, 3)};

            string[,] resultList = new string[11, 11];
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    Position posi = keys[i];
                    Position posj = keys[j];
                    int deltaX = posj.X - posi.X;
                    int deltaY = posj.Y - posi.Y;

                    string result = "";
                    if (Math.Abs(deltaX) > 0 && Math.Abs(deltaY) > 0)// && i >= 1 && i <= 9 && j >= 1 && j <= 9)
                    {
                        string result1 = "";
                        if (posi.X + deltaX == 0 && posi.Y == 3)
                        {
                            //Log($"shit");
                        }
                        else
                        {
                            if (deltaX > 0)
                                result1 += new string('>', deltaX);
                            else if (deltaX < 0)
                                result1 += new string('<', -deltaX);
                            if (deltaY > 0)
                                result1 += new string('v', deltaY);
                            else if (deltaY < 0)
                                result1 += new string('^', -deltaY);
                            result1 += 'A';
                        }

                        string result2 = "";
                        if (posi.Y + deltaY == 3 && posi.X == 0)
                        {
                            //Log($"shit");
                        }
                        else
                        {
                            if (deltaY > 0)
                                result2 += new string('v', deltaY);
                            else if (deltaY < 0)
                                result2 += new string('^', -deltaY);
                            if (deltaX > 0)
                                result2 += new string('>', deltaX);
                            else if (deltaX < 0)
                                result2 += new string('<', -deltaX);
                            result2 += 'A';
                        }

                        if (string.IsNullOrEmpty(result1))
                            resultList[i, j] = result2;
                        else if (string.IsNullOrEmpty(result2))
                            resultList[i, j] = result1;
                        else
                        {
                            string len1 = ExecuteArrows(result1, level);
                            string len2 = ExecuteArrows(result2, level);

                            resultList[i, j] = (len1.Length < len2.Length) ? result1 : result2;
                        }
                    }    
                    else
                    {
                        if (deltaY > 0)
                            result += new string('v', deltaY);
                        else if (deltaY < 0)
                            result += new string('^', -deltaY);
                        if (deltaX > 0)
                            result += new string('>', deltaX);
                        else if (deltaX < 0)
                            result += new string('<', -deltaX);
                        result += 'A';
                        resultList[i, j] = result;
                    }                 
                }
            }
            return resultList;
        }

        public string ExecuteArrows(string data, int level)
        {
            string acursor1moves = "A" + data;
            string cursor2moves = "";
            for (int n = 0; n < acursor1moves.Length - 1; n++)
            {
                cursor2moves += Arrows(acursor1moves[n], acursor1moves[n + 1]);
            }
            if (level > 0)
            {
                return ExecuteArrows(cursor2moves, level - 1);
            }
            return cursor2moves;
        }

        public string Arrows(char from, char to)
        {
            if (from == 'A' && to == 'A') return "A";
            if (from == 'A' && to == '>') return "vA";
            if (from == 'A' && to == 'v') return "<vA";  // A<vA = v<<A, >A, >^A
            if (from == 'A' && to == '<') return "v<<A"; // 4  // Av<A = v<A, <A  >>^A, 
            if (from == 'A' && to == '^') return "<A";
            if (from == '^' && to == 'A') return ">A";
            if (from == '^' && to == '>') return ">vA";
            if (from == '^' && to == 'v') return "vA";
            if (from == '^' && to == '<') return "v<A";
            if (from == '^' && to == '^') return "A";
            if (from == '>' && to == 'A') return "^A";
            if (from == '>' && to == '>') return "A";
            if (from == '>' && to == 'v') return "<A";
            if (from == '>' && to == '<') return "<<A";
            if (from == '>' && to == '^') return "<^A";
            if (from == '<' && to == 'A') return ">>^A"; // 4
            if (from == '<' && to == '>') return ">>A";
            if (from == '<' && to == 'v') return ">A";
            if (from == '<' && to == '<') return "A";
            if (from == '<' && to == '^') return ">^A";
            if (from == '^' && to == 'A') return ">A";
            if (from == '^' && to == '>') return ">vA";
            if (from == '^' && to == 'v') return "vA";
            if (from == '^' && to == '<') return "v<A";
            if (from == '^' && to == '^') return "A";
            if (from == 'v' && to == 'A') return "^>A";
            if (from == 'v' && to == '>') return ">A";
            if (from == 'v' && to == 'v') return "A";
            if (from == 'v' && to == '<') return "<A";
            if (from == 'v' && to == '^') return "^A";
            return "WTF";
        }

        public override void Assignment1()
        {
            // Keypad Conundrum (2 robots)
            long sum = 0;

            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();

            string[,] keyMoves = InitializeKeypad(3);

            foreach (string line in lines)
            {
                Log($"Code: {line}");

                string fullLine = "A" + line;
                string keypadmoves = "";
                for (int n = 0; n < fullLine.Length - 1; n++)
                {
                    int n1 = (fullLine[n] == 'A') ? 10 : int.Parse(fullLine[n].ToString());
                    int n2 = (fullLine[n + 1] == 'A') ? 10 : int.Parse(fullLine[n + 1].ToString());
                    keypadmoves += keyMoves[n1, n2];
                }
                Log($"Moves: {keypadmoves}");

                string cursormoves = "A" + keypadmoves;
                for (int robotTimes = 0; robotTimes < 2; robotTimes++)
                {
                    string cursor2moves = "";
                    for (int n = 0; n < cursormoves.Length - 1; n++)
                    {
                        cursor2moves += Arrows(cursormoves[n], cursormoves[n + 1]);
                    }
                    Log($"Robo[{robotTimes}]: {cursormoves} [{cursormoves.Length}]");
                    cursormoves = "A" + cursor2moves;
                }

                long complexity = cursormoves.Length * int.Parse(line.Replace("A", ""));
                sum += complexity;
                Log($"==== {complexity} =====");
            }

            // 188078 Too high
            // 178258 too high
            // 176650

            LogAnswer(1, $"{sum}");
        }

        public override void Assignment2()
        {
            // Keypad Conundrum (25 robots....)
            long sum = 0;

            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();

            string[,] keyMoves = InitializeKeypad(3);

            foreach (string line in lines)
            {
                Log($"Code: {line}");

                string fullLine = "A" + line;
                string keypadmoves = "";
                for (int n = 0; n < fullLine.Length - 1; n++)
                {
                    int n1 = (fullLine[n] == 'A') ? 10 : int.Parse(fullLine[n].ToString());
                    int n2 = (fullLine[n + 1] == 'A') ? 10 : int.Parse(fullLine[n + 1].ToString());
                    keypadmoves += keyMoves[n1, n2];
                }
                Log($"Moves: {keypadmoves}");

                string akeypadmoves = "A" + keypadmoves;
                string cursor1moves = "";
                for (int n = 0; n < akeypadmoves.Length - 1; n++)
                {
                    cursor1moves += Arrows(akeypadmoves[n], akeypadmoves[n + 1]);
                }
                Log($"Robo1: {cursor1moves}");

                string acursor1moves = "A" + cursor1moves;
                string cursor2moves = "";
                for (int n = 0; n < acursor1moves.Length - 1; n++)
                {
                    cursor2moves += Arrows(acursor1moves[n], acursor1moves[n + 1]);
                }
                Log($"Robo2: {cursor2moves} [{cursor2moves.Length}]");

                long complexity = cursor2moves.Length * int.Parse(line.Replace("A", ""));
                sum += complexity;
                Log($"==== {complexity} =====");
            }

            // 188078 Too high
            // 178258 too high
            // 176650

            LogAnswer(2, $"{sum}");
        }

    }
}
