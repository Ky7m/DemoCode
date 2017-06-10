using System;
using System.Threading.Tasks;

namespace CSharpSixFeatures
{
    public static class Helpers
    {
        public static string A()
        {
            return string.Empty;
        }

        public static Task<string> B(this string text, int value)
        {
            return Task.FromResult(value.ToString());
        }

        public static string GetFromCache(string key)
        {
            throw new NotImplementedException();
        }

        public static T FromJson<T>(this object value)
        {
            throw new NotImplementedException();
        }
    }
}