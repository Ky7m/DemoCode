using Cake.Common;
using Cake.Common.IO;
using Cake.Frosting;
using JetBrains.Annotations;

namespace Build
{
    [UsedImplicitly]
    public sealed class Lifetime : FrostingLifetime<Context>
    {
        public override void Setup(Context context)
        {
            // arguments
            context.Target = context.Argument("target", "Default");
            context.Configuration = context.Argument("configuration", "Release");
            context.AllSolutions = context.GetFiles("./**/*.sln")
                                   - context.MakeAbsolute(context.File("./#build/Build.sln"));
        }
    }
}