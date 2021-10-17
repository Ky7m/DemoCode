namespace MakingDotNETApplicationsFaster.Runners.Models
{
    class Vector
    {
        private Point _location;
        public Point Location { get; set; }

        public ref Point RefLocation => ref this._location;

        private int _magnitude;

        public int Magnitude { get ; set; }

        public ref int RefMagnitude => ref this._magnitude;
    }
}
