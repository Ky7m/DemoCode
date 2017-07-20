using static System.Console;

namespace CSharp7
{
    public sealed class MoreExpressionBodiedMembers
    {
        public MoreExpressionBodiedMembers() => WriteLine($"ctor {nameof(MoreExpressionBodiedMembers)}");
        ~MoreExpressionBodiedMembers() => WriteLine($"finalizer {nameof(MoreExpressionBodiedMembers)}");

        private int _minutes;

        public int Hours
        {
            get => _minutes / 60;
            set => _minutes = value * 60;
        }

        public bool IsEarly => _minutes <= 15;
    }
}
