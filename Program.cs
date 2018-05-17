using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Linq;

namespace CreateRandomStringBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }

    [MemoryDiagnoser]
    public class Benchmarks
    {
        private static Random random = new Random();

        [Params(1, 10, 100, 1000)]
        public int Length { get; set; }

        [Benchmark(Baseline = true)]
        public string RandomStringSpan()
        {
            return string.Create(Length, random, (Span<char> chars, Random r) =>
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i] = (char)(r.Next(0, 10) + '0');
                }
            });
        }
        [Benchmark]
        public string RandomStringLoop()
        {
            var stringChars = new char[Length];
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = (char)(random.Next(0, 10) + '0');
            }

            return new string(stringChars);
        }

        [Benchmark]
        public string RandomStringEnumerable()
        {
            return new string(Enumerable.Repeat("", Length)
              .Select(s => (char)(random.Next(0, 10) + '0')).ToArray());
        }

    }
}
