using AdventOfCodeHelpers;
using System.Collections.Generic;
using System;

namespace CodingAdvent2024
{
    internal class Day22 : DayBase
    {
        public Day22()
            : base(22)
        {
        }

        public override void Assignment1()
        {
            // Monkey Market, calculate the secret numbers
            long sum = 0;

            List<long> numbers = ReadNumberPerLine();

            foreach(long secretnumber in numbers)
            {
                //long secretnumber = 123;
                long newnumber = secretnumber;
                for (int n = 0; n < 2000; n++)
                {
                    newnumber = (newnumber ^ (newnumber << 6)) & 0xFFFFFF;
                    newnumber = (newnumber ^ (newnumber >> 5)) & 0xFFFFFF;
                    newnumber = (newnumber ^ (newnumber << 11)) & 0xFFFFFF;
                }
                sum += newnumber;
                Log($"{secretnumber}: {newnumber}");
            }

            LogAnswer(1, $"{sum}");
        }

        public override void Assignment2()
        {
            // Monkey Market. Get the most bananas
            long sum = 0;

            List<long> numbers = ReadNumberPerLine();

            Dictionary<string, int> sequences = new Dictionary<string, int>();
            foreach (long secretnumber in numbers)
            {
                Dictionary<string, bool> sequenceAdded = new Dictionary<string, bool>();

                //long secretnumber = 123;
                long newnumber = secretnumber;
                List<int> lastNumbers = new List<int>();
                for (int n = 0; n < 2000; n++)
                {
                    newnumber = (newnumber ^ (newnumber << 6)) & 0xFFFFFF;
                    newnumber = (newnumber ^ (newnumber >> 5)) & 0xFFFFFF;
                    newnumber = (newnumber ^ (newnumber << 11)) & 0xFFFFFF;
                    int bananas = (int)(newnumber % 10);
                    lastNumbers.Add(bananas);
                    if (lastNumbers.Count > 4)
                    {
                        int delta1 = lastNumbers[lastNumbers.Count - 1] - lastNumbers[lastNumbers.Count - 2];
                        int delta2 = lastNumbers[lastNumbers.Count - 2] - lastNumbers[lastNumbers.Count - 3];
                        int delta3 = lastNumbers[lastNumbers.Count - 3] - lastNumbers[lastNumbers.Count - 4];
                        int delta4 = lastNumbers[lastNumbers.Count - 4] - lastNumbers[lastNumbers.Count - 5];
                        string sequence = $"{delta4},{delta3},{delta2},{delta1}";
                        if (!sequences.ContainsKey(sequence))
                        {
                            sequences[sequence] = bananas;
                            sequenceAdded[sequence] = true;
                        }   
                        else
                        {
                            if (!sequenceAdded.ContainsKey(sequence) || !sequenceAdded[sequence])
                            {
                                sequences[sequence] += bananas;
                                sequenceAdded[sequence] = true;
                            }
                        }
                    }
                }
            }

            var mostbananas = sequences.OrderByDescending(a => a.Value);
            sum = mostbananas.First().Value;
            for (int n = 0; n < 10; n++)
            {
                var item = mostbananas.ElementAt(n);
                Log($"{item.Key} = {item.Value}");
            }

            LogAnswer(1, $"{sum}");
        }

    }
}
