using AdventOfCodeHelpers;
using System.Linq;

namespace CodingAdvent2024
{
    internal class Day19 : DayBase
    {
        public Day19()
            : base(19)
        {
        }

        // Test data:
        // r(1), g(1), b(1), w, u
        // u in only 1 combination

        // In the real data
        // w(1), u(1), b, r(1), g(1)
        // Sol all combinations are possible, except for the B's
        // Number of b's = 411

        // Easy solution
        //public bool IsPossible(string line, int startIdx, string[] allCombinations)
        //{
        //    if (startIdx < line.Length)
        //    {
        //        foreach (string combi in allCombinations)
        //        {
        //            if (line.IndexOf(combi, startIdx) == 0)
        //            {
        //                count = 1 + IsPossible(line, startIdx + combi.Length, count, allCombinations);
        //            }
        //        }
        //    }
        //    return start;
        //}


        public override void Assignment1()
        {
            return;

            // Linen Layout
            long sum = 0;

            char[] chUsefull = { 'b' };
            if (_IsExecutingTest)
                chUsefull = new [] { 'w', 'u'};

            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            string[] allCombinations = lines[0].Split(", ");

            for(int n = 2; n < lines.Count; n++)
            {
                string line = lines[n];
                bool[] success = { true, true };
                int[] faultPos = { 0, 0 };
                foreach (char ch in chUsefull)
                {
                    string[] combinations = allCombinations.Where(a => a.Contains(ch)).ToArray();

                    for (int j = 0; j < 2; j++)
                    {
                        Array.Sort(combinations, delegate (string x, string y) { return y.Count(a => a == ch).CompareTo(x.Count(a => a == ch)); });

                        int idx = line.IndexOf(ch, 0);
                        while (idx >= 0)
                        {
                            bool found = false;
                            foreach (string combi in combinations)
                            {
                                for (int i = 0; i < combi.Length; i++)
                                {
                                    int idxOfCombo = line.IndexOf(combi, Math.Max(idx - i, 0));
                                    if (idxOfCombo != -1)
                                    {
                                        //            |  | 14
                                        // wrurggubggwbgbbbbbugugwbugruwrrbbubgbruwrubruuwbgrw
                                        // match:       bbbb 
                                        if (idxOfCombo + combi.IndexOf(ch) == idx)
                                        {
                                            found = true;
                                            idx = idxOfCombo + combi.Length - 1;
                                            break;
                                        }
                                    }
                                }
                                if (found)
                                    break;
                            }
                            
                            if (!found)
                            {
                                success[j] = false;
                                faultPos[j] = idx;
                                break;
                            }

                            // Next
                            idx = line.IndexOf(ch, idx + 1);
                        }
                    }
                }
                if (success[0] || success[1])
                {
                    sum++;
                }
                else
                {
                    if (faultPos[0] > 0)
                        Log($"{n}: {line} = INCORRECT pos: {faultPos[0]}");
                    if (faultPos[1] > 0)
                        Log($"{n}: {line} = INCORRECT pos: {faultPos[1]}");
                }
            }

            // 198 too low (counting the false ones)
            // 153 too low (counting the correct ones)
            // 202 too low
            // 239 incorrect => 8 incorrect not at 0

            // 247 is correct => I calculated 242 and I found 5 false incorrect ones I checked by hand


            LogAnswer(1, $"{sum}");
        }


        public long CountCombinations(string line, int startIdx, string[] allCombinations)
        {
            long sum = 0;
            foreach (string combi in allCombinations)
            {
                if (line.IndexOf(combi, startIdx) == startIdx)
                {
                    if (startIdx + combi.Length == line.Length)
                    {
                        sum += 1; // It fits, and we reached the end
                    }
                    else
                    {
                        sum += CountCombinations(line, startIdx + combi.Length, allCombinations);
                    }
                }
            }
            return sum;
        }


        public override void Assignment2()
        {
            // Linen Layout
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            string[] allCombinations = lines[0].Split(", ");

            int count = 0;
            long total = 0;
            for (int n = 2; n < lines.Count; n++)
            {
                string line = lines[n];
                var data = CountCombinations(line, 0, allCombinations);
                if (data > 0)
                {
                    Log($"{n}: {data}");
                    total += data;
                    count++;
                }
            }
            // 705 is too low
            LogAnswer(2, $"{total} - {count}");
        }

    }
}
