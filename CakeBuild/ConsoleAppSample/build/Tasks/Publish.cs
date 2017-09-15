using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Publish;
using Cake.Frosting;

namespace Build.Tasks
{
    public sealed class Publish : FrostingTask<Context>
    {
        public override void Run(Context context)
        {
            context.DotNetCorePublish(
                context.ProjectPath,
                new DotNetCorePublishSettings()
                {
                    Configuration = context.Configuration,
                    OutputDirectory = context.OutputPath
                    //Runtime = "win10-x64"
                });
        }
    }
}