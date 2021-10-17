using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using FastMember;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class FastMemberPerformanceRunner
    {
        public string Value { get; set; }

        private FastMemberPerformanceRunner _obj;
        private dynamic _dlr;
        private PropertyInfo _prop ;

        // FastMember
        private TypeAccessor _accessor ;
        private ObjectAccessor _wrapped ;

        private Type _type ;

        [GlobalSetup]
        public void SetupData()
        {
            _obj = new FastMemberPerformanceRunner();
            _dlr = _obj;
            _prop = typeof(FastMemberPerformanceRunner).GetProperty("Value");

            // FastMember
            _accessor = FastMember.TypeAccessor.Create(typeof(FastMemberPerformanceRunner));
            _wrapped = FastMember.ObjectAccessor.Create(_obj);

            _type = typeof(FastMemberPerformanceRunner);
        }

        [Benchmark]
        public string StaticCSharp()
        {
            _obj.Value = "abc";
            return _obj.Value;
        }

        [Benchmark]
        public string DynamicCSharp()
        {
            _dlr.Value = "abc";
            return _dlr.Value;
        }

        [Benchmark]
        public string PropertyInfo()
        {
            _prop.SetValue(_obj, "abc", null);
            return (string)_prop.GetValue(_obj, null);
        }

        [Benchmark]
        public string TypeAccessor()
        {
            _accessor[_obj, "Value"] = "abc";
            return (string)_accessor[_obj, "Value"];
        }

        [Benchmark]
        public string ObjectAccessor()
        {
            _wrapped["Value"] = "abc";
            return (string)_wrapped["Value"];
        }

        [Benchmark]
        public FastMemberPerformanceRunner CSharpNew()
        {
            return new FastMemberPerformanceRunner();
        }

        [Benchmark]
        public object ActivatorCreateInstance()
        {
            return Activator.CreateInstance(_type);
        }

        [Benchmark]
        public object TypeAccessorCreateNew()
        {
            return _accessor.CreateNew();
        }
    }
}
