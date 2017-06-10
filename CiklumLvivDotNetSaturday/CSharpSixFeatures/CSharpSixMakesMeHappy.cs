using System;
using System.ComponentModel;
using System.Threading.Tasks;
using static CSharpSixFeatures.Helpers;

namespace CSharpSixFeatures.CSharpSixMakesMeHappy
{
    public static class ExpressionBodiesAndExtensions
    {
        public static bool HasValue(this string s) => !string.IsNullOrEmpty(s);

        public static T Get<T>(string key) where T : class
        {
            var serialized = GetFromCache(key);
            if (serialized == null)
            {
                return null;
            }

            return serialized.FromJson<T>();
        }

        public static T GetNew<T>(string key) where T : class => GetFromCache(key).FromJson<T>();
    }

    public class ExpressionBodiesAndMembers
    {
        public static string First { get; set; } = "Jane";
        public static string Last { get; set; } = "Doe";
        public string Name => First + " " + Last;
    }

    public class AwaitCatchFinallyBlocks
    {
        private async Task DoWorkAsync()
        {
            try
            {
                await Task.Yield();
            }
            finally
            {
                Task.Run(async () => await Task.Delay(100)).Wait();
            }
        }

        private async Task DoWorkNewAsync()
        {
            try
            {
                await Task.Yield();
            }
            finally
            {
                await Task.Delay(100);
            }
        }
    }

    public class ExceptionFilters
    {
        private void MakeRequest()
        {
            var httpStatusCode = 404;
            try
            {
                throw new Exception(httpStatusCode.ToString());
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("500"))
                    Console.WriteLine("Bad Request");
                else if (ex.Message.Equals("401"))
                    Console.WriteLine("Unauthorized");
                else if (ex.Message.Equals("402"))
                    Console.WriteLine("Payment Required");
                else if (ex.Message.Equals("403"))
                    Console.WriteLine("Forbidden");
                else if (ex.Message.Equals("404"))
                    Console.WriteLine("Not Found");
            }
        }

        private void MakeRequestNew()
        {
            var httpStatusCode = 404;
            try
            {
                throw new Exception(httpStatusCode.ToString());
            }
            catch (Exception ex) when (ex.Message.Equals("400"))
            {
                Console.WriteLine("Bad Request");
            }
            catch (Exception ex) when (ex.Message.Equals("401"))
            {
                Console.WriteLine("Unauthorized");
            }
            catch (Exception ex) when (ex.Message.Equals("402"))
            {
                Console.WriteLine("Payment Required");
            }
            catch (Exception ex) when (ex.Message.Equals("403"))
            {
                Console.WriteLine("Forbidden");
            }
            catch (Exception ex) when (ex.Message.Equals("404"))
            {
                Console.WriteLine("Not Found");
            }
        }
    }


    public class Person : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Person(string name)
        {
            Name = name;
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }
    }
}
