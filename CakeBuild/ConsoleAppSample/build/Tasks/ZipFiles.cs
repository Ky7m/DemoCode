using Cake.Common.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    [IsDependentOn(typeof(Publish))]
    public sealed class ZipFiles : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            context.Zip(context.OutputPath, context.PackageFullName);
        }

        public override bool ShouldRun(BuildContext context) => context.IsContinuousIntegrationBuild;
    }
}