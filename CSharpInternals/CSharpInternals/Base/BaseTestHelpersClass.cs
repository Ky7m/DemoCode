using Xunit.Abstractions;

namespace CSharpInternals.Base
{
    public abstract class BaseTestHelpersClass
    {
        protected readonly ITestOutputHelper Output;

        protected BaseTestHelpersClass(ITestOutputHelper output)
        {
            Output = output;
        }
    }
}