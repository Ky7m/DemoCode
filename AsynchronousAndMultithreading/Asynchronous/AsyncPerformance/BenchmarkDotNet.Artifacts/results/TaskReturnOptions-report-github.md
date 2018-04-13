``` ini

BenchmarkDotNet=v0.10.14, OS=macOS Sierra 10.12.6 (16G1212) [Darwin 16.7.0]
Intel Core i5-5257U CPU 2.70GHz (Broadwell), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=2.1.300-preview2-008530
  [Host]     : .NET Core 2.0.6 (CoreCLR 4.6.0.0, CoreFX 4.6.26212.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.6 (CoreCLR 4.6.0.0, CoreFX 4.6.26212.01), 64bit RyuJIT


```
|            Method |      Mean |     Error |    StdDev |    Median |
|------------------ |----------:|----------:|----------:|----------:|
|    TaskFromResult | 13.828 ns | 1.1874 ns | 3.4260 ns | 12.759 ns |
| TaskCompletedTask |  2.507 ns | 0.1590 ns | 0.4432 ns |  2.383 ns |
