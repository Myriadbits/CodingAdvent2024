using AdventOfCodeHelpers;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CodingAdvent2024
{
    enum EOperation
    {
        or,
        xor,
        and
    };

    class Operation
    {
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Output { get; set; }
        public EOperation Operand { get; set; }
        private bool m_fProcessed = false;

        public Operation(Match mA)
        {
            Input1 = mA.Groups[1].Value;
            Input2 = mA.Groups[3].Value;
            switch (mA.Groups[2].Value.ToLower())
            {
                case "or":
                    Operand = EOperation.or;
                    break;
                case "xor":
                    Operand = EOperation.xor;
                    break;
                case "and":
                    Operand = EOperation.and;
                    break;
            }
            Output = mA.Groups[4].Value;
        }

        public bool Execute(Dictionary<string, bool> values)
        {
            if (m_fProcessed) return true;
            if (values.ContainsKey(Input1) && values.ContainsKey(Input2))
            {
                switch(Operand)
                {
                    case EOperation.or:
                        values[Output] = values[Input1] | values[Input2];
                        break;
                    case EOperation.xor:
                        values[Output] = values[Input1] ^ values[Input2];
                        break;
                    case EOperation.and:
                        values[Output] = values[Input1] & values[Input2];
                        break;
                }
                m_fProcessed = true;
                return true;
            }
            return false;
        }

        public void Clear()
        {
            m_fProcessed = false;
        }
    }


    internal class Day24 : DayBase
    {
        Dictionary<string, bool> m_values = new Dictionary<string, bool>();
        List<Operation> m_operations = new List<Operation>();


        public Day24()
            : base(24)
        {
        }

        private void Initialize()
        {
            List<string> lines = System.IO.File.ReadLines(m_filePath).ToList();
            bool parsingInputs = true;
            for (int n = 0; n < lines.Count; n++)
            {
                string line = lines[n];
                if (string.IsNullOrEmpty(line))
                {
                    parsingInputs = false;
                    continue;
                }

                if (parsingInputs)
                {
                    string[] parts = line.Split(':');
                    m_values[parts[0]] = (parts[1].Trim() == "1");
                }
                else
                {
                    // y33 AND x33 -> bfn
                    Regex exp = new Regex(@"(\w+)\s(\w+)\s(\w+)\s->\s(\w+)");
                    Match mA = exp.Match(line);
                    if (mA.Success)
                    {
                        Operation operation = new Operation(mA);
                        m_operations.Add(operation);
                    }
                }
            }
        }

        private ulong getZValue()
        {
            ulong value = 0;
            var ordered = m_values.OrderByDescending(a => a.Key);
            foreach (var order in ordered)
            {
                if (order.Key.StartsWith('z'))
                {
                    value <<= 1;
                    value += (ulong)((order.Value) ? 1 : 0);
                }
            }
            return value;
        }

        public override void Assignment1()
        {
            //  Crossed Wires
            long sum = 0;

            Initialize();

            int step = 0;
            while (m_operations.Count > 0)
            {
                Log($"[{step}] Operations: {m_operations.Count}");
                step++;

                List<Operation> newOperations = new List<Operation>();
                foreach (Operation operation in m_operations)
                {
                    if (!operation.Execute(m_values))
                    {
                        newOperations.Add(operation);
                    }
                }
                m_operations = newOperations;
            }

            var ordered = m_values.OrderByDescending(a => a.Key);
            foreach(var order in ordered)
            {
                if (order.Key.StartsWith('z'))
                {
                    sum <<= 1;
                    sum += (order.Value) ? 1 : 0;
                }
            }
            LogAnswer(1, $"{sum}");
        }

        private void Swap(List<Operation> operations, int i, int j)
        {
            string output = operations[i].Output;
            operations[i].Output = operations[j].Output;
            operations[j].Output = output;
        }

        public override void Assignment2()
        {
            //  Crossed Wires, determine the crosses
            long sum = 0;

            Initialize();

            // Fill X and Y with values
            // X & Y both 44 bits
            //long x = 0x1AAAAAAAAAAA;
            //long y = 0x1AAAAAAAAAAA;
            //long x = 0x1FFFFFFFFFFF;
            //long y = 0x1FFFFFFFFFFF;
            // Check if result matches
            //long x = 0xFFFFFFFFFFF; // 0x2A;
            //long y = 0xFFFFFFFFFFF;
            //long y = 0xAAAAAAAAAAA; // 0x2A;
            //long x = 0x55555555555;
            //long y = 0x55555555555;

            long x = 0x01;
            long y = 0x04;

            

            long xymask = 0x08;
            for (int n = 16; n < 44; n++)
            {
                x = x | xymask;
                y = y | xymask;
                xymask <<= 1;
                Log($"Checking: x={x:X}, y={y:X}");
                bool keepswapping = true;

                Stopwatch sw = Stopwatch.StartNew();
                for (int i = 0; i < m_operations.Count && keepswapping; i++)
                {
                    for (int j = 0; j < m_operations.Count && keepswapping; j++)
                    {
                        List<Operation> operations = new List<Operation>(m_operations);
                        operations.ForEach(a => a.Clear());
                        if (i != 0 && j != 0)
                        {
                            if (i == j) continue;
                            Swap(operations, i, j);
                        }

                        //Swap(operations, 214, 188); // 28
                        //Swap(operations, 24, 23); // 28
                        //Swap(operations, 119, 62);
                        //Swap(operations, 13, 124);
                        Swap(operations, i, j);


                        //Swap(operations, 186, 171);
                        //Swap(operations, 0, 5);
                        //Swap(operations, 1, 2);

                        m_values = new Dictionary<string, bool>();
                        long mask = 0x01;
                        for (int k = 0; k < 45; k++)
                        {
                            m_values[$"x{k:00}"] = ((x & mask) == mask);
                            m_values[$"y{k:00}"] = ((y & mask) == mask);
                            mask <<= 1;
                        }

                        long loopcheck = 0;
                        int invalidOperations = 0;
                        bool success = true;
                        do
                        {
                            success = true;
                            invalidOperations = 0;
                            foreach (Operation operation in operations)
                            {
                                if (!operation.Execute(m_values))
                                {
                                    invalidOperations++;
                                    success = false;
                                }
                            }
                            loopcheck++;
                            if (loopcheck > 200)
                                break;
                        }
                        while (!success);
                        if (success)
                        {
                            Log($"[{i}]-[{j}]: SUCCESS LOOP [{invalidOperations}]");
                        }
                        else
                        {
                           //Log($"[{i}]-[{j}]: FAILED LOOP [{invalidOperations}]");
                        }
                        //long loopcheck = 0;
                        //while (operations.Count > 0)
                        //{
                        //    List<Operation> newOperations = new List<Operation>();
                        //    foreach (Operation operation in operations)
                        //    {
                        //        if (!operation.Execute(m_values))
                        //        {
                        //            newOperations.Add(operation);
                        //        }
                        //    }
                        //    operations = newOperations;
                        //    loopcheck++;
                        //    if (loopcheck > 50)
                        //        break;
                        //}

                        ulong z = getZValue();
                        ulong data = (ulong)(x + y);
                        //if (z != 0)
                        //{
                        //    Log($"[{i}]-[{j}]: x + y - z = 0x{data:X}  [0x{z:X}]");
                        //}
                        //ulong bits = System.Runtime.Intrinsics.X86.Popcnt.X64.PopCount(z);
                        if (data == z)
                        {
                            Log($"[{i}]-[{j}]: Yes, found!");
                            if (i == 0 && j == 0)
                            {
                                keepswapping = false;
                                break;
                            }
                        }
                        //if (bits > bitcount)
                        //{
                        //    Log($"[{i}]-[{j}]: x + y - z = 0x{data:X}  [0x{z:X}] => bits [{bitcount}]");
                        //    //if (x + y == z)
                        //    //{
                        //    //    Log("Yes, found!");
                        //    //}
                        //    bitcount = bits;
                        //}



                    }
                }
                Log($"Single loop in {sw.ElapsedMilliseconds} ms");
            }

            

            LogAnswer(2, $"{sum}");
        }

    }
}
