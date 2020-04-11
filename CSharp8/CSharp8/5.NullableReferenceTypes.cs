using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Abstractions;

namespace CSharp8
{ 
    public class NullableReferenceTypes
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public NullableReferenceTypes(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Basics()
        {
            string? x = null;
            string y = null;

            Assert.Throws<NullReferenceException>(() =>
            {
                _testOutputHelper.WriteLine(x.Length.ToString());
                _testOutputHelper.WriteLine(y.Length.ToString());
            });

            string z = null!;
            var l = z!?.Length;
            // var l = z?!.Length; won't compile
        }

        [Fact]
        public void Advanced()
        {
            Action<string?> actionA = s =>
            {
                _testOutputHelper.WriteLine(s);
            };
            Action<string> actionB = actionA;
            
            List<string> listA = new List<string>();
            List<string?> listB = listA;
        }

        [Fact]
        public void SuperAdvanced()
        {
            int ArrayAccess(int[][,][,,] array) => array[1][2, 2][3, 3, 3];
            
            //int ArrayAccess2(int[]?[,]?[,,]? array) => array[1][2, 2][3, 3, 3];
            int? ArrayAccess2(int[]?[,]?[,,]? array) => array?[3, 3, 3]?[2, 2]?[1];

            int? ArrayAccess3(int[]?[,][,,] array) => array[2, 2][3, 3, 3]?[1];

            var array = new int[]?[10][];
            /* nullable types are disallowed in pattern matching
             *  var a = x is int? true : false
             *  var a = x is int ? true : false
             */
        }
        
        [Fact]
        public void ComplexFlowField()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                new ChangingField("Igor", _testOutputHelper).PrintNameLength();
            });
        }

        [Fact]
        public void Attributes()
        {
            string? text = null;
            _testOutputHelper.WriteLine(StringIsNullOrEmpty(text) 
                ? "It was null or empty" 
                : text.Length.ToString());
        }

        public class ChangingField
        {
            //[AllowNull]
            private string? _name;
            private readonly ITestOutputHelper _testOutputHelper;

            public ChangingField(string? name, ITestOutputHelper testOutputHelper)
            {
                _name = name;
                _testOutputHelper = testOutputHelper;
            }
            
            public void PrintNameLength()
            {
                //if (_name != null)
                {
                    DoSomethingEvil();
                    _testOutputHelper.WriteLine(_name.Length.ToString());
                }
            }

            private void DoSomethingEvil()
            {
                _name = null;
            }
        }
        
        
        [return: MaybeNull]
        public static T Find<T>(T[] array, Func<T, bool> match)
        {
            return default;
        }

        private static bool StringIsNullOrEmpty([NotNullWhen(false)] string? text) =>
            string.IsNullOrEmpty(text);
    }
}