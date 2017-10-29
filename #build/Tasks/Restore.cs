using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Frosting;

namespace Build.Tasks
{
    public sealed class Restore : FrostingTask<Context>
    {
        public override void Run(Context context)
        {
            foreach (var solution in context.AllSolutions)
            {
                context.NuGetRestore(solution, new NuGetRestoreSettings
                {
                    ToolPath = "./#build/tools/NuGet.exe"
                });
            }
        }
    }
}