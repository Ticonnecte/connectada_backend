using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using Xunit.Abstractions;
using Xunit.Runners;

namespace CidConnectada.Tests
{
    internal class Program
    {
        // We use consoleLock because messages can arrive in parallel, so we want to make sure we get
        // consistent console output.
        private static readonly object consoleLock = new object();

        // Use an event to know when we're done
        private static readonly ManualResetEvent finished = new ManualResetEvent(false);

        // Start out assuming success; we'll set this to 1 if we get a failed test
        private static int result;

        private static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("usage: TestRunner <assembly> [typeName [typeName...]]");
                return 2;
            }

            string testAssembly = args[0];
            var typeNames = new List<string>();
            for (int i = 1; i < args.Length; i++)
                typeNames.Add(args[i]);

            using (var runner = new XunitFrontController(AppDomainSupport.IfAvailable, testAssembly))
            {
                var discoverySink = new TestDiscoverySink();
                runner.Find(false, discoverySink, TestFrameworkOptions.ForDiscovery());
                discoverySink.Finished.WaitOne();

                lock (consoleLock)
                {
                    Console.WriteLine($"Running {discoverySink.TestCases.Count} tests...");
                }

                var executionSink = new TestExecutionSink(OnTestFailed, OnTestSkipped);
                runner.RunTests(discoverySink.TestCases, executionSink, TestFrameworkOptions.ForExecution());

                executionSink.Finished.WaitOne();
            }

            finished.Set();
            return result;
        }

        private static void OnDiscoveryComplete(DiscoveryCompleteInfo info)
        {
            lock (consoleLock)
            {
                Console.WriteLine($"Running {info.TestCasesToRun} of {info.TestCasesDiscovered} tests...");
            }
        }

        private static void OnExecutionComplete(ExecutionCompleteInfo info)
        {
            lock (consoleLock)
            {
                Console.WriteLine($"Finished: {info.TotalTests} tests in {Math.Round(info.ExecutionTime, 3)}s ({info.TestsFailed} failed, {info.TestsSkipped} skipped)");
            }

            finished.Set();
        }

        private static void OnTestFailed(ITestFailed info)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[FAIL] {info.Test.DisplayName}: {info.Messages[0]}");
                if (!String.IsNullOrEmpty(info.StackTraces?[0]))
                    Console.WriteLine(info.StackTraces[0]);
                Console.ResetColor();
            }

            result = 1;
        }

        private static void OnTestSkipped(ITestSkipped info)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[SKIP] {info.Test.DisplayName}: {info.Reason}");
                Console.ResetColor();
            }
        }
    }
}