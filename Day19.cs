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

        public long CountCombinations(string line, int startIdx, Dictionary<char, List<string>> combos, Dictionary<int, long> foundItems, string output)
        {
            long sum = 0;
            char ch = line[startIdx];
            foreach (string combi in combos[ch])
            {
                if ((startIdx + combi.Length) > line.Length)
                    continue;
                bool match = true;
                for (int i = 0; i < combi.Length; i++)
                {
                    if (line[i + startIdx] != combi[i])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    if (startIdx + combi.Length == line.Length)
                    {
                        sum += 1; // It fits, and we reached the end
                        Log($"Match: {output},{combi} + [{sum}] <");
                    }
                    else
                    {
                        // It fits, count 
                        int idx = startIdx + combi.Length;
                        if (foundItems.ContainsKey(idx))
                        {
                            //long newsum = foundItems[idx];
                            //sum += newsum;
                            //Log($"Match: {output},{combi} + [{newsum}]");
                        }
                        else
                        {
                            long newsum = CountCombinations(line, idx, combos, foundItems, output + "," + combi);
                            if (newsum > 0)
                            {
                                sum += newsum;
                                //foundItems[idx] = newsum;
                                Log($"Match: {output},{combi} + [{newsum}]");
                            }
                        }
                    }
                }
            }
            foundItems[startIdx] = sum;
            return sum;
        }


        public override void Assignment2()
        {
            // Linen Layout
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            string[] allCombinations = lines[0].Split(", ");

            // w(1), u(1), b, r(1), g(1)
            Dictionary<char, List<string>> combos = new Dictionary<char, List<string>>();
            combos.Add('w', allCombinations.Where(a => a.StartsWith('w')).ToList());
            combos.Add('u', allCombinations.Where(a => a.StartsWith('u')).ToList());
            combos.Add('r', allCombinations.Where(a => a.StartsWith('r')).ToList());
            combos.Add('g', allCombinations.Where(a => a.StartsWith('g')).ToList());
            combos.Add('b', allCombinations.Where(a => a.StartsWith('b')).ToList());

            //Array.Sort(combinations, delegate (string x, string y) { return y.Count(a => a == ch).CompareTo(x.Count(a => a == ch)); });

            //string line = "grwwbwuwuwwrruwwrrwwbgrgwrggwurguuwwgburwbgwuuru";



            int count = 0;
            long total = 0;
            for (int n = 2; n < lines.Count; n++)
            {
                string line = lines[n];
                Log($"--------------");
                Log($"{n}: {line}");
                Dictionary<int, long> foundItems = new Dictionary<int, long>();
                var data = CountCombinations(line, 0, combos, foundItems, "");
                if (data > 0)
                {
                    Log($"{n}: {data}");
                    total += data;
                    count++;
                }
                else
                {
                    Log($"{n}: {data}");
                }
            }
            // 705 is too low
            // 1007396318504191 is too high
            //     600467533368
            LogAnswer(2, $"{total} - {count}");
        }

    }
}
