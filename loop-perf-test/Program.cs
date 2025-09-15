
using System.Diagnostics;

namespace loop_perf_test;

internal class Program
{
    const long PASSES = 100;
    const long NUMBER_OF_LOOPS = 10;

    static async Task Main(string[] args)
    {
        var scenarios = CreateScenarios();

        for (long i = 0; i < PASSES; i++)
        {
            Console.WriteLine($"\nPASS {i} of {PASSES}");
            foreach (var scenario in scenarios)
            {
                Console.WriteLine($"Executing scenario: {scenario.Description}");
                await Task.Run(() => scenario.Activity(scenario));
                scenario.AccumulatedTime += scenario.Microseconds;
            }
        }

        Console.WriteLine("\nAVERAGE TIMINGS:");
        foreach (var scenario in scenarios)
        {
            scenario.Microseconds = scenario.AccumulatedTime / (double)PASSES;
            Console.WriteLine($"{scenario.Description}: {scenario.Microseconds:F0} µs");
        }
    }

    static List<Scenario> CreateScenarios()
    {
        return new List<Scenario>()
        {
            new("Thread.Sleep(0)")
            {
                Activity = async (Scenario scenario) =>
                {
                    long countdown = NUMBER_OF_LOOPS;
                    Stopwatch clock = new();
                    clock.Start();
                    while(countdown-- > 0)
                    {
                        Thread.Sleep(0);
                    }
                    clock.Stop();
                    scenario.Microseconds = clock.Elapsed.Microseconds;
                }
            },

            new("spinWait.SpinOnce()")
            {
                Activity = async (Scenario scenario) =>
                {
                    long countdown = NUMBER_OF_LOOPS;
                    Stopwatch clock = new();
                    clock.Start();
                    var spinWait = new SpinWait();
                    while(countdown-- > 0)
                    {
                        try
                        {
                            spinWait.SpinOnce();
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            countdown = 0;
                        }
                    }
                    clock.Stop();
                    scenario.Microseconds = clock.Elapsed.Microseconds;
                }
            },

            new("Thread.Sleep(1)")
            {
                Activity = async (Scenario scenario) =>
                {
                    long countdown = NUMBER_OF_LOOPS;
                    Stopwatch clock = new();
                    clock.Start();
                    while(countdown-- > 0)
                    {
                        Thread.Sleep(1);
                    }
                    clock.Stop();
                    scenario.Microseconds = clock.Elapsed.Microseconds;
                }
            },

            new("Thread.Yield()")
            {
                Activity = async (Scenario scenario) =>
                {
                    long countdown = NUMBER_OF_LOOPS;
                    Stopwatch clock = new();
                    clock.Start();
                    while(countdown-- > 0)
                    {
                        Thread.Yield();
                    }
                    clock.Stop();
                    scenario.Microseconds = clock.Elapsed.Microseconds;
                }
            },

            new("await Task.Delay(0)")
            {
                Activity = async (Scenario scenario) =>
                {
                    long countdown = NUMBER_OF_LOOPS;
                    Stopwatch clock = new();
                    clock.Start();
                    while(countdown-- > 0)
                    {
                        await Task.Delay(0);
                    }
                    clock.Stop();
                    scenario.Microseconds = clock.Elapsed.Microseconds;
                }
            },

            new("// do nothing")
            {
                Activity = async (Scenario scenario) =>
                {
                    long countdown = NUMBER_OF_LOOPS;
                    Stopwatch clock = new();
                    clock.Start();
                    while(countdown-- > 0)
                    {
                        // do nothing
                    }
                    clock.Stop();
                    scenario.Microseconds = clock.Elapsed.Microseconds;
                }
            },

            new("Thread.SpinWait(1)")
            {
                Activity = async (Scenario scenario) =>
                {
                    long countdown = NUMBER_OF_LOOPS;
                    Stopwatch clock = new();
                    clock.Start();
                    while(countdown-- > 0)
                    {
                        Thread.SpinWait(1);
                    }
                    clock.Stop();
                    scenario.Microseconds = clock.Elapsed.Microseconds;
                }
            },

            new("await Task.Yield()")
            {
                Activity = async (Scenario scenario) =>
                {
                    long countdown = NUMBER_OF_LOOPS;
                    Stopwatch clock = new();
                    clock.Start();
                    while(countdown-- > 0)
                    {
                        await Task.Yield();
                    }
                    clock.Stop();
                    scenario.Microseconds = clock.Elapsed.Microseconds;
                }
            },

        };
    }
}
