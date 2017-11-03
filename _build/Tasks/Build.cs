using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.MSBuild;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    public sealed class Build : FrostingTask<Context>
    {
        public override void Run(Context context)
        {
            var buildSettings = new MSBuildSettings()
                .SetConfiguration(context.Configuration)
                .WithTarget("Restore")
                //.WithTarget("Rebuild")
                .SetVerbosity(Verbosity.Minimal)
                .UseToolVersion(MSBuildToolVersion.VS2017)
                .SetMSBuildPlatform(MSBuildPlatform.Automatic)
                .SetPlatformTarget(PlatformTarget.MSIL) // Any CPU
                .SetMaxCpuCount(0) // Use as many MSBuild processes as available CPUs
                .SetNodeReuse(true);

            foreach (var solution in context.AllSolutions)
            {
                context.MSBuild(solution, buildSettings);
            }
        }
    }
}