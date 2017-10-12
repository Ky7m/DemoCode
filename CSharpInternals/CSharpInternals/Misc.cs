using System;
using System.Runtime.Serialization;
using CSharpInternals.Base;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals
{
    public class Misc : BaseTestHelpersClass
    {
        public Misc(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void CreateNewInstanceWithoutCtorCall()
        {
            Assert.Throws<NotImplementedException>(() => new ClassWithExceptionInCtor());
            
            var instance = FormatterServices.GetUninitializedObject(typeof(ClassWithExceptionInCtor)) as ClassWithExceptionInCtor;
            Assert.NotNull(instance);
        }

        private class ClassWithExceptionInCtor
        {
            public ClassWithExceptionInCtor()
            {
                throw new NotImplementedException();
            }
        }
    }
}