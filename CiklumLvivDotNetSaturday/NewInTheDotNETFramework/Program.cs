
namespace NewInTheDotNETFramework
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AsyncVsThreadLocal.AsyncMethodA().Wait();
        }
    }
}
