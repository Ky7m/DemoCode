using BenchmarkDotNet.Attributes;
using System.Linq;
using System.Collections.Generic;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class SwitchVsIfOperatorsForIntRunner
    {
        [Params(1, 3, 5, 10, 15, 20)]
        public int Input;

        private readonly Dictionary<int, int> _keyValues;

        public SwitchVsIfOperatorsForIntRunner()
        {
            var values = Enumerable.Range(1, 51).ToList();

            _keyValues = new Dictionary<int, int>();

            for (var index = 0; index < values.Count; ++index)
            {
                if (index == values.Count - 1)
                {
                    _keyValues.Add(values[index], 0);
                    break;
                }

                _keyValues.Add(values[index], values[index]);
            }
        }

        [Benchmark]
        public int IfWithLongCycleOrOperator()
        {
            int result = 0;

            if (Input == 1 |
                Input == 2 |
                Input == 3 |
                Input == 4 |
                Input == 5 |
                Input == 6 |
                Input == 7 |
                Input == 8 |
                Input == 9 |
                Input == 10 |
                Input == 11 |
                Input == 12 |
                Input == 13 |
                Input == 14 |
                Input == 15 |
                Input == 16 |
                Input == 17 |
                Input == 18 |
                Input == 19 |
                Input == 20)
            {
                result = Input;
            }

            return result;
        }

        [Benchmark]
        public int IfWithShortCycleOrOperator()
        {
            int result = 0;

            if (Input == 1 ||
                Input == 2 ||
                Input == 3 ||
                Input == 4 ||
                Input == 5 ||
                Input == 6 ||
                Input == 7 ||
                Input == 8 ||
                Input == 9 ||
                Input == 10 ||
                Input == 11 ||
                Input == 12 ||
                Input == 13 ||
                Input == 14 ||
                Input == 15 ||
                Input == 16 ||
                Input == 17 ||
                Input == 18 ||
                Input == 19 ||
                Input == 20)
            {
                result = Input;
            }

            return result;
        }

        [Benchmark]
        public int IfElseOperator()
        {
            int result;

            if (Input == 1)
            {
                result = Input;
            }
            else if (Input == 2)
            {
                result = Input;
            }
            else if (Input == 3)
            {
                result = Input;
            }
            else if (Input == 4)
            {
                result = Input;
            }
            else if (Input == 5)
            {
                result = Input;
            }
            else if (Input == 6)
            {
                result = Input;
            }
            else if (Input == 7)
            {
                result = Input;
            }
            else if (Input == 8)
            {
                result = Input;
            }
            else if (Input == 9)
            {
                result = Input;
            }
            else if (Input == 10)
            {
                result = Input;
            }
            else if (Input == 11)
            {
                result = Input;
            }
            else if (Input == 12)
            {
                result = Input;
            }
            else if (Input == 13)
            {
                result = Input;
            }
            else if (Input == 14)
            {
                result = Input;
            }
            else if (Input == 15)
            {
                result = Input;
            }
            else if (Input == 16)
            {
                result = Input;
            }
            else if (Input == 17)
            {
                result = Input;
            }
            else if (Input == 18)
            {
                result = Input;
            }
            else if (Input == 19)
            {
                result = Input;
            }
            else if (Input == 20)
            {
                result = Input;
            }
            else
            {
                result = 0;
            }

            return result;
        }

        [Benchmark]
        public int SwitchOperator()
        {
            int result;

            switch (Input)
            {
                case 1:
                    result = Input;
                    break;
                case 2:
                    result = Input;
                    break;
                case 3:
                    result = Input;
                    break;
                case 4:
                    result = Input;
                    break;
                case 5:
                    result = Input;
                    break;
                case 6:
                    result = Input;
                    break;
                case 7:
                    result = Input;
                    break;
                case 8:
                    result = Input;
                    break;
                case 9:
                    result = Input;
                    break;
                case 10:
                    result = Input;
                    break;
                case 11:
                    result = Input;
                    break;
                case 12:
                    result = Input;
                    break;
                case 13:
                    result = Input;
                    break;
                case 14:
                    result = Input;
                    break;
                case 15:
                    result = Input;
                    break;
                case 16:
                    result = Input;
                    break;
                case 17:
                    result = Input;
                    break;
                case 18:
                    result = Input;
                    break;
                case 19:
                    result = Input;
                    break;
                case 20:
                    result = Input;
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;
        }


        [Benchmark]
        public int TryGetValueFromDictionary()
        {
            int result;

            return _keyValues.TryGetValue(Input, out result) ? result : 0;
        }
    }
}
