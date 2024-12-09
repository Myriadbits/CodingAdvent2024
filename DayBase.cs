using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CodingAdvent
{
    public class DayBase
    {
        public int DayNumber { get; set; }
        protected string m_filePath = string.Empty;
        protected bool m_fShowLog = true;
        public string Filepath { get { return $"..\\..\\..\\Inputs\\ca{DayNumber}.txt"; } }
        public string FilepathTest1 { get { return $"..\\..\\..\\Inputs\\ca{DayNumber}_1_test.txt"; } }
        public string FilepathTest2 { get { return $"..\\..\\..\\Inputs\\ca{DayNumber}_2_test.txt"; } }
        public string FilepathTest { get { return $"..\\..\\..\\Inputs\\ca{DayNumber}_test.txt"; } }
        public string GetFilepath(int subNumber) { return $"..\\..\\..\\Inputs\\ca{DayNumber}-{subNumber}.txt"; }
        public Stopwatch Watch { get; set; } = new Stopwatch();
        private int m_subNumber = 0;

        protected bool _IsExecutingTest = false;

        public DayBase(int dayNumber)
        { 
            DayNumber = dayNumber;
        }

        public void Execute(bool executeTests)
        {
            Execute(executeTests, true);
        }

        public void Execute(bool executeTests, bool showLog)
        {
            _IsExecutingTest = executeTests;
            m_fShowLog = showLog;
            m_subNumber = 1;
            m_filePath = (executeTests) ? (File.Exists(FilepathTest1)) ? FilepathTest1 : (File.Exists(FilepathTest) ? FilepathTest : Filepath) : Filepath;
            Watch = Stopwatch.StartNew();
            Assignment1();
            Console.WriteLine();

            m_subNumber = 2;
            m_filePath = (executeTests) ? (File.Exists(FilepathTest2)) ? FilepathTest2 : (File.Exists(FilepathTest) ? FilepathTest : Filepath) : Filepath;
            Watch = Stopwatch.StartNew();
            Assignment2();
            Console.WriteLine();
            m_fShowLog = true;
        }

        public void ExecuteNoLog(bool executeTests)
        {
            Execute(executeTests, false);
        }

        public virtual void Assignment1()
        {
        }

        public virtual void Assignment2()
        {
        }

        public void Log(string message)
        {
            if (m_fShowLog)
            {
                Console.WriteLine($"{DateTime.Now} - [{DayNumber}-{m_subNumber}] {message}");
            }
        }

        public void LogAnswer(int subtest, string message)
        {
            string answer = $"ANSWER = {message} (in {Watch.ElapsedMilliseconds} ms)";
            string boxbox = new string('═', answer.Length); 
            Console.WriteLine($"[{DayNumber}-{m_subNumber}] ╔═{boxbox}═╗");
            Console.WriteLine($"[{DayNumber}-{m_subNumber}] ║ {answer} ║");
            Console.WriteLine($"[{DayNumber}-{m_subNumber}] ╚═{boxbox}═╝");
        }

        public char[][] Create2DCA(int sizeX, int sizeY, char defaultChar)
        {
            char[][] data2d = new char[sizeY][];
            for (int i = 0; i < sizeY; i++)
            {
                data2d[i] = new char[sizeX];
                Array.Fill(data2d[i], defaultChar);
            }
            return data2d;
        }

        public char[][] Load2DCA(string filePath)
        {
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            int sizeY = lines.Count;
            int sizeX = lines[0].Length;
            char[][] inputs = Create2DCA(sizeX, sizeY, '.');

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    inputs[y][x] = lines[y][x];
                }
            }
            return inputs;
        }

        public void Log2DCA(char[][] data)
        {
            foreach (var item in data)
            {
                Console.WriteLine($"[{DayNumber}-{m_subNumber}] {new string(item)}");                
            }
        }

        public void Log2DMap(Map2D map)
        {
            map.Print(DayNumber, m_subNumber);
        }


        public void MarkPosition(char[][] data, Position pos)
        {
            data[pos.Y][pos.X] |= (char)(pos.Direction);
        }

        public bool HasVisitedWIthSameDirection(char[][] data, Position pos)
        {
            return (data[pos.Y][pos.X] & (char)(pos.Direction)) == (char)(pos.Direction);
        }

        ///
        /// Helper methods
        ///

        public static T GreatestCommonDivisor<T>(T n1, T n2) where T : INumber<T>
        {
            if (n2 == T.Zero)
            {
                return n1;
            }
            else
            {
                return GreatestCommonDivisor(n2, n1 % n2);
            }
        }

        protected static T LowestCommonMultiplier<T>(T[] numbers) where T : INumber<T>
        {
            return numbers.Aggregate((S, val) => S * val / GreatestCommonDivisor(S, val));
        }

        public List<string> PivotStringList(List<string> input)
        {
            List<string> output = new List<string>();
            for (int x = 0; x < input[0].Length; x++)
            {
                string newLine = string.Empty;
                for (int y = 0; y < input.Count; y++)
                {
                    newLine += input[y][x];
                }
                output.Add(newLine);
            }
            return output;
        }

        public List<char[]> PivotCharArrayList(List<char[]> input)
        {
            List<char[]> output = new List<char[]>();
            for (int x = 0; x < input[0].Length; x++)
            {
                char[] newLine = new char[input.Count];
                for (int y = input.Count - 1; y >= 0; y--)
                {
                    newLine[y] = input[y][x];
                }
                output.Add(newLine);
            }
            return output;
        }

        // Please use list.SequenceEqual(list2) :)
        public bool CompareCharArray(char[] list1, char[] list2)
        {
            for (int n = 0; n < list1.Length && n < list2.Length; n++)
            {
                if (list1[n] != list2[n])
                    return false;
            }
            return true;
        }

        public int[,,] Map3DDeepCopy(int[,,] map)
        {
            int[,,] mapCopy = new int[map.GetLength(0), map.GetLength(1), map.GetLength(2)];
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    for (int z = 0; z < map.GetLength(2); z++)
                        mapCopy[x, y, z] = map[x, y, z];
            return mapCopy;
        }

        public int Print2DMap(int[,] data, List<Position> positions)
        {
            int counter = 0;
            for (int y = 0; y < data.GetLength(1); y++)
            {
                string line = "";
                for (int x = 0; x < data.GetLength(0); x++)
                {
                    if (positions.Where(a => a.X == x && a.Y == y).Any())
                    {
                        line += 'O';
                        counter++;
                    }
                    else if (data[x, y] == 0)
                        line += '.';
                    else if (data[x, y] == 1)
                        line += '#';
                    else
                        line += Convert.ToChar(data[x, y]);
                }
                Log($"{line}");
            }
            return counter;
        }


        
    }
}
