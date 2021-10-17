using BenchmarkDotNet.Attributes;

namespace MakingDotNETApplicationsFaster.Runners
{

    [Config(typeof(CoreConfig))]
    public class DelegateConstructionRunner
    {
        private const int Iterations = 10000;

        private delegate int MathOperation(int x, int y);

        private int Add(int x, int y) => x + y;

        private static int DoOperation(MathOperation operation, int x, int y) => operation(x, y);

        private readonly MathOperation _operation;

        public DelegateConstructionRunner()
        {
            _operation = Add;
        }


        [Benchmark]
        public void DoOperationWithDelegate()
        {
            for (int i = 0; i < Iterations; i++)
            {
                DoOperation(Add, 1, 2);
            }
        }

        [Benchmark]
        public void DoOperationWithMethod()
        {            
            for (int i = 0; i < Iterations; i++)
            {
                DoOperation(_operation, 1, 2);
            }
        }

        [Benchmark]
        public void DoOperationWithLambdaDelegate()
        {           
            for (int i = 0; i < Iterations; i++)
            {
                DoOperation((x, y) => Add(x, y), 1, 2);
            }
        }

        [Benchmark]
        public void DoOperationWithLambda()
        {
            for (int i = 0; i < Iterations; i++)
            {
                DoOperation((x, y) => x + y, 1, 2);
            }
        }
    }
}
