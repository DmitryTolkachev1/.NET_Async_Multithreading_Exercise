/*
╔══════════════════════════════════════════════════════════════╗
║               Thread vs Task – Key Differences               ║
╠════════════════════════════╦═════════════════════════════════╣
║ Thread                     ║ Task                            ║
╠════════════════════════════╬═════════════════════════════════╣
║ Low-level execution unit   ║ High-level abstraction          ║
║ Managed directly by OS     ║ Managed by .NET Task Scheduler  ║
║ Manual start and control   ║ Supports async/await            ║
║ Does not return results    ║ Can return results (Task<T>)    ║
║ Resource-intensive         ║ Lightweight, reuses thread pool ║
║ No cancellation support    ║ Supports cancellation tokens    ║
║ Hard to scale              ║ Easily scalable                 ║
╚════════════════════════════╩═════════════════════════════════╝

Thread is a fundamental, low-level component that represents an OS-managed unit of execution.
You create and control it manually, and it always results in a new thread being allocated by the system.
While it's powerful, it's also heavy and not well-suited for large-scale concurrent operations.

Task, on the other hand, is a high-level .NET construct for representing asynchronous operations.
It is typically backed by the thread pool, making it more efficient and easier to scale.
Tasks integrate seamlessly with async/await, support cancellation, continuation, and can return values.
They are preferred for most asynchronous programming in modern .NET applications.
*/
using System.Diagnostics;

Console.WriteLine("Main thread started. Thread ID: " + Thread.CurrentThread.ManagedThreadId);
Console.WriteLine(new string('-', 60));

RunThreadExample();
Console.WriteLine(new string('-', 60));

RunTaskExample();
Console.WriteLine(new string('-', 60));

Console.WriteLine("Demo completed.");

static void RunThreadExample()
{
    Console.WriteLine("THREAD EXAMPLE — manually created threads");

    var sw = Stopwatch.StartNew();

    for (int i = 1; i <= 3; i++)
    {
        var thread = new Thread(() =>
        {
            Console.WriteLine($"[Thread {i}] Start - Thread ID: {Thread.CurrentThread.ManagedThreadId}, IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}");
            Thread.Sleep(500);
            Console.WriteLine($"[Thread {i}] End");
        });

        thread.Start();
        thread.Join();
    }

    sw.Stop();
    Console.WriteLine($"Total execution time (Thread): {sw.ElapsedMilliseconds}ms");
}

static void RunTaskExample()
{
    Console.WriteLine("TASK EXAMPLE — using thread pool via Task.Run");

    var sw = Stopwatch.StartNew();

    var tasks = new Task[3];

    for (int i = 0; i < tasks.Length; i++)
    {
        int taskNum = i + 1;
        tasks[i] = Task.Run(() =>
        {
            Console.WriteLine($"[Task {taskNum}] Start - Thread ID: {Thread.CurrentThread.ManagedThreadId}, IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}");
            Thread.Sleep(500);
            Console.WriteLine($"[Task {taskNum}] End");
        });
    }

    Task.WaitAll(tasks);

    sw.Stop();
    Console.WriteLine($"Total execution time (Task): {sw.ElapsedMilliseconds}ms");
}