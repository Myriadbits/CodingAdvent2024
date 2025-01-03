﻿using AdventOfCodeHelpers;
using System.Diagnostics;
using System.Security.Cryptography;

namespace CodingAdvent2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            bool doTest = false;
            if (args.Length > 0)
            {
                doTest = (args[0].ToLower() == "--test");
            }

            // Find all tests
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
               .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(DayBase))).ToList();
            List<DayBase> allDays = new List<DayBase>();
            foreach (var t in types) 
            {
                DayBase? bc = (DayBase?)Activator.CreateInstance(t);
                if (bc != null)
                    allDays.Add(bc);
            }
            // Sort them by increasing number
            allDays.Sort(delegate (DayBase t1, DayBase t2)
            {
                return t1.DayNumber.CompareTo(t2.DayNumber);
            });


            // Execute all tests
            //foreach (DayBase d in allDays)
                //d.ExecuteNoLog(doTest);


            DayBase day = allDays[19];

            //DayBase day = allDays.Last();
            day.Execute(doTest); 

            Console.WriteLine();
            Console.WriteLine($"Executed {allDays.Count} test in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}