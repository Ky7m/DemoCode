using System;

namespace LiskovSubstitutionPrinciple
{
    static class LiskovSubstitutionPrincipleProgram
    {
        static void Main(string[] args)
        {
            try
            {
                MyClass myClass = new MyDerivedClass();
                myClass.Foo();
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("catch (Exception)");
            }
        }
    }


    class MyClass
    {
        public virtual void Foo()
        {
            throw new Exception();
        }
    }

    class MyDerivedClass : MyClass
    {
        public override void Foo()
        {
            throw new ArgumentNullException();
        }
    }
}


