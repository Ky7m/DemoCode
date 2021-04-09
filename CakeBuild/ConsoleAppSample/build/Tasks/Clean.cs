using Cake.Common.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    public sealed class Clean : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context) =>
            context.CleanDirectories(context.GetDirectories(context.ProjectPath + "/obj") +
                                     context.GetDirectories(context.ProjectPath + "/bin") +
                                     context.OutputPath +
                                     context.PackagePath);
    }
}