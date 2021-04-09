using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Publish;
using Cake.Frosting;

namespace Build.Tasks
{
    public sealed class Publish : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            context.DotNetCorePublish(
                context.ProjectPath,
                new DotNetCorePublishSettings
                {
                    Configuration = context.MsBuildConfiguration,
                    OutputDirectory = context.OutputPath
                    //Runtime = "win10-x64"
                });
        }
    }
}