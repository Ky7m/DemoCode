using System.Collections.Generic;
using System.Linq;
using CSharpInternals.Utils;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.Numbers
{
    public class Sum : BaseTestHelpersClass
    {
        private readonly List<double> _listOfDoubles = new List<double>() { 10e15 };
        
        public Sum(ITestOutputHelper output) : base(output)
        {
            for (var i = 0; i < 100; i++)
            {
                _listOfDoubles.Add(1);
            }
        }

        [Fact]
        public void NonOverflowOfComputableSumSingle()
        {
            var source = new List<double>
            { 
                double.MaxValue, double.MaxValue, 
                -double.MaxValue, -double.MaxValue 
            }; 
            // In a world where we summed using a float accumulator, the 
            // result would be infinity. 
            Assert.NotEqual(0.0, source.Sum()); 
        }
        
        [Fact]
        public void PlainSummation()
        {
            var sum = Plain(_listOfDoubles);
            WriteLine($"{sum:N}");
        }
        
        [Fact]
        public void KahanSummation()
        {
            var sum = Kahan(_listOfDoubles);
            WriteLine($"{sum:N}");
        }

        private static double Plain(List<double> vals)
        {
            var sum = 0.0;
            foreach (var number in vals)
            {
                sum += number;
            }
            return sum;
        }

        private static double Kahan(List<double> vals)
        {
            var sum = 0.0;
            var compensation = 0.0;
            foreach (var number in vals)
            {
                var tmpNum = number - compensation;
                var tmpSum = sum + tmpNum;

                compensation = (tmpSum - sum) - tmpNum;
                sum = tmpSum;
            }
            return sum;
        }
    }
}