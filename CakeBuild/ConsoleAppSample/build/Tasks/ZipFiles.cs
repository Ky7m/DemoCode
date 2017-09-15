using Cake.Common.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    [Dependency(typeof(Publish))]
    public sealed class ZipFiles : FrostingTask<Context>
    {
        public override void Run(Context context)
        {
            context.Zip(context.OutputPath, context.PackageFullName);
        }

        public override bool ShouldRun(Context context) => context.IsContinuousIntegrationBuild;
    }
}