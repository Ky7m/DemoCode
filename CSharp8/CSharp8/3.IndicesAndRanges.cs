using System;
using Xunit;

namespace CSharp8
{
    public class IndicesAndRanges
    {
         /*
         * Two new types
         * Index refers to a single position
         * Range refers to a range of positions
         */

         [Fact]
         public void Indices()
         {
             Index index1 = 2;
             var index2 = new Index(0,true);

             var index3 = ^0;
             Assert.Equal(index2, index3);

             var array = new[] {1, 2, 3};
             array[^1] *= -1;
             Assert.Equal(-3, array[2]);
         }
         
         [Fact]
         public void Ranges()
         {
             var range = 0..4;
             // range = ..4;
             // range = ..;
             // _ = ..;

             var array = new[] {1, 2, 3, 4, 5};
             
             Assert.Equal(new[]{1,2,3,4},array[range]);

             Assert.Equal(new[]{1,2,3,4,5},array[0..^0]);
             
             Assert.Throws<ArgumentOutOfRangeException>(() =>
             {
                 var i1 = ^2;
                 var inverse = array[i1..0];
             });

             var conf = "Conference";
             Assert.Equal("Conf",conf.AsMemory(range).ToString());
         }
    }
}