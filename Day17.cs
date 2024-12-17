using AdventOfCodeHelpers;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;

namespace CodingAdvent2024
{
    internal class Day17 : DayBase
    {
        public Day17()
            : base(17)
        {
        }

        long m_registerA = 0;
        long m_registerB = 0;
        long m_registerC = 0;

        public int[] ExecuteProgram(int[] code, long value)
        {
            m_registerA = value;
            m_registerB = 0;
            m_registerC = 0;
            List<int> outputs = new List<int>();
            for (int j = 0; j < code.Length; j += 2)
            {
                int operand = code[j + 1];
                long combo = 0;
                if (operand < 4) combo = (long) operand;
                if (operand == 4) combo = m_registerA;
                if (operand == 5) combo = m_registerB;
                if (operand == 6) combo = m_registerC;
                switch (code[j])
                {
                    case 0: // adv
                        m_registerA = m_registerA >> (int)combo;
                        break;
                    case 1: // bxl
                        m_registerB = m_registerB ^ operand;
                        break;
                    case 2: // bst
                        m_registerB = combo % 8;
                        break;
                    case 3: // jnz
                        if (m_registerA != 0)
                        {
                            j = operand - 2;
                            continue;
                        }
                        // Do nothing
                        break;
                    case 4: // bxc
                        m_registerB = m_registerB ^ m_registerC;
                        break;
                    case 5: // out
                        outputs.Add((int)(combo % 8));
                        break;
                    case 6: // bdv
                        m_registerB = m_registerA >> (int)combo;
                        break;
                    case 7: // cdv
                        m_registerC = m_registerA >> (int)combo;
                        break;
                }
            }
            return outputs.ToArray();
        }

        public override void Assignment1()
        {
            // Chronospatial Computer
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            Regex exp = new Regex(@"Register A: (\d+)");
            Match mA = exp.Match(lines[0]);
            long startValue = int.Parse(mA.Groups[1].ToString());
            string codeString = lines[4].Substring(9);
            int[] code = codeString.Split(',').Select(a => int.Parse(a)).ToArray();

            int[] outputs = ExecuteProgram(code, startValue);

            string output = string.Join(",", outputs);
            Log($"{output}");

            // Correct answer: 7,3,1,3,6,3,6,0,2
            LogAnswer(1, $"{output}");
        }

        public bool ExecuteProgramCheckValue(int[] code, long value, int[] codeToCheck)
        {
            m_registerA = value;
            m_registerB = 0;
            m_registerC = 0;
            int index = 0;
            for (int j = 0; j < code.Length; j += 2)
            {
                int operand = code[j + 1];
                long combo = operand;
                if (operand == 4) combo = m_registerA;
                if (operand == 5) combo = m_registerB;
                if (operand == 6) combo = m_registerC;
                switch (code[j])
                {
                    case 0: // adv
                        m_registerA = m_registerA >> (int)combo;
                        break;
                    case 1: // bxl
                        m_registerB = m_registerB ^ operand;
                        break;
                    case 2: // bst
                        m_registerB = combo & 7;
                        break;
                    case 3: // jnz
                        if (m_registerA != 0)
                        {
                            j = operand - 2;
                            continue;
                        }
                        // Do nothing
                        break;
                    case 4: // bxc
                        m_registerB = m_registerB ^ m_registerC;
                        break;
                    case 5: // out
                        if ((combo & 7) == codeToCheck[index])
                        {
                            index++;
                            if (index >= codeToCheck.Length)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case 6: // bdv
                        m_registerB = m_registerA >> (int)combo;
                        break;
                    case 7: // cdv
                        m_registerC = m_registerA >> (int)combo;
                        break;
                }
            }
            return false;
        }

        public override void Assignment2()
        {
            // Chronospatial Computer output should be input
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            Regex exp = new Regex(@"Register A: (\d+)");
            Match mA = exp.Match(lines[0]);
                        
            string codeString = lines[4].Substring(9);
            int[] code = codeString.Split(',').Select(a => int.Parse(a)).ToArray();

            Stopwatch sw = Stopwatch.StartNew();

            // A = A >> 3 // To get 16 digits, we need a 16 * 3 bit number = 48 = 6 byte
            // First determine lowest bytes for 8 matches
            long lowestBytes = 0;
            for (long n = 0; n < 0xFFFFFF; n++) // Yes, use one F extra (so check some overlap). We need 24 bits, check 28 bits
            {
                if (ExecuteProgramCheckValue(code, n, code.Take(7).ToArray()))
                {
                    // Now show the output
                    int[] outputs1 = ExecuteProgram(code, n);
                    Log($"{n}: 0x{n:X} - {string.Join(",", outputs1)}");
                    lowestBytes = n & 0xFFFFF;
                    break;
                }
            }
            // Take lower 7*3 bytes = 20 bits
            // 0x32299A
            long notBestValue = 0;
            int shifter = 20;
            for (long n = 0; n < 0xFFFFFFF; n++)
            {
                long value = (n << shifter) + lowestBytes;
                if (ExecuteProgramCheckValue(code, value, code.ToArray()))
                {
                    // Found it, show the output
                    notBestValue = value;
                    int[] outputs2 = ExecuteProgram(code, notBestValue);
                    Log($"{notBestValue}: 0x{notBestValue:X} - {string.Join(",", outputs2)}");
                    break;
                }
            }

            // Now loop down, check if find a lower number
            long finalAnswer = 0;
            for (long counter = 0; counter < 100000000; counter++)
            {
                notBestValue--;
                if (ExecuteProgramCheckValue(code, notBestValue, code.ToArray()))
                {
                    finalAnswer = notBestValue;
                    int[] outputs2 = ExecuteProgram(code, finalAnswer);
                    Log($"{finalAnswer}: 0x{finalAnswer:X} - {string.Join(",", outputs2)}");
                }
            }
            // final value 6043A932299A

            // Final value 6043A921699A

            // 6 8 3 2 2 9 9 A
            // 2 4 1 5 7 5 1 6 0 3

            // 2,4, 1,5, 7,5, 1,6, 0,3, 4,0, 5,5, 3,0
            // bst B = A & 7
            // bxl B = B ^ 5
            // cdv C = A >> B
            // bxl B = B ^ 6
            // adv A = A >> 8
            // bxc B = B ^ C
            // out OUT = B & 7
            // jnz 0

            // No overcarry B and C from last loop

            // A = A >> 3 // To get 16 digits, we need a 16 * 3 bit number
            // 
            // B = ((A & 7) ^ 5) ^ 6  => (A & 7) ^ 3
            // C = A >> ((A & 7) ^ 5)
            // Out = B ^ C


            // Checked upto 265970000000

            // My answer: 105843717712282 was too high
            // Final answer: 105843716614554 is correct

            LogAnswer(2, $"{finalAnswer}");
        }

    }
}
