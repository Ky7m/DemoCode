using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DemoCode.Utils;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.Strings
{
    public class StringInterpolation : BaseTestHelpersClass
    {
        public StringInterpolation(ITestOutputHelper output) : base(output) { }
        
        [Fact]
        public void WholeProgramInString()
        {
            WriteLine($@"{((Func<Task<string>>) (async () =>
            {
                WriteLine("Hello from there! Dowloading information...");
                return await new WebClient().DownloadStringTaskAsync("https://www.ifesenko.com/robots.txt");
            }))().GetAwaiter().GetResult()}!");
        }

        [Fact]
        public void SqlCommand()
        {
            var name = "Igor'; DROP TABLE Users;--";
            using (var connection = new SqlConnection())
            {
                using (var command = new SqlCommand($"SELECT * FROM Users WHERE Name='{name}'", connection))
                {
                    WriteLine(command.CommandText);
                }
            }
        }
        
        [Fact]
        public void NewSqlCommand()
        {
            var name = "Igor'; DROP TABLE Users;--";
            using (var connection = new SqlConnection())
            {
                using (var command = connection.NewSqlCommand($"SELECT * FROM Users WHERE Name='{name}'"))
                {
                    WriteLine(command.CommandText);
                    var p = command.Parameters[0];
                    WriteLine($"{p.ParameterName}: Type={p.SqlDbType}; Value={p.Value}");
                }
            }
        }
    }
    
    // Do not use this code in production!
    public static class SqlFormattableString
    {
        public static SqlCommand NewSqlCommand(this SqlConnection conn, FormattableString formattableString)
        {
            var sqlParameters = formattableString.GetArguments()
                .Select((value, position) =>
                    new SqlParameter(FormattableString.Invariant($"@p{position}"), value))
                .ToArray();
            
            var formatArguments = sqlParameters.Select(p => p.ParameterName).ToArray();
            
            var sql = string.Format(formattableString.Format, formatArguments);
            
            var command = new SqlCommand(sql, conn);
            command.Parameters.AddRange(sqlParameters);
            return command;
        }
    }
}