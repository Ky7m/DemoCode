using Cake.Frosting;

namespace Build.Tasks
{
    [Dependency(typeof(Clean))]
    [Dependency(typeof(Publish))]
    [Dependency(typeof(ZipFiles))]
    public sealed class Default : FrostingTask<Context>
    {
    }
}