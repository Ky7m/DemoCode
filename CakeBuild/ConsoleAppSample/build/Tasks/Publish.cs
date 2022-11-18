
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Frosting;

namespace Build.Tasks
{
    public sealed class Publish : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            context.DotNetPublish(
                context.ProjectPath,
                new DotNetPublishSettings
                {
                    Configuration = context.MsBuildConfiguration,
                    OutputDirectory = context.OutputPath
                    //Runtime = "win10-x64"
                });
        }
    }
}