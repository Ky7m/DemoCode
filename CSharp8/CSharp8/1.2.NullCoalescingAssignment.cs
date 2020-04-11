using System;
using Xunit;

namespace CSharp8
{
    public class NullCoalescingAssignment
    {
        [Theory]
        [InlineData(null)]
        public void LegacyCheckForNull(string param)
        {
            Assert.Throws<ArgumentNullException>(() =>
                {
                    if (param is null)
                    {
                        throw new ArgumentNullException(nameof(param), "NRE");
                        // consistency is always important :)
                        // throw new ArgumentException("NRE", nameof(param));
                        // throw null;
                    }
                }
            );
        }
        
        [Theory]
        [InlineData(null)]
        public void ModernCheckForNull(string param)
        {
            Assert.Throws<ArgumentNullException>(() =>
                {
                    _ = param ?? throw new ArgumentNullException(nameof(param));
                }
            );
        }
        
        [Theory]
        [InlineData(null)]
        public void AssignValueIfVariableIsNull(string param)
        {
            // param = param ?? string.Empty;
            // if (param is null) param = string.Empty;
            param ??= string.Empty;
            Assert.NotNull(param);
            
            string? defaultParam = default;
            param ??= defaultParam ??= string.Empty;
        }
    }
}