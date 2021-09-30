using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.IO;
using Newtonsoft.Json;
using Xunit;

namespace DarkSideOfCSharp
{
    public class RecyclableMemoryStreamBenchmarks
    {
        private readonly WebhookResponse<CountryIndicator> _webhookResponse = new ()
        {
            Data = Enumerable.Range(1, 3000)
                .Select(i => new CountryIndicator($"Country #{i}",$"Indicator #{i}", i))
                .ToArray()
        };
        private readonly RecyclableMemoryStreamManager _memoryStreamManager = new ();
        private readonly JsonSerializer _jsonSerializer = new();
        private static readonly byte[] HmacKey = Encoding.UTF8.GetBytes("Dark side of C#");

        [Fact]
        public void AssertThatAllMethodsProduceSameResult()
        {
            var expected = SerializeAndSign();
            Assert.Equal(expected,SerializeAndSignUsingMemoryStream());
            Assert.Equal(expected,SerializeAndSignUsingRecyclableMemoryStream());
            Assert.Equal(expected,SerializeAndSignUsingSystemTextJson());
        }
        
        [Benchmark(Baseline = true)]
        public byte[] SerializeAndSign()
        {
            var json = JsonConvert.SerializeObject(_webhookResponse);
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            using var hmac = new HMACSHA256(HmacKey);
            return hmac.ComputeHash(jsonBytes);
        }
        
        [Benchmark]
        public byte[] SerializeAndSignUsingSystemTextJson()
        {
            var jsonBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(_webhookResponse);
            using var hmac = new HMACSHA256(HmacKey);
            return hmac.ComputeHash(jsonBytes);
        }

        [Benchmark]
        public byte[] SerializeAndSignUsingMemoryStream()
        {
            using var jsonStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(jsonStream, leaveOpen: true))
            {
                _jsonSerializer.Serialize(streamWriter, _webhookResponse);
            }
            
            jsonStream.Position = 0;

            using var hmac = new HMACSHA256(HmacKey);
            return hmac.ComputeHash(jsonStream);
        }
        
        [Benchmark]
        public byte[] SerializeAndSignUsingRecyclableMemoryStream()
        {
            using var jsonStream = _memoryStreamManager.GetStream();
            using (var streamWriter = new StreamWriter(jsonStream, leaveOpen: true))
            {
                _jsonSerializer.Serialize(streamWriter, _webhookResponse);
            }
                
            jsonStream.Position = 0;

            using var hmac = new HMACSHA256(HmacKey);
            return hmac.ComputeHash(jsonStream);
        }
    }
    
    public sealed class WebhookResponse<T>
    {
        public T[]? Data { get; set; }
    }
    public sealed class CountryIndicator
    {
        public CountryIndicator(string country, string indicator, int value)
        {
            Country = country;
            Indicator = indicator;
            Value = value;
        }

        public string Country { get; set; }
        public string Indicator { get; set; }
        public int Value { get; set; }
    }
}