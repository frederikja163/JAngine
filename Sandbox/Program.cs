using System.Diagnostics;
using JAngine.Rendering;
using JAngine.Rendering.OpenGL;

try
{
    using Window window = new Window("Test", 100, 100);

    // Warmup
    Console.WriteLine("Warming up.");
    BenchmarkFancySubdata(1000);
    BenchmarkSimpleSubdata(1000);
    Console.WriteLine("Actual tests");
    BenchmarkFancySubdata(100_000_000);
    BenchmarkSimpleSubdata(100_000_000);
    
    Window.Run();
    void BenchmarkFancySubdata(int n)
    {
        var watch = Stopwatch.StartNew();
        var buffer = new FixedBuffer<float>(window, Gl.BufferStorageMask.DynamicStorageBit, n);
        float[] data = new float[1000];
        for (int i = 0; i < n; i++)
        {
            data[i % 1000] = Random.Shared.Next();
            if (i % 1000 == 0)
            {
                buffer.SetSubData(i, data);
            }
        }
        Console.WriteLine($"Fancy subdata: {watch.ElapsedMilliseconds} ms for {n} floats");
    }
    
    void BenchmarkSimpleSubdata(int n)
    {
        var watch = Stopwatch.StartNew();
        var buffer = new FlexibleBuffer<float>(window, Gl.BufferStorageMask.DynamicStorageBit, n);
        float[] data = new float[100];
        for (int i = 0; i < n; i++)
        {
            data[i % 100] = Random.Shared.Next();
            if (i % 100 == 0)
            {
                buffer.SetSubData(i, data);
            }
        }
        Console.WriteLine($"Simple subdata: {watch.ElapsedMilliseconds} ms for {n} floats");
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine(e.StackTrace);
}

Console.ReadKey();