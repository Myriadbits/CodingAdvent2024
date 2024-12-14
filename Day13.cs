using CodingAdvent2024;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodingAdvent
{
    internal class Day13 : DayBase
    {
        public Day13()
            : base(13)
        {
        }

        (double x, double y) GetData(string line)
        {
            Regex exp = new Regex(@"\D+(\d+),\D+(\d+)");
            Match mA = exp.Match(line);
            return (long.Parse(mA.Groups[1].ToString()), long.Parse(mA.Groups[2].ToString()));
        }

        public override void Assignment1()
        {
            // Claw Contraption
            long sum = 0;
            List<string> lines = File.ReadLines(m_filePath).ToList();

            //Button A: X+94, Y+34
            //Button B: X+22, Y+67
            //Prize: X=8400, Y=5400

            Regex exp = new Regex(@"\D+(\d+),\D+(\d+)");
            for (int i = 0; i < lines.Count; i += 4)
            {
                var buttonA = GetData(lines[i + 0]);
                var buttonB = GetData(lines[i + 1]);
                var result = GetData(lines[i + 2]);

                var times = LinearEquations.SolveDoubleLinearCheckLong(buttonA.x, buttonB.x, result.x, buttonA.y, buttonB.y, result.y);
                if (times.success)
                {
                    sum += times.x * 3 + times.y;
                    Log($"Times: A={times.x} - B={times.y}");
                }
            }
            // Good answer: 29711
            LogAnswer(1, $"{sum}");
        }

        public override void Assignment2()
        {
            // Claw Contraption part 2 + 10000000000000.
            long sum = 0;
            List<string> lines = File.ReadLines(m_filePath).ToList();

            Regex exp = new Regex(@"\D+(\d+),\D+(\d+)");
            for (int i = 0; i < lines.Count; i += 4)
            {
                var buttonA = GetData(lines[i + 0]);
                var buttonB = GetData(lines[i + 1]);
                var result = GetData(lines[i + 2]);

                var times = LinearEquations.SolveDoubleLinearCheckLong(buttonA.x, buttonB.x, result.x + 10000000000000, buttonA.y, buttonB.y, result.y + 10000000000000);
                if (times.success)
                {
                    sum += times.x * 3 + times.y;
                    Log($"Times: A={times.x} - B={times.y}");
                }
            }
            // Good answer: 94955433618919
            LogAnswer(2, $"{sum}");
        }
    }
}
