namespace Blog.Performance.Tests
{
    using BenchmarkDotNet.Running;

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<InliningBenchmark>();
        }
    }

}
