// https://github.com/dotnet/roslyn/issues/18554
namespace CSharp7
{
    /*
    class DeconstructionCompilationError
    {
        public DeconstructionCompilationError()
        {
            var tuple = (1, 2);
            (this[0], this[1]) = tuple;
        }
        int this[int index]
        {
            set => System.Console.WriteLine($"{index}={value}");
            //get => throw new Exception("Shouldn't be called");
        }
    }
    /**/
}
