using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class SwitchVsIfOperatorsForStringRunner
    {
        [Params(3, 5, 10, 25, 50)]
        public int Input;

        private readonly Dictionary<string, int> _keyValues;

        public SwitchVsIfOperatorsForStringRunner()
        {
            var values = Enumerable.Range(1, 51).ToList();

            _keyValues = new Dictionary<string, int>();

            for (var index = 0; index < values.Count; ++index)
            {
                if (index == values.Count - 1)
                {
                    _keyValues.Add(values[index].ToString(), 0);
                    break;
                }

                _keyValues.Add(values[index].ToString(), values[index]);
            }
        }

        [Benchmark]
        public int SwitchFor3ItemsOperator()
        {
            int result;

            switch (Input.ToString())
            {
                case "1":
                    result = Input;
                    break;
                case "2":
                    result = Input;
                    break;
                case "3":
                    result = Input;
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;
        }

        [Benchmark]
        public int SwitchFor6ItemsOperator()
        {
            int result;

            switch (Input.ToString())
            {
                case "1":
                    result = Input;
                    break;
                case "2":
                    result = Input;
                    break;
                case "3":
                    result = Input;
                    break;
                case "4":
                    result = Input;
                    break;
                case "5":
                    result = Input;
                    break;
                case "6":
                    result = Input;
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;
        }

        [Benchmark]
        public int SwitchFor7ItemsOperator()
        {
            int result;

            switch (Input.ToString())
            {
                case "1":
                    result = Input;
                    break;
                case "2":
                    result = Input;
                    break;
                case "3":
                    result = Input;
                    break;
                case "4":
                    result = Input;
                    break;
                case "5":
                    result = Input;
                    break;
                case "6":
                    result = Input;
                    break;
                case "7":
                    result = Input;
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;
        }

        [Benchmark]
        public int SwitchFor50ItemsOperator()
        {
            int result;

            switch (Input.ToString())
            {
                case "1":
                    result = Input;
                    break;
                case "2":
                    result = Input;
                    break;
                case "3":
                    result = Input;
                    break;
                case "4":
                    result = Input;
                    break;
                case "5":
                    result = Input;
                    break;
                case "6":
                    result = Input;
                    break;
                case "7":
                    result = Input;
                    break;
                case "8":
                    result = Input;
                    break;
                case "9":
                    result = Input;
                    break;
                case "10":
                    result = Input;
                    break;
                case "11":
                    result = Input;
                    break;
                case "12":
                    result = Input;
                    break;
                case "13":
                    result = Input;
                    break;
                case "14":
                    result = Input;
                    break;
                case "15":
                    result = Input;
                    break;
                case "16":
                    result = Input;
                    break;
                case "17":
                    result = Input;
                    break;
                case "18":
                    result = Input;
                    break;
                case "19":
                    result = Input;
                    break;
                case "20":
                    result = Input;
                    break;
                case "21":
                    result = Input;
                    break;
                case "22":
                    result = Input;
                    break;
                case "23":
                    result = Input;
                    break;
                case "24":
                    result = Input;
                    break;
                case "25":
                    result = Input;
                    break;
                case "26":
                    result = Input;
                    break;
                case "27":
                    result = Input;
                    break;
                case "28":
                    result = Input;
                    break;
                case "29":
                    result = Input;
                    break;
                case "30":
                    result = Input;
                    break;
                case "31":
                    result = Input;
                    break;
                case "32":
                    result = Input;
                    break;
                case "33":
                    result = Input;
                    break;
                case "34":
                    result = Input;
                    break;
                case "35":
                    result = Input;
                    break;
                case "36":
                    result = Input;
                    break;
                case "37":
                    result = Input;
                    break;
                case "38":
                    result = Input;
                    break;
                case "39":
                    result = Input;
                    break;
                case "40":
                    result = Input;
                    break;
                case "41":
                    result = Input;
                    break;
                case "42":
                    result = Input;
                    break;
                case "43":
                    result = Input;
                    break;
                case "44":
                    result = Input;
                    break;
                case "45":
                    result = Input;
                    break;
                case "46":
                    result = Input;
                    break;
                case "47":
                    result = Input;
                    break;
                case "48":
                    result = Input;
                    break;
                case "49":
                    result = Input;
                    break;
                case "50":
                    result = Input;
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;
        }

        [Benchmark]
        public int IfElseOperator()
        {
            int result;

            var input = Input.ToString();

            if (input == "1")
            {
                result = Input;
            }
            else if (input == "2")
            {
                result = Input;
            }
            else if (input == "3")
            {
                result = Input;
            }
            else if (input == "4")
            {
                result = Input;
            }
            else if (input == "5")
            {
                result = Input;
            }
            else if (input == "6")
            {
                result = Input;
            }
            else if (input == "7")
            {
                result = Input;
            }
            else if (input == "8")
            {
                result = Input;
            }
            else if (input == "9")
            {
                result = Input;
            }
            else if (input == "10")
            {
                result = Input;
            }
            else if (input == "11")
            {
                result = Input;
            }
            else if (input == "12")
            {
                result = Input;
            }
            else if (input == "13")
            {
                result = Input;
            }
            else if (input == "14")
            {
                result = Input;
            }
            else if (input == "15")
            {
                result = Input;
            }
            else if (input == "16")
            {
                result = Input;
            }
            else if (input == "17")
            {
                result = Input;
            }
            else if (input == "18")
            {
                result = Input;
            }
            else if (input == "19")
            {
                result = Input;
            }
            else if (input == "20")
            {
                result = Input;
            }
            else if (input == "21")
            {
                result = Input;
            }
            else if (input == "22")
            {
                result = Input;
            }
            else if (input == "23")
            {
                result = Input;
            }
            else if (input == "24")
            {
                result = Input;
            }
            else if (input == "25")
            {
                result = Input;
            }
            else if (input == "26")
            {
                result = Input;
            }
            else if (input == "27")
            {
                result = Input;
            }
            else if (input == "28")
            {
                result = Input;
            }
            else if (input == "29")
            {
                result = Input;
            }
            else if (input == "30")
            {
                result = Input;
            }
            else if (input == "31")
            {
                result = Input;
            }
            else if (input == "32")
            {
                result = Input;
            }
            else if (input == "33")
            {
                result = Input;
            }
            else if (input == "34")
            {
                result = Input;
            }
            else if (input == "35")
            {
                result = Input;
            }
            else if (input == "36")
            {
                result = Input;
            }
            else if (input == "37")
            {
                result = Input;
            }
            else if (input == "38")
            {
                result = Input;
            }
            else if (input == "39")
            {
                result = Input;
            }
            else if (input == "40")
            {
                result = Input;
            }
            else if (input == "41")
            {
                result = Input;
            }
            else if (input == "42")
            {
                result = Input;
            }
            else if (input == "43")
            {
                result = Input;
            }
            else if (input == "44")
            {
                result = Input;
            }
            else if (input == "45")
            {
                result = Input;
            }
            else if (input == "46")
            {
                result = Input;
            }
            else if (input == "47")
            {
                result = Input;
            }
            else if (input == "48")
            {
                result = Input;
            }
            else if (input == "49")
            {
                result = Input;
            }
            else if (input == "50")
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
        public int TryGetValueFromDictionary()
        {
            int result;

            return _keyValues.TryGetValue(Input.ToString(), out result) ? result : 0;
        }
    }
}
