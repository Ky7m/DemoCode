using Cake.Common;
using Cake.Common.Build;
using Cake.Core;
using Cake.Frosting;
using JetBrains.Annotations;

namespace Build
{
    [UsedImplicitly]
    public class BuildContext : FrostingContext
    {
        public string Target { get; set; }
        public string MsBuildConfiguration { get; set; }
        public string BuildNumber { get; set; }
        public string ProjectPath { get; set; }
        public string OutputPath { get; set; }
        public string PackagePath { get; set; }
        public string PackageFullName { get; set; }
        public bool IsContinuousIntegrationBuild { get; set; }

        public BuildContext(ICakeContext context) 
            : base(context)
        {
            // arguments
            Target = context.Argument("target", "Default");
            MsBuildConfiguration = context.Argument("configuration", "Release");
            BuildNumber = context.Argument("buildNumber", "255.255.255.255");
            
            // global variables
            OutputPath = "./output";
            PackagePath = "./publish";
            ProjectPath = "../src/ConsoleAppSample";
            PackageFullName = $"{PackagePath}/ConsoleAppSample.{BuildNumber}.zip";
            
            IsContinuousIntegrationBuild = !context.BuildSystem().IsLocalBuild;
        }
    }
}