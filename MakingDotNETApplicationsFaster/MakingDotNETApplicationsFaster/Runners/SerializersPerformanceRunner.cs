using System.IO;
using BenchmarkDotNet.Attributes;
using MakingDotNETApplicationsFaster.Runners.Models;
using Newtonsoft.Json;
using ZeroFormatter;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class SerializersPerformanceRunner
    {
        private readonly Summary _summary;
        private readonly string _summaryJson;
        private readonly byte[] _summaryZeroFormatterBytes;
        public SerializersPerformanceRunner()
        {
            _summary = new Summary();
            _summaryJson = NewtonsoftJsonSerialization();
            _summaryZeroFormatterBytes = ZeroFormatterBinarySerialization();
        }

        [Benchmark]
        public string NewtonsoftJsonSerialization()
        {
            return JsonConvert.SerializeObject(_summary);
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
        public Summary ZeroFormatterBinaryDeserializer()
        {
            return ZeroFormatterSerializer.Deserialize<Summary>(_summaryZeroFormatterBytes);
        }
    }

}