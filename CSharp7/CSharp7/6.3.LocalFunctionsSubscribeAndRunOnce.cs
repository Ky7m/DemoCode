// https://ayende.com/blog/178115/fun-with-c-local-functions
using System;
namespace CSharp7
{
    class LocalFunctionsSubscribeAndRunOnce
    {
        private event EventHandler EventHandler;

        public LocalFunctionsSubscribeAndRunOnce()
        {
			var line = Console.ReadLine();

			void PrintUserInput(object sender, EventArgs e)
			{
				Console.WriteLine(line);
                EventHandler -= PrintUserInput;
			}

			EventHandler += PrintUserInput;

			EventHandler?.Invoke(null, EventArgs.Empty);
            EventHandler?.Invoke(null, EventArgs.Empty);
        }
    }
}
