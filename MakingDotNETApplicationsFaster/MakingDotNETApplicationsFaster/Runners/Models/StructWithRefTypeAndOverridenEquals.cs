namespace MakingDotNETApplicationsFaster.Runners.Models
{
    public struct StructWithRefTypeAndOverridenEquals
    {
        public int Age { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is StructWithRefTypeAndOverridenEquals))
            {
                return false;
            }

            var other = (StructWithRefTypeAndOverridenEquals)obj;

            return Age == other.Age && Height == other.Height && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //== != operators can be ommited for this example
    }
}