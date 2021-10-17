using System.IO;
using BenchmarkDotNet.Attributes;
using MakingDotNETApplicationsFaster.Runners.Models;
using Newtonsoft.Json;
using Wire;
using ZeroFormatter;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class SerializersPerformanceRunner
    {
        private readonly Summary _summary;
        private readonly Serializer _wireSerializer = new Serializer(new SerializerOptions(true));
        private readonly string _summaryJson;
        private readonly byte[] _summaryBytes;
        private readonly byte[] _summaryZeroFormatterBytes;
        public SerializersPerformanceRunner()
        {
            _summary = new Summary();
            _summaryJson = NewtonsoftJsonSerialization();
            _summaryBytes = WireBinarySerialization();
            _summaryZeroFormatterBytes = ZeroFormatterBinarySerialization();
        }

        [Benchmark]
        public string NewtonsoftJsonSerialization()
        {
            return JsonConvert.SerializeObject(_summary);
        }

        [Benchmark]
        public byte[] WireBinarySerialization()
        {
            using (var memoryStream = new MemoryStream())
            {
                _wireSerializer.Serialize(_summary, memoryStream);
                return  memoryStream.ToArray();
            }
        }

        [Benchmark]
        public byte[] ZeroFormatterBinarySerialization()
        {
            return ZeroFormatterSerializer.Serialize(_summary);
        }

        [Benchmark]
        public Summary NewtonsoftJsonDeserializer()
        {
            return JsonConvert.DeserializeObject<Summary>(_summaryJson);
        }

        [Benchmark]
        public Summary WireBinaryDeserializer()
        {
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(_summaryBytes, 0, _summaryBytes.Length);
                memoryStream.Position = 0;
                return _wireSerializer.Deserialize<Summary>(memoryStream);
            }
        }

        [Benchmark]
        public Summary ZeroFormatterBinaryDeserializer()
        {
            return ZeroFormatterSerializer.Deserialize<Summary>(_summaryZeroFormatterBytes);
        }
    }

}