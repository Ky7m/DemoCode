using System;
using Xunit;

namespace CSharp8
{
    public class Intro
    {
        class MyAttribute : Attribute { }
        public Version CSharp7 { [My] get; } = new Version("7.3");
        
        
        //public Version CSharp7 { get; [Obsolete] set; } = new Version("7.3");
        public Version CSharp8
        {
            get; 
            //[Obsolete] set;
        } = new Version("8.0");
        //public Version CSharp8 { get; [Conditional("DEBUG")] set; } = new Version("8.x"); // not allowed
    
        
        [Fact]
        public void ObsoleteOnPropertyAccessor()
        {
            Assert.NotSame(CSharp8, CSharp7);
        }
    }
}