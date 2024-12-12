using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodingAdvent
{
    public class Map2D
    {
        private char[][] m_data;

        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public char[][] Data
        {
            get
            {
                return m_data;
            }
        }

        /// <summary>
        /// Create an empty map
        /// </summary>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        public Map2D(int sizeX, int sizeY, char defaultChar = '.')
        {
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            m_data = new char[this.SizeY][];
            for (int y = 0; y < this.SizeY; y++)
            {
                m_data[y] = new char[this.SizeX];
                Array.Fill(m_data[y], defaultChar);
            }
        }

        /// <summary>
        /// Load a map from file
        /// </summary>
        /// <param name="filePath"></param>
        public Map2D(string filePath)
        {
            List<string> lines = System.IO.File.ReadLines(filePath).ToList();
            this.SizeY = lines.Count;
            this.SizeX = lines[0].Length;
            m_data = new char[this.SizeY][];
            for (int y = 0; y < this.SizeY; y++)
            {
                m_data[y] = new char[this.SizeX];
                for (int x = 0; x < this.SizeX; x++)
                {
                    m_data[y][x] = lines[y][x];
                }
            }
        }

        /// <summary>
        /// Check if a point is of a specific value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public bool IsValue(int x, int y, char ch)
        {
            return m_data[y][x] == ch;
        }

        /// <summary>
        /// Returns true if a position contains a value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ch"></param>
        public bool IsPositionValue(Position pos, char ch)
        {
            if (pos.X >= 0 && pos.X < this.SizeX &&
                pos.Y >= 0 && pos.Y < this.SizeY)
            {
                return m_data[pos.Y][pos.X] == ch;
            }
            return false;
        }

        /// <summary>
        /// Returns true if a position is in the bounds
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool IsInBounds(int x, int y)
        {
            return (x >= 0 && x < this.SizeX && y >= 0 && y < this.SizeY);
        }

        /// <summary>
        /// Set a character in the map and check the boundaries
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ch"></param>
        public bool SetInBounds(int x, int y, char ch)
        {
            if (x >= 0 && x < this.SizeX &&
                y >= 0 && y < this.SizeY)
            {
                m_data[y][x] = ch;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set a character in the map and check the boundaries
        /// And check if the input map is empty at that point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ch"></param>
        public void SetInBoundsNoOverwriteInput(int x, int y, char ch, Map2D input, char emptyChar = '.')
        {
            if (x >= 0 && x < this.SizeX &&
                y >= 0 && y < this.SizeY)
            {
                if (input.m_data[y][x] == emptyChar)
                {
                    m_data[y][x] = ch;
                }
            }
        }

        /// <summary>
        /// Print the map to the console
        /// </summary>
        public void Print(int dayNumber, int subNumber)
        {
            foreach (var line in m_data)
            {
                Console.WriteLine($"[{dayNumber}-{subNumber}] {new string(line)}");
            }
        }

        /// <summary>
        /// Count the number of specific characters
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public int Count(char ch)
        {
            int count = 0;
            for (int y = 0; y < this.SizeY; y++)
            {
                for (int x = 0; x < this.SizeX; x++)
                {
                    if (m_data[y][x] == ch)
                        count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Determine the cicumfence of an area
        /// It is a circumfence if it is on the outside, or the pixel next to it is another value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public long GetCircumfence(int x, int y)
        {
            long circumfence = 0;
            if (x == 0) circumfence++;
            if (y == 0) circumfence++;
            if (x == SizeX - 1) circumfence++;
            if (y == SizeY - 1) circumfence++;

            if (x > 0 && m_data[y][x] != m_data[y][x - 1]) circumfence++;
            if (y > 0 && m_data[y][x] != m_data[y - 1][x]) circumfence++;
            if (x < SizeX - 1 && m_data[y][x] != m_data[y][x + 1]) circumfence++;
            if (y < SizeY - 1 && m_data[y][x] != m_data[y + 1][x]) circumfence++;
            return circumfence;
        }

        /// <summary>
        /// Check if 2 adjacent pixels are the different
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dir">BITFIELDS</param>
        /// <returns></returns>
        public bool IsAdjacentDiff(int x, int y, EDirection dir)
        {
            bool ret = true;
            if ((dir & EDirection.North) != 0)
                ret = ret && (y == 0 || m_data[y][x] != m_data[y - 1][x]);
            if ((dir & EDirection.East) != 0)
                ret = ret && (x == (SizeX - 1) || m_data[y][x] != m_data[y][x + 1]);
            if ((dir & EDirection.South) != 0)
                ret = ret && (y == (SizeY - 1) || m_data[y][x] != m_data[y + 1][x]);
            if ((dir & EDirection.West) != 0)
                ret = ret && (x == 0 || m_data[y][x] != m_data[y][x - 1]);
            return ret;
        }

        /// <summary>
        /// Check if 2 adjacent pixels are the same
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dir">BITFIELDS</param>
        /// <returns></returns>
        public bool IsAdjacentSame(int x, int y, EDirection dir)
        {
            bool ret = true;
            if ((dir & EDirection.North) != 0)
                ret = ret && (y > 0 && m_data[y][x] == m_data[y - 1][x]);
            if ((dir & EDirection.East) != 0)
                ret = ret && (x < (SizeX - 1) && m_data[y][x] == m_data[y][x + 1]);
            if ((dir & EDirection.South) != 0)
                ret = ret && (y < (SizeY - 1) && m_data[y][x] == m_data[y + 1][x]);
            if ((dir & EDirection.West) != 0)
                ret = ret && (x > 0 && m_data[y][x] == m_data[y][x - 1]);
            return ret;
        }

        /// <summary>
        /// Returns true if a diagonal is different
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool IsDiagDiff(int x, int y, EDirection dir)
        {
            if (dir == EDirection.NorthEast)
                return (x < SizeX - 1 && y > 0 && m_data[y][x] != m_data[y - 1][x + 1]);
            if (dir == EDirection.SouthEast)
                return (x < SizeX - 1 && y < SizeY - 1 && m_data[y][x] != m_data[y + 1][x + 1]);
            if (dir == EDirection.SouthWest)
                return (x > 0 && y < SizeY - 1 && m_data[y][x] != m_data[y + 1][x - 1]);
            if (dir == EDirection.NorthWest)
                return (x > 0 && y > 0 && m_data[y][x] != m_data[y - 1][x - 1]);
            return false;
        }

        /// <summary>
        /// Return true if the pixel is a corner
        /// Corners are at the corner of the image,
        /// or when the up/left, up/right, down/right or down/left are not equal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int CountCorners(int x, int y)
        {
            int corners = 0;

            // Count outside corners
            if (IsAdjacentDiff(x, y, EDirection.NorthEast)) corners++;
            if (IsAdjacentDiff(x, y, EDirection.SouthEast)) corners++;
            if (IsAdjacentDiff(x, y, EDirection.SouthWest)) corners++;
            if (IsAdjacentDiff(x, y, EDirection.NorthWest)) corners++;

            // Count inner corners
            if (IsAdjacentSame(x, y, EDirection.NorthEast) && IsDiagDiff(x, y, EDirection.NorthEast)) corners++;
            if (IsAdjacentSame(x, y, EDirection.SouthEast) && IsDiagDiff(x, y, EDirection.SouthEast)) corners++;
            if (IsAdjacentSame(x, y, EDirection.SouthWest) && IsDiagDiff(x, y, EDirection.SouthWest)) corners++;
            if (IsAdjacentSame(x, y, EDirection.NorthWest) && IsDiagDiff(x, y, EDirection.NorthWest)) corners++;

            return corners;
        }

        /// <summary>
        /// Fast floodfill an area
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fillChar"></param>
        /// <param name="visitedValue"></param>
        /// <returns>(area, circumfence)</returns>
        public (long area, long circumfence, long corners) FloodFillFast(int x, int y, char fillChar, Map2D originalMap, char visitedValue = '-')
        {
            Stack<Position> pixels = new Stack<Position>();

            pixels.Push(new Position(x, y));
            long area = 0;
            long circumfence = 0;
            long corners = 0;
            while (pixels.Count > 0)
            {
                // Go right
                Position pos = pixels.Pop();
                int previousUp = 1;
                int previousDown = 1;
                for (int x1 = pos.X; x1 < SizeX; x1++)
                {
                    if (m_data[pos.Y][x1] == fillChar)
                    {
                        m_data[pos.Y][x1] = visitedValue;
                        area++;
                        circumfence += originalMap.GetCircumfence(x1, pos.Y);
                        corners += originalMap.CountCorners(x1, pos.Y);
                    }
                    else
                    {
                        break;
                    }

                    if (pos.Y > 0)
                    {
                        if (previousUp != 0 && m_data[pos.Y - 1][x1] == fillChar)
                        {
                            pixels.Push(new Position(x1, pos.Y - 1));
                        }
                        previousUp = m_data[pos.Y - 1][x1];
                    }

                    if (pos.Y < SizeY - 1)
                    {
                        if (previousDown != 0 && m_data[pos.Y + 1][x1] == fillChar)
                        {
                            pixels.Push(new Position(x1, pos.Y + 1));
                        }
                        previousDown = m_data[pos.Y + 1][x1];
                    }
                }

                for (int x1 = pos.X - 1; x1 >= 0; x1--)
                {
                    if (m_data[pos.Y][x1] == fillChar)
                    {
                        m_data[pos.Y][x1] = visitedValue;
                        area++;
                        circumfence += originalMap.GetCircumfence(x1, pos.Y);
                        corners += originalMap.CountCorners(x1, pos.Y);
                    }
                    else
                    {
                        break;
                    }

                    if (pos.Y > 0)
                    {
                        if (previousUp != 0 && m_data[pos.Y - 1][x1] == fillChar)
                        {
                            pixels.Push(new Position(x1, pos.Y - 1));
                        }
                        previousUp = m_data[pos.Y - 1][x1];
                    }

                    if (pos.Y < SizeY - 1)
                    {
                        if (previousDown != 0 && m_data[pos.Y + 1][x1] == fillChar)
                        {
                            pixels.Push(new Position(x1, pos.Y + 1));
                        }
                        previousDown = m_data[pos.Y + 1][x1];
                    }
                }
            }
            return (area, circumfence, corners);
        }
    }
}
