using Xunit.Abstractions;

namespace CSharpInternals.Base
{
    public abstract class BaseTestHelpersClass
    {
        private readonly ITestOutputHelper _output;
    
        protected BaseTestHelpersClass(ITestOutputHelper output)
        {
            _output = output;
        }

        protected void WriteLine(string message)
        {
            _output.WriteLine(message);
        }
    }
}