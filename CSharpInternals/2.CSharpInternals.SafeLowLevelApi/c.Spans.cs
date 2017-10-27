using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CSharpInternals.Utils;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.SafeLowLevelApi
{
    public class Spans : BaseTestHelpersClass
    {
        public Spans(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void SpanBasics()
        {
            var array = new []{ 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            var span = new Span<int>(array).Slice(6);

            Assert.Equal(4, span.Length);
            
            ref var sourceRef = ref array[6];
            ref var pinnableRef = ref span.DangerousGetPinnableReference(); // "ref span[0]"
            
            Assert.True(Unsafe.AreSame(ref sourceRef, ref pinnableRef));
            
            WriteLine("Before:");
            WriteArray(array);
            
            span[0] = -span[0];

            WriteLine("After:");
            WriteArray(array);
        }

        [Fact]
        public void ReadOnlySpanBasics()
        {
            ReadOnlySpan<char> readOnlySpan = "We will talk about Strings a bit later".AsSpan(); // AsReadOnlySpan
            ProcessData(readOnlySpan);
        }

        private void ProcessData(ReadOnlySpan<char> readOnlySpan)
        {
            while (readOnlySpan.Length > 0)
            {
                var position = readOnlySpan.IndexOf(' ');
                if (position == -1)
                {
                    // show last segment
                    WriteArray(readOnlySpan);
                    break;
                }
                
                var wordSpan = readOnlySpan.Slice(0, position);
                WriteArray(wordSpan);
                
                readOnlySpan = readOnlySpan.Slice(position + 1);  
            }
        }


        private void WriteArray<T>([NotNull] IEnumerable<T> array, string separator = ",")
        {
            WriteLine(string.Join(separator, array));
        }
        
        private void WriteArray(ReadOnlySpan<char> span)
        {
            WriteArray(span.ToArray(), string.Empty);
        }
    }
}