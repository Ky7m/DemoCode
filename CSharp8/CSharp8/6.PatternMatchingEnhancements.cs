using System;
using Xunit;
using Xunit.Abstractions;

namespace CSharp8
{
    public class PatternMatchingEnhancements
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PatternMatchingEnhancements(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void SwitchExpression()
        {
            Assert.Equal(5, Fib(5));
        }
        
        [Fact]
        public void PropertyPatterns()
        {
            var point = new Point(0,1);
            var t = point switch
            {
                Point { X: 0, Y: 0 } p => "origin",
                { X: var x, Y: 1 } p => $"({x})",
                {} o => o.ToString(),
                null => "null",
                //_ => "unknown"
            };
        }

        [Fact]
        public void TuplePatterns()
        {
            string RockPaperScissors(string first, string second)
                => (first, second) switch
                {
                    ("rock", "paper") => "rock is covered by paper. Paper wins.",
                    ("rock", "scissors") => "rock breaks scissors. Rock wins.",
                    ("paper", "rock") => "paper covers rock. Paper wins.",
                    ("paper", "scissors") => "paper is cut by scissors. Scissors wins.",
                    ("scissors", "rock") => "scissors is broken by rock. Rock wins.",
                    ("scissors", "paper") => "scissors cuts paper. Scissors wins.",
                    (_, _) => "tie"
                };
        }

        [Fact]
        public void PositionalPatterns()
        {
            ShowFact(new DateTime(2020, 2, 29));
            ShowFact(new DateTime(2020, 3, 1));

            void ShowFact(DateTime date) => _testOutputHelper.WriteLine(GetInterestingFact(date));
        }

        private static int Fib(int n)
        {
            var result = n switch
            {
                _ when n < 0 => throw new ArgumentOutOfRangeException(),
                0 => 0,
                1 => 1,
                _ => Fib(n - 1) + Fib(n - 2)
            };
            return result;
        }

        private static string GetInterestingFact(DateTime date) => date switch
        {
            (_, _, 1) => $"{date:d} is the first of the month",
            (int year, 2, 29) => $"{year} is a leap year",
            _ => $"{date:d} is a boring date"
        };
        
        public class Point
        {
            public int X { get; }
            public int Y { get; }
            public Point(int x, int y) => (X, Y) = (x, y);
        }
    }
    
    static class DateTimeExtensions
    {
        public static void Deconstruct(this DateTime date, out int year, out int month, out int day) =>
            (year, month, day) = (date.Year, date.Month, date.Day);
    }
}