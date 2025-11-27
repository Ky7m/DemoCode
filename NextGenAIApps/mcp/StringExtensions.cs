
namespace mcp;

public static class StringExtensions
{
    /// <summary />
    public static string NormalizeSqlTextResponse(this string jsonString) =>
        jsonString
            .Replace("```sql", string.Empty)
            .Replace("```", string.Empty);
}