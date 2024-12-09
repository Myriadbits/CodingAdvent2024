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
                if (input.Data[y][x] == emptyChar)
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
    }
}
