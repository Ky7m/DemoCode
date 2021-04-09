using Cake.Frosting;

namespace Build.Tasks
{
    [IsDependentOn(typeof(Clean))]
    [IsDependentOn(typeof(Publish))]
    [IsDependentOn(typeof(ZipFiles))]
    public sealed class Default : FrostingTask
    {
    }
}