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
        public FilePathCollection AllSolutions { get; set; }

        public Context(ICakeContext context)
            : base(context)
        {
        }
    }
}