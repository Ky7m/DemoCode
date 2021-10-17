using BenchmarkDotNet.Attributes;
using ReflectionInterface;
using System;
using System.Linq;
using System.Reflection;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class ReflectionMethodInvokeVsInterfaceRunner
    {
        private readonly Type _extensionType;
        private readonly Object _extensionObject;
        private const int Iterations = 10000;

        private readonly MethodInfo _executeMethodWithParameters;
        private readonly MethodInfo _executeMethodEmpty;
        private readonly IExtension _extensionViaInterface;

        public ReflectionMethodInvokeVsInterfaceRunner()
        {            
            var assembly = Assembly.Load("ReflectionExtension");

            var types = assembly.GetTypes();
            _extensionType = types.FirstOrDefault(x => x.GetInterface("IExtension") != null);

            if (!(_extensionType is null))
            {
                _extensionObject = Activator.CreateInstance(_extensionType) ?? throw new Exception($"Could not create instance of type {_extensionType}");
            }

            _executeMethodWithParameters = _extensionType.GetMethod("ExecuteParameters");
            _executeMethodEmpty = _extensionType.GetMethod("ExecuteEmpty");

            _extensionViaInterface = _extensionObject as IExtension;
        }

        [Benchmark]
        public int ReflectionMethodWithParametersInvoke()
        {                                         
            int actualResult = 0;
            for (int i = 0; i < Iterations; i++)
            {
                object result = _executeMethodWithParameters.Invoke(_extensionObject, new object[] { 1, 2 });
                actualResult = (int)result;
            }

            return actualResult;
        }

        [Benchmark]
        public int InterfaceMethodWithParametersExecute()
        {            
            int actualResult = 0;
            for (int i = 0; i < Iterations; i++)
            {
                actualResult = _extensionViaInterface.ExecuteParameters(1, 2);
            }

            return actualResult;
        }

        [Benchmark]
        public void ReflectionEmptyMethodInvoke()
        {            
            for (int i = 0; i < Iterations; i++)
            {
                _executeMethodEmpty.Invoke(_extensionObject, null);
            }         
        }

        [Benchmark]
        public void InterfaceEmptyMethodExecute()
        {            
            for (int i = 0; i < Iterations; i++)
            {
                _extensionViaInterface.ExecuteEmpty();
            }
        }
    }
}
