using Cake.Frosting;

namespace Build
{
    public static class Program
    {
        public static void Main(string[] args) => new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}
