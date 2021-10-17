using ReflectionInterface;

namespace ReflectionExtension
{
    public class Extension : IExtension
    {      
        public void ExecuteEmpty()
        {
            
        }

        public int ExecuteParameters(int arg1, int arg2)
        {
            return arg1 + arg2;
        }
    }
}
