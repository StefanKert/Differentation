using Differentation.Diff;
using Rota.PayrollInterface.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differentation
{
    class Program
    {
        public const int ITERATIONS = 10000;
        public const int RUNS = 5;

        static void Main(string[] args)
        {
            Console.WriteLine($"Sync/Parallel Vergleich: {ITERATIONS:N0} Iterationen");
            Execute_Version1_Sync();
            Execute_Version2_Sync();
            Execute_Version3_Sync();

            Console.ReadLine();
        }

        public static void Execute_Version1_Sync()
        {
            Console.WriteLine("Version 1");
            var result = PerformanceChecker.ExecuteWithPerformanceCheck(() => ExecuteSync(WorkerMethods.DoWork_Version1), RUNS);
            PrintResult(result);
        }

        public static void Execute_Version2_Sync()
        {
            Console.WriteLine("Version 2");
            var result = PerformanceChecker.ExecuteWithPerformanceCheck(() => ExecuteSync(WorkerMethods.DoWork_Version2), RUNS);
            PrintResult(result);
        }

        public static void Execute_Version3_Sync()
        {
            Console.WriteLine("Version 3");
            var result = PerformanceChecker.ExecuteWithPerformanceCheck(() => ExecuteSync(WorkerMethods.DoWork_Version3), RUNS);
            PrintResult(result);
        }

        private static void PrintResult(PerformanceResult result)
        {
            foreach (var duration in result.Durations)
            {
                Console.WriteLine($"| Run {duration.Key} | Zeit (ms): {duration.Value.Milliseconds}");
            }

            Console.WriteLine($"| {result.Durations.Count} Runs | {result.Durations.Average(x => x.Value.Milliseconds)} ms (/)");
        }

        public static void DoWork()
        {
            var firstObj = new PiContract();
            var secondObj = new PiContract();
            secondObj.BeginOfContract = new DateTime(2015, 1, 1);

            var differentiator = new GenericComparer<PiContract>();
            var differences = differentiator.GetDifferences(firstObj, secondObj).ToList();
        }

        public static void ExecuteParallel()
        {
            Parallel.For(0, ITERATIONS, i =>
            {
                DoWork();
            });
        }

        public static void ExecuteSync(Action doWork)
        {
            for (int i = 0; i < ITERATIONS; i++)
            {
                doWork();
            }
        }
    }


    public static class WorkerMethods
    {
        public static void DoWork_Version1()
        {
            var firstObj = new PiContract();
            var secondObj = new PiContract();
            secondObj.BeginOfContract = new DateTime(2015, 1, 1);

            var differentiator = new Diff.Version_1.GenericComparer<PiContract>();
            var differences = differentiator.GetDifferences(firstObj, secondObj).ToList();
        }

        public static void DoWork_Version2()
        {
            var firstObj = new PiContract();
            var secondObj = new PiContract();
            secondObj.BeginOfContract = new DateTime(2015, 1, 1);

            var differentiator = new Diff.Version_2.GenericComparer<PiContract>(new BasicComparisionStrategy<PiContract>());
            var differences = differentiator.GetDifferences(firstObj, secondObj).ToList();
        }

        public static void DoWork_Version3()
        {
            var firstObj = new PiContract();
            var secondObj = new PiContract();
            secondObj.BeginOfContract = new DateTime(2015, 1, 1);

            var differentiator = new Diff.Version_3.GenericComparer<PiContract>(new Diff.Version_3.BasicComparisionStrategy<PiContract>());
            var differences = differentiator.GetDifferences(firstObj, secondObj).ToList();
        }

        public static void DoWork_FinalVersion()
        {
            var firstObj = new PiContract();
            var secondObj = new PiContract();
            secondObj.BeginOfContract = new DateTime(2015, 1, 1);

            var differentiator = new GenericComparer<PiContract>();
            var differences = differentiator.GetDifferences(firstObj, secondObj).ToList();
        }
    }

    public class PerformanceChecker
    {
        public static PerformanceResult ExecuteWithPerformanceCheck(Action actionToExecute, int runs)
        {
            var result = new PerformanceResult
            {
                Runs = runs
            };
            var stopWatch = new Stopwatch();

            for(int i = 0; i < runs; i++)
            {
                stopWatch.Reset();
                stopWatch.Start();
                actionToExecute();
                stopWatch.Stop();
                result.Durations.Add(i + 1, stopWatch.Elapsed);
            }
            return result;
        }
    }

    public class PerformanceResult
    {
        public int Runs;
        public Dictionary<int, TimeSpan> Durations = new Dictionary<int, TimeSpan>();
    }
}
