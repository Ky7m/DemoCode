using Cake.Common.IO;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    public sealed class Clean : FrostingTask<Context>
    {
        public override void Run(Context context)
        {
            var toDelete = context.GetDirectories("./**/obj")
                           - context.MakeAbsolute(DirectoryPath.FromString("./_build/obj"))
                           + context.GetDirectories("./**/bin")
                           - context.MakeAbsolute(DirectoryPath.FromString("./_build/bin"))
                           + context.GetDirectories("./**/packages");
            
            context.CleanDirectories(toDelete);
        }
    }
}