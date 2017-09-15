using System.IO;
using Cake.Common;
using Cake.Common.Build;
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
            context.BuildNumber = context.Argument("buildNumber", "255.255.255.255");
            
            // global variables
            context.OutputPath = "./output";
            context.PackagePath = "./publish";
            context.ProjectPath = "./src/ConsoleAppSample";
            context.PackageFullName = $"{context.PackagePath}/ConsoleAppSample.{context.BuildNumber}.zip";
            
            context.IsContinuousIntegrationBuild = !context.BuildSystem().IsLocalBuild;
        }
    }
}