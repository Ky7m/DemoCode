namespace CSharp8
{
    public class ReadonlyMembers
    {
        public struct MyVector
        {
            public float x;
            public float y;

            public float LengthSquared => (x * x) + (y * y);

            public float LengthSquaredReadonly
            {
                readonly get => (x * x) + (y * y);
                private set {}
            }
        }

        public float Sum(in MyVector value)
        {
            return value.LengthSquared + value.LengthSquared;
        }

        public float SumReadonly(in MyVector value)
        {
            return value.LengthSquaredReadonly + value.LengthSquaredReadonly;
        }
    }
}