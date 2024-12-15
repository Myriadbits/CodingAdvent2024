using System.Numerics;
using AdventOfCodeHelpers;

namespace CodingAdvent2024
{
    internal class Day9 : DayBase
    {
        public Day9()
            : base(9)
        {
        }
       
        public override void Assignment1()
        {
            // Disk space compacting blocks

            // Only a single line
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            string line = lines[0].Trim();

            // Get all files
            List<long> fileBlocks = new List<long>();
            long id = 0;
            for(int i = 0; i < line.Length; i += 2)
            {
                int numTimes = int.Parse(line.Substring(i, 1));
                for (int cnt = 0; cnt < numTimes; cnt++)
                    fileBlocks.Add(id);
                id++;
            }
            //Log($"{string.Join(",", fileBlocks)}");

            BigInteger biSum = new BigInteger(0);
            List<long> allBlocks = new List<long>();
            long index = 0;
            long idQueue = 0;
            int idx = 0;
            int startIdx = 0;
            int endIdx = fileBlocks.Count - 1;
            while (startIdx <= endIdx)
            {
                int numTimes = int.Parse(line.Substring(idx, 1));
                for (int cnt = 0; cnt < numTimes; cnt++)
                {
                    if (startIdx > endIdx)
                        break;
                    if (idx % 2 == 0)
                    {
                        idQueue = fileBlocks[startIdx];
                        startIdx++;
                    }
                    else
                    {
                        idQueue = fileBlocks[endIdx];
                        endIdx--;
                    }
                    allBlocks.Add(idQueue);
                    biSum += index * idQueue;
                    index++;
                }
                idx++;
            }

            // 6309364681516
            // 6311986840296 <= too high
            // 6310675819476 <= Good!

            //Log($"{string.Join(",", allBlocks)}");

            LogAnswer(1, $"{biSum}");
        }

        public class FakeFile
        {
            public long Id { get; set; }
            public long Position { get; set; }
            public int Size { get; set; }
        }

        public override void Assignment2()
        {
            // Disk space compacting whole files

            // Only a single line
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            string line = lines[0].Trim();

            int[] map = new int[line.Length];
            int maxPos = 0;
            for (int i = 0; i < line.Length; i++)
            {
                map[i] = int.Parse(line.Substring(i, 1));
                maxPos += map[i];
            }

            // Get all files
            long[] output = new long[maxPos];
            List<FakeFile> files = new List<FakeFile>();
            long id = 0;
            long pos = 0;
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i]; j++)
                {
                    if (i % 2 == 0)
                    {
                        output[pos + j] = id;
                    }
                    else
                    {
                        output[pos + j] = -1;
                    }
                }

                if (i % 2 == 0)
                {
                    files.Add(new FakeFile
                    {
                        Id = id,
                        Position = pos,
                        Size = map[i]
                    });
                    id++;
                }

                pos += map[i];
            }
            //Log($"{string.Join(",", output)}");

            files.Reverse();

            foreach(FakeFile f in files)
            {
                pos = 0;
                for (int i = 0; i < output.Length && i < f.Position; i++)
                {
                    // Count the free space
                    if (output[i] == -1)
                    {
                        int count = 1;
                        for (int j = i + 1; j < output.Length; j++, count++)
                        {
                            if (output[j] != -1)
                                break;
                        }
                        if (count >= f.Size)
                        {
                            // Yes, fits                            
                            for (int j = 0; j < f.Size; j++)
                            {
                                output[f.Position + j] = -1; // Erase file
                                output[i + j] = f.Id; // Place file
                            }
                            f.Position = i;
                            break;
                        }
                    }
                }
            }
            //Log($"{string.Join(",", output)}");

            BigInteger biSum = new BigInteger(0);            
            foreach (FakeFile f in files)
            {
                for (int i = 0; i < f.Size; i++)
                    biSum += (f.Position + i) * f.Id;
            }

            // Answer = 6335972980679
            LogAnswer(2, $"{biSum}");
        }
    }
}
