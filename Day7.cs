using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day7 : DayBase
    {
        public Day7()
            : base(7)
        {
        }

        public bool SolveMultiplyAddition(long[] numbers, long expectedResult)
        {
            int numcnt = numbers.Length;
            if (numcnt == 1)
            {
                if (numbers[0] == expectedResult)
                {
                    Log($"Found {numbers[0]}");
                    return true;
                }
                return false;
            }

            Permutations perm = new Permutations(2, numcnt - 1);
            for (int i = 0; i < perm.NumberOfSolutions; i++)
            {
                long result = numbers[0];
                string resultString = numbers[0].ToString();
                for (int j = 0; j < numcnt - 1; j++)
                {
                    if (perm[j] == 0)
                    {
                        result += numbers[j + 1];
                        resultString += " + ";
                    }
                    else
                    {
                        result *= numbers[j + 1];
                        resultString += " * ";
                    }
                    resultString += numbers[j + 1].ToString();
                }
                if (result == expectedResult)
                {
                    Log($"Found {resultString}");
                    return true;
                }
                perm.Tick();
            }
            return false;
        }
       
        public override void Assignment1()
        {
            // Fill in the operators + and *
            // Anser = 3119088655389
            long sum = 0;

            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    long expectedResult = long.Parse(parts[0]);
                    string rightPart = parts[1].Trim();
                    long[] numbers = rightPart.Split(' ').Select(a => long.Parse(a)).ToArray();

                    if (SolveMultiplyAddition(numbers, expectedResult))
                    {
                        sum += expectedResult;
                    }
                }
            }          
            LogAnswer(1, $"{sum}");
        }

        public override void Assignment2()
        {
            // Fill in the operators + or * or append numbers
            long sum = 0;

            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    long expectedResult = long.Parse(parts[0]);
                    string rightPart = parts[1].Trim();
                    string[] numbersString = rightPart.Split(' ');
                    int numCnt = numbersString.Length;
                    long[] numbers = rightPart.Split(' ').Select(a => long.Parse(a)).ToArray();

                    Permutations perm = new Permutations(3, numCnt - 1);
                    for (int i = 0; i < perm.NumberOfSolutions; i++)
                    {
                        long result = numbers[0];
                        string resultString = numbers[0].ToString();
                        for (int j = 0; j < numCnt - 1; j++)
                        {
                            if (perm[j] == 0)
                            {
                                result += numbers[j + 1];
                                resultString += " + ";
                            }
                            else if (perm[j] == 1)
                            {
                                result *= numbers[j + 1];
                                resultString += " * ";
                            }
                            else
                            {
                                result = long.Parse(result.ToString() + numbers[j + 1].ToString());
                                resultString += " || ";
                            }

                            resultString += numbers[j + 1].ToString();
                            if (result > expectedResult)
                                break;
                        }
                        if (result == expectedResult)
                        {
                            Log($"Found {resultString}");
                            sum += expectedResult;
                            break;
                        }
                        perm.Tick();
                    }

                }
            }
            LogAnswer(2, $"{sum}");
        }
    }
}
