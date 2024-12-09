using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodingAdvent
{
    internal class Day3 : DayBase
    {
        public Day3()
            : base(3)
        {
        }
       
        public override void Assignment1()
        {
            long sum = 0;

            foreach (string line in System.IO.File.ReadLines(m_filePath).ToList())
            {
                for (int n = 0; n < line.Length - 8; n++)
                {
                    if (line[n] == 'm' && line[n + 1] == 'u' && line[n + 2] == 'l' && line[n + 3] == '(')
                    {
                        int endMarker = line.IndexOf(')', n + 4);
                        if (endMarker <= n + 11)
                        {
                            int commaMarker = line.IndexOf(',', n + 4);
                            if (commaMarker <= n + 10)
                            {
                                string snumber1 = line.Substring(n + 4, commaMarker - n - 4);
                                string snumber2 = line.Substring(commaMarker + 1, endMarker - commaMarker - 1);
                                if (snumber1.All(char.IsDigit) && snumber2.All(char.IsDigit))
                                {
                                    int number1 = int.Parse(snumber1);
                                    int number2 = int.Parse(snumber2);
                                    Log($"{number1}*{number2} = {number1 * number2}");
                                    sum += number1 * number2;
                                }
                            }
                        }
                    }
                }
            }
            LogAnswer(1, $"{sum}");
        }

        public override void Assignment2()
        {
            long sum = 0;
            bool fon = true;

            foreach (string line in System.IO.File.ReadLines(m_filePath).ToList())
            {
                for (int n = 0; n < line.Length - 8; n++)
                {
                    if (line.Substring(n, 4) == "do()")
                    {
                        fon = true;
                    }
                    else if (line.Substring(n, 7) == "don't()")
                    {
                        fon = false;
                    }
                    else if (line.Substring(n, 4) == "mul(")
                    {
                        int endMarker = line.IndexOf(')', n + 4);
                        if (endMarker <= n + 11)
                        {
                            int commaMarker = line.IndexOf(',', n + 4);
                            if (commaMarker <= n + 10)
                            {
                                string snumber1 = line.Substring(n + 4, commaMarker - n - 4);
                                string snumber2 = line.Substring(commaMarker + 1, endMarker - commaMarker - 1);
                                if (snumber1.All(char.IsDigit) && snumber2.All(char.IsDigit))
                                {
                                    int number1 = int.Parse(snumber1);
                                    int number2 = int.Parse(snumber2);
                                    Log($"{number1}*{number2} = {number1 * number2}");
                                    if (fon)
                                        sum += number1 * number2;
                                }
                            }
                        }
                    }
                }
            }
            LogAnswer(2, $"{sum}");
        }
    }
}
