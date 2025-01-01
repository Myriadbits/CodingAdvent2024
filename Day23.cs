using AdventOfCodeHelpers;
using System.Collections.Immutable;

namespace CodingAdvent2024
{
    internal class Day23 : DayBase
    {
        public Day23()
            : base(23)
        {
        }

        private List<string> m_foundPaths = new List<string>();

        private void CheckCircle(Dictionary<string, List<string>> links, List<string> options, int level, string path, string startPC)
        {
            foreach (string nextPc in options)
            {
                if (level == 0)
                {
                    if (nextPc == startPC)
                    {
                        //Log($"Path: {path}");
                        m_foundPaths.Add(path);
                    }
                }
                else
                {
                    CheckCircle(links, links[nextPc], level - 1, path + "," + nextPc, startPC);
                }
            }
        }

        public override void Assignment1()
        {
            // LAN Party
            long sum = 0;

            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            Dictionary<string, List<string>> links = new Dictionary<string, List<string>>();
            foreach (string line in lines)
            {
                string[] pcs = line.Split('-');
                if (!links.ContainsKey(pcs[0]))
                    links[pcs[0]] = new List<string>();
                links[pcs[0]].Add(pcs[1]);
                if (!links.ContainsKey(pcs[1]))
                    links[pcs[1]] = new List<string>();
                links[pcs[1]].Add(pcs[0]);
            }

            foreach (var link in links)
            {
                CheckCircle(links, link.Value, 2, link.Key, link.Key);
            }

            foreach(string s in m_foundPaths)
            {
                string[] pcs = s.Split(',');
                sum += (pcs.Count(a => a.StartsWith('t')) > 0) ? 1 : 0;
            }

            // Correct answer: 1083
            LogAnswer(1, $"{sum / 6}"); // There are always 6 different ways to connect 3 PCs. Since I counted them all, divide by 3
        }

        public override void Assignment2()
        {
            // LAN Party (find the largest network of interconnected PC's)
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            Dictionary<string, List<string>> links = new Dictionary<string, List<string>>();
            foreach (string line in lines)
            {
                string[] pcs = line.Split('-');
                if (!links.ContainsKey(pcs[0]))
                    links[pcs[0]] = new List<string>();
                links[pcs[0]].Add(pcs[1]);
                if (!links.ContainsKey(pcs[1]))
                    links[pcs[1]] = new List<string>();
                links[pcs[1]].Add(pcs[0]);
            }

            int maxDepth = 0;
            foreach (var link in links)
                maxDepth = Math.Max(maxDepth, link.Value.Count);
            Log($"Maxdepth: {maxDepth}");

            // Start checking the maximum depth - 1:

            List<string> links2 = new List<string>();
            foreach (var link in links)
            {
                for (int n = 0; n < link.Value.Count; n++)
                {
                    List<string> newList = new List<string>(link.Value);
                    newList.RemoveAt(n); // Remove a single item (to get to the 13)
                    newList.Add(link.Key);
                    newList.Sort(); // Sort so we can count the equal items
                    links2.Add(string.Join(',', newList));
                }
            }

            string answer = "";
            foreach (string link in links2)
            {
                int count = links2.Count(a => a == link);
                if (count >= maxDepth)
                {
                    Log($"{link}: {count}");
                    answer = link;
                    break;
                }
            }

            // Correct answer: as,bu,cp,dj,ez,fd,hu,it,kj,nx,pp,xh,yu
            LogAnswer(2, $"{answer}");
        }

    }
}
