using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;
using JetBrains.Annotations;

namespace Build
{
    [UsedImplicitly]
    public class Context : FrostingContext
    {
        public string Target { get; set; }
        public string Configuration { get; set; }
        public string BuildNumber { get; set; }
        public string ProjectPath { get; set; }
        public string OutputPath { get; set; }
        public string PackagePath { get; set; }
        public string PackageFullName { get; set; }
        public bool IsContinuousIntegrationBuild { get; set; }

        public Context(ICakeContext context) 
            : base(context)
        {
        }
    }
}