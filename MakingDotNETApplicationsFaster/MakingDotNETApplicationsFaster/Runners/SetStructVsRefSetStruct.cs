using BenchmarkDotNet.Attributes;
using MakingDotNETApplicationsFaster.Runners.Models;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class SetStructVsRefSetStruct
    {
        private const int Iterations = 10000;
        private readonly Vector _vector;

        public SetStructVsRefSetStruct()
        {
            _vector = new Vector 
            { 
                Location = new Point 
                { 
                    x = 1, 
                    y = 2, 
                    z = 3 
                }, 
                Magnitude = 2 
            };
        }

        [Benchmark]
        public void SetStructToOrigin()
        {
            for (int i = 0; i < Iterations; i++)
            {
                SetVectorToOrigin(_vector);
            }
        }

        [Benchmark]
        public void RefSetStructToOrigin()
        {
            for (int i = 0; i < Iterations; i++)
            {
                RefSetVectorToOrigin(_vector);
            }
        }

        private static void SetVectorToOrigin(Vector vector)
        {
            Point point = vector.Location;
            point.x = 4;
            point.y = 6;
            point.z = 6;
            vector.Location = point;
        }

        private static void RefSetVectorToOrigin(Vector vector)
        {
            ref Point location = ref vector.RefLocation;
            location.x = 4;
            location.y = 5;
            location.z = 6;
        }
    }
}
