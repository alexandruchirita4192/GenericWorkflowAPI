using BenchmarkDotNet.Running;

namespace GenericWorkflowAPI.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run(typeof(BenchmarkMappings).Assembly);
        }
    }
}