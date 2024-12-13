using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingAdvent2024
{
    public class LinearEquations
    {

        /// <summary>
        /// Solve 2 linear equations
        /// a1*x + a1*y = c1
        /// a2*x + a2*y = c2
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="b1"></param>
        /// <param name="c1"></param>
        /// <param name="a2"></param>
        /// <param name="b2"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        /// 
        //Button A: X+94, Y+34 
        //Button B: X+22, Y+67 
        // 94A + 22B = 8400
        // 34A + 67B = 5400
        public static (double x, double y) SolveDoubleLinear(double a1, double b1, double c1, double a2, double b2, double c2)
        {
            double y = (c1 - a1 * c2 / a2) / (b1 - a1 * b2 / a2);
            double x = (c2 - b2 * y) / a2;
            return (x, y);
        }

        /// <summary>
        /// Solve a linear equation & check the results as longs
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="b1"></param>
        /// <param name="c1"></param>
        /// <param name="a2"></param>
        /// <param name="b2"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static (bool success, long x, long y) SolveDoubleLinearCheckLong(double a1, double b1, double c1, double a2, double b2, double c2)
        {
            var result = SolveDoubleLinear(a1, b1, c1, a2, b2, c2);

            long lTimesA = (long)Math.Round(result.x);
            long lTimesB = (long)Math.Round(result.y);
            if (lTimesA * a1 + lTimesB * b1 == c1 &&
                lTimesA * a2 + lTimesB * b2 == c2)
            {
                return (true, lTimesA, lTimesB);
            }
            return (false, 0, 0);
        }
    }
}
