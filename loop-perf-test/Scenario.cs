
namespace loop_perf_test;

public class Scenario
{
    // for display purposes
    public string Description;

    // async Task Activity(Sscenario scenario)
    public Func<Scenario, Task> Activity;

    // passed to the function's microseconds arg; this is the output
    public double Microseconds;

    // for averaging Microseconds over multiple passes
    public double AccumulatedTime;

    public Scenario(string description)
    {
        Description = description;
    }
}
