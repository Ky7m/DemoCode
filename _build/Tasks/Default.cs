using Cake.Frosting;

namespace Build.Tasks
{
    [Dependency(typeof(Clean))]
    [Dependency(typeof(Build))]
    public sealed class Default : FrostingTask<Context>
    {
    }
}