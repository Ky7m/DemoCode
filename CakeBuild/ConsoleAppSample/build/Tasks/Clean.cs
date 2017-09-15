using Cake.Common.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    public sealed class Clean : FrostingTask<Context>
    {
        public override void Run(Context context) =>
            context.CleanDirectories(context.GetDirectories(context.ProjectPath + "/obj") +
                                     context.GetDirectories(context.ProjectPath + "/bin") +
                                     context.OutputPath);
    }
}