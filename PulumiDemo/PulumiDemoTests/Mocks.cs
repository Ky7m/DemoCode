using System.Collections.Immutable;
using System.Threading.Tasks;
using Pulumi.Testing;

namespace PulumiDemoTests
{
    internal class Mocks : IMocks
    {
        public Task<(string id, object state)> NewResourceAsync(MockResourceArgs args)
        {
            var outputs = ImmutableDictionary.CreateBuilder<string, object>();
            
            // Forward all input parameters as resource outputs, so that we could test them.
            outputs.AddRange(args.Inputs);

            if (args.Type == "azure:storage/blob:Blob")
            {
                // Assets can't directly go through the engine.
                // We don't need them in the test, so blank out the property for now.
                outputs.Remove("source");
            }
            
            // For a Storage Account...
            if (args.Type == "azure:storage/account:Account")
            {
                // ... set its web endpoint property.
                // Normally this would be calculated by Azure, so we have to mock it. 
                outputs.Add("primaryWebEndpoint", $"https://{args.Name}.web.core.windows.net");
            }

            // Default the resource ID to `{name}_id`.
            // We could also format it as `/subscription/abc/resourceGroups/xyz/...` if that was important for tests.
            args.Id ??= $"{args.Name}_id";
            return Task.FromResult((args.Id, (object)outputs));
        }

        public Task<object> CallAsync(MockCallArgs args)
        {
            // We don't use this method in this particular test suite.
            // Default to returning whatever we got as input.
            return Task.FromResult((object)args);
        }
    }
}