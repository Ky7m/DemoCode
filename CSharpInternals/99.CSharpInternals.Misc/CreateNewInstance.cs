using System;
using System.Runtime.Serialization;
using CSharpInternals.Utils;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.Misc
{
    public class CreateNewInstance : BaseTestHelpersClass
    {
        public CreateNewInstance(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void CreateNewInstanceWithoutCtorCall()
        {
            Assert.Throws<ApplicationException>(() => new ClassWithExceptionInCtor());
            
            var instance = FormatterServices.GetUninitializedObject(typeof(ClassWithExceptionInCtor)) as ClassWithExceptionInCtor;
            Assert.NotNull(instance);
        }

        private class ClassWithExceptionInCtor
        {
            public ClassWithExceptionInCtor()
            {
                throw new ApplicationException();
            }
        }
    }
}