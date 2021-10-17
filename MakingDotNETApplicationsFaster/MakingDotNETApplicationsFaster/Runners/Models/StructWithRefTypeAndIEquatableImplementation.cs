using System;

namespace MakingDotNETApplicationsFaster.Runners.Models
{
    public struct StructWithRefTypeAndEquatableImplementation : IEquatable<StructWithRefTypeAndEquatableImplementation>
    {
        public int Age { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }

        public bool Equals(StructWithRefTypeAndEquatableImplementation other)
        {
            return Age == other.Age && Height == other.Height && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StructWithRefTypeAndEquatableImplementation))
            {
                return false;
            }

            var other = (StructWithRefTypeAndEquatableImplementation)obj;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //== != operators can be ommited for this example
    }
}