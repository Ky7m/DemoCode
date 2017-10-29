using Cake.Frosting;

namespace Build.Tasks
{
    [Dependency(typeof(Clean))]
    [Dependency(typeof(Restore))]
    public sealed class Default : FrostingTask<Context>
    {
    }
}