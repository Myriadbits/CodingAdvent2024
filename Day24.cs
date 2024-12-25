using AdventOfCodeHelpers;
using System.Text.RegularExpressions;

namespace CodingAdvent2024
{
    class Operation
    {
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Output { get; set; }
        public string Operand { get; set; }

        public Operation(Match mA)
        {
            Input1 = mA.Groups[1].Value;
            Input2 = mA.Groups[3].Value;
            Operand = mA.Groups[2].Value;
            Output = mA.Groups[4].Value;
        }

        public bool Execute(Dictionary<string, bool> values)
        {
            if (values.ContainsKey(Input1) && values.ContainsKey(Input2))
            {
                switch(Operand.ToLower())
                {
                    case "or":
                        values[Output] = values[Input1] | values[Input2];
                        break;
                    case "xor":
                        values[Output] = values[Input1] ^ values[Input2];
                        break;
                    case "and":
                        values[Output] = values[Input1] & values[Input2];
                        break;
                }
                return true;
            }
            return false;
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

        private long getZValue()
        {
            long value = 0;
            var ordered = m_values.OrderByDescending(a => a.Key);
            foreach (var order in ordered)
            {
                if (order.Key.StartsWith('z'))
                {
                    value <<= 1;
                    value += (order.Value) ? 1 : 0;
                }
            }
            return value;
        }

        public override void Assignment1()
        {
            //  Crossed Wires
            long sum = 0;

            Initialize();

            while (m_operations.Count > 0)
            {
                Log($"Operations: {m_operations.Count}");
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

        public override void Assignment2()
        {
            //  Crossed Wires, determine the crosses
            long sum = 0;

            Initialize();

            // Fill X and Y with values
            // X & Y both 44 bits
            long x = 0x1AAAAAAAAAAA;
            long y = 0x1AAAAAAAAAAA;
            //long x = 0x1FFFFFFFFFFF;
            //long y = 0x1FFFFFFFFFFF;
            // Check if result matches

            m_values = new Dictionary<string, bool>();
            long mask = 0x01;
            for(int n = 0; n < 45; n++)
            {
                m_values[$"x{n:00}"] = ((x & mask) == mask);
                m_values[$"y{n:00}"] = ((y & mask) == mask);
                mask <<= 1;
            }

            while (m_operations.Count > 0)
            {
                Log($"Operations: {m_operations.Count}");
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

            long z = getZValue();
            long data = x + y - z;
            Log($"x + y - z = {data:X}");
            if (x + y == z)
            {
                Log("Yes, found!");
            }

            LogAnswer(2, $"{sum}");
        }

    }
}
