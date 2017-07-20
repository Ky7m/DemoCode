using System.Threading.Tasks;

namespace CSharp7
{
    public sealed class AsyncMainMethods
    {
        public static async Task<int> Main(string[] args)
        {
            await GeneralizedAsyncReturnTypes.RunExampleAsync();
            return 0;
        }
    }
}
