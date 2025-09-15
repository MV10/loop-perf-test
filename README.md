# loop-perf-test

Tests the performance impact of the various ways .NET offers to "do nothing" in a background thread in a tight loop (for example, waiting for a flag to be set, such as an indicator that some other thread has data to be processed). Because some of these approaches depend on other system activity, the timings shown here aren't a guarantee, but on a system as quiet as I can make it, the relative performance may still be useful.

Also, in some cases (like `spinWait.SpinOnce`) the description is _extremely_ over-simplified.

| Method | Description | Avg Loop |
|---|---|---|
| `Thread.Sleep(0)`     | cede control to any thread of equal priority   | 3 탎 |
| `spinWait.SpinOnce()` | periodically yields (default is 10 iterations) | 2 탎 |
| `Thread.Sleep(1)`     | cede control to any thread of OS choice        | 490 탎 |
| `Thread.Yield()`      | cede control to any thread on the same core    | 2 탎 |
| `await Task.Delay(0)` | creates and waits on a system timer            | 1 탎 |
| `// do nothing`       | burn down a CPU core                           | 0 탎 |
| `Thread.SpinWait(1)`  | duration-limited Yield                         | 0 탎 |
| `await Task.Yield()`  | suspend task indefinitely (scheduler control)  | 208 탎 |

