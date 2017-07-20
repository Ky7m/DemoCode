using System;
using System.Collections.Generic;
using static System.Console;

namespace CSharp7
{
    public sealed class PatternMatchingSwitch
    {
        public PatternMatchingSwitch()
        {
            var obj = "5";
            SwitchPatterns(obj);
        }
        void SwitchPatterns(object objectToTest)
        {
            switch (objectToTest)
            {
                case int i when i == 5:
                    WriteLine($"found the secret");
                    break;

                case int i:
                    WriteLine($"found an int {i}");
                    break;

                case null:
                    throw new ArgumentNullException(nameof(objectToTest));

                case string s when string.IsNullOrEmpty(s):
                    WriteLine("the empty string");
                    break;

                case string s:
                    WriteLine($"Found a string {s}");
                    break;

                case Point p when p.X == 0 && p.Y == 0:
                    WriteLine($"Found a point");
                    break;

                case IEnumerable<int> intList:
                    break;

                case var o:
                    WriteLine(o.GetType());
                    break;

                //case (int,int) tuple: break;

                default:
                    WriteLine("No ideas yet");
                    break;
            }
        }

        struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

    }
}
