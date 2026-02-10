using System.ComponentModel;
using System.Text.Json;
using Dapper;
using mcp;
using Microsoft.Agents.AI;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Caching.Memory;
using ModelContextProtocol.Server;

namespace Mcp.Tools;

[McpServerToolType]
public sealed class DataQueryTools(
    IChatClient chatClient,
    SqlConnection connection,
    IMemoryCache memoryCache,
    ILogger<DataQueryTools> logger)
{
    private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions =
        new() { SlidingExpiration = TimeSpan.FromHours(1) };
    
    [McpServerTool(
         Name = "query_data",
         Title = "Query data",
         UseStructuredContent = false,
         ReadOnly = true
     ),
     Description("Ask questions in plain language and receive structured, human-readable answers from data")]
    public async Task<string> QueryData(
        [Description("The natural language question provided by the user")]
        string userQuery)
    {
        var sqlQuery = await NaturalLanguageToSqlAsync(userQuery);
        var sqlResults = await ExecuteSqlQueryAsync(sqlQuery);
        var summary = await SummarizeResultsAsync(userQuery, sqlResults);
        return summary;
    }

    private async Task<dynamic[]> ExecuteSqlQueryAsync(string sqlQuery)
    {
        var results = await connection.QueryAsync(sqlQuery);
        return results.ToArray();
    }

    private async Task<string> NaturalLanguageToSqlAsync(string userQuery)
    {
        var schemaContext = await GetSqlSchemaContextAsync("[dbo].[Images]");
        
        var agent = chatClient.AsAIAgent(
            name: "NaturalLanguageToSqlAgent",
            instructions:
            $"""
             You are an expert at converting natural language to SQL queries.
             Given a user's question, generate an appropriate SQL query to retrieve the relevant data.
             Ensure the SQL syntax is correct and optimized for performance.
             Only generate SQL queries without any additional explanations.
             The SQL will query Azure SQL Database.

             Today's date is {DateTime.Now:yyyy-MM-dd}

             Database Schema Information:
             {schemaContext}
             """
        );
        
        var response = await agent.RunAsync(userQuery, options: new ChatClientAgentRunOptions(new ChatOptions { Temperature = 0f, TopP = 0.01f }));
        var sqlQuery = response.Text.NormalizeSqlTextResponse();
        
        logger.LogInformation("Generated SQL Query: {SqlQuery}", sqlQuery);
        
        return sqlQuery;
    }

    private async Task<string> GetSqlSchemaContextAsync(string tableName)
    {
        if (!memoryCache.TryGetValue(tableName, out string? sqlSchemaContext))
        {

            var result = await DescribeTableAsync(tableName);
            if (result.Count > 0)
            {
                sqlSchemaContext = JsonSerializer.Serialize(new { schema_items = result });
                memoryCache.Set(tableName, sqlSchemaContext, _memoryCacheEntryOptions);
            }
        }

        return sqlSchemaContext ?? string.Empty;
    }

    public async Task<Dictionary<string, object>> DescribeTableAsync(string tableName)
    {
        string? tableSchema = null;
        if (tableName.Contains('.'))
        {
            var parts = tableName.Split('.');
            if (parts.Length > 1)
            {
                tableName = parts[1].TrimStart('[').TrimEnd(']'); // Use only the table name part
                tableSchema = parts[0].TrimStart('[').TrimEnd(']'); // Use the first part as schema
            }
        }

        var sqlParameters = new { tableName, tableSchema };

        try
        {
            var result = await QueryTableMetadataAsync(sqlParameters);
            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error describing table {TableName}", tableName);
        }

        return new Dictionary<string, object>();
    }

    private async Task<string> SummarizeResultsAsync(string userQuery, dynamic[] sqlResults)
    {
        if (sqlResults.Length == 0)
        {
            return "No results found.";
        }

        var agent = chatClient.AsAIAgent(
            name: "SummarizationAgent",
            instructions:
            """
            You are an expert at summarizing database query results.
            Given a user's question and the corresponding SQL query results, generate a concise, accurate, and informative summary that directly addresses the user's question.
            Ensure the summary is clear, easy to understand, and focuses on delivering actionable insights.
            If a specific instruction is provided, ensure it is respected and reflected appropriately in the summary.
            """);

        var prompt = $"""
         User Question: {userQuery}

         SQL Query Results: {JsonSerializer.Serialize(sqlResults)}

         Provide a concise summary of the results that answers the user's question.
         """;

        var response = await agent.RunAsync(prompt, options: new ChatClientAgentRunOptions(new ChatOptions { Temperature = 0f, TopP = 0.1f }));
        return response.Text;
    }
    
    private async Task<Dictionary<string, object>> QueryTableMetadataAsync(object sqlParameters)
    {
        var result = new Dictionary<string, object>();
        
        var tableInfo = await connection.QueryFirstAsync(_tableMetadataSqlQuery, sqlParameters);
        if (string.IsNullOrEmpty(tableInfo.name))
        {
            return result;
        }

        result["table_name"] = tableInfo.name;
        result["table_description"] = tableInfo.description;
        result["table_schema"] = tableInfo.schema;

        var columnsInfo = await connection.QueryAsync(_columnsMetadataSqlQuery, sqlParameters);
        var columns = columnsInfo.Select(x => new Dictionary<string, object>
        {
            ["column_name"] = x.name,
            ["column_description"] = x.description,
            ["column_type"] = x.type,
            ["column_length"] = x.length,
            ["column_precision"] = x.precision,
            ["column_scale"] = x.scale,
            ["column_nullable"] = x.nullable
        });
        result["table_columns"] = columns;

        var indexesInfo = await connection.QueryAsync(_indexesMetadataSqlQuery, sqlParameters);
        var indexes = indexesInfo.Select(x => new Dictionary<string, object>
        {
            ["index_name"] = x.name,
            ["index_description"] = x.description,
            ["index_type"] = x.type,
            ["index_keys"] = x.keys
        });

        result["table_indexes"] = indexes;

        var constraintsInfo = await connection.QueryAsync(_constraintsMetadataSqlQuery, sqlParameters);
        var constraints = constraintsInfo.Select(x => new Dictionary<string, object>
        {
            ["constraint_name"] = x.name, ["constraint_type"] = x.type, ["constraint_keys"] = x.keys
        });
        result["table_constraints"] = constraints;

        var foreignKeysInfo = await connection.QueryAsync(_foreignKeysMetadataSqlQuery, sqlParameters);
        var foreignKeys = foreignKeysInfo.Select(x => new Dictionary<string, object>
        {
            ["foreign_key_name"] = x.name,
            ["foreign_key_schema"] = x.schema,
            ["foreign_key_table_name"] = x.table_name,
            ["foreign_key_column_names"] = x.column_names,
            ["referenced_schema"] = x.referenced_schema,
            ["referenced_table"] = x.referenced_table,
            ["referenced_column_names"] = x.referenced_column_names
        });
        result["table_foreign_keys"] = foreignKeys;

        return result;
    }
    
    
    #region Metadata SQL Queries

    private readonly string _tableMetadataSqlQuery =
        """
        SELECT t.name, s.name AS [schema], p.value AS description
        FROM sys.tables t
                 INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
                 LEFT JOIN sys.extended_properties p
                           ON p.major_id = t.object_id AND p.minor_id = 0 AND p.name = 'MS_Description'
                 LEFT JOIN sys.sysusers u ON t.principal_id = u.uid
        WHERE t.name = @tableName
          and (s.name = @tableSchema or @tableSchema IS NULL)
        """;

    private readonly string _columnsMetadataSqlQuery =
        """
        SELECT c.name,
               ty.name       AS type,
               c.max_length  AS length,
               c.precision,
               c.scale,
               c.is_nullable AS nullable,
               p.value       AS description
        FROM sys.columns c
                 INNER JOIN sys.types ty ON c.user_type_id = ty.user_type_id
                 LEFT JOIN sys.extended_properties p
                           ON p.major_id = c.object_id AND p.minor_id = c.column_id AND p.name = 'MS_Description'
        WHERE c.object_id = (SELECT object_id
                             FROM sys.tables t
                                      INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
                             WHERE t.name = @tableName
                               and (s.name = @tableSchema or @tableSchema IS NULL))
        """;

    private readonly string _indexesMetadataSqlQuery =
        """
        SELECT i.name,
               i.type_desc                         AS type,
               p.value                             AS description,
               STUFF((SELECT ',' + c.name
                      FROM sys.index_columns ic
                               INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                      WHERE ic.object_id = i.object_id
                        AND ic.index_id = i.index_id
                      ORDER BY ic.key_ordinal
                      FOR XML PATH('')), 1, 1, '') AS keys
        FROM sys.indexes i
                 LEFT JOIN sys.extended_properties p
                           ON p.major_id = i.object_id AND p.minor_id = i.index_id AND p.name = 'MS_Description'
        WHERE i.object_id = (SELECT object_id
                             FROM sys.tables t
                                      INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
                             WHERE t.name = @tableName
                               and (s.name = @tableSchema or @tableSchema IS NULL))
          AND i.is_primary_key = 0
          AND i.is_unique_constraint = 0
        """;


    // Query for constraints
    private readonly string _constraintsMetadataSqlQuery =
        """
        SELECT kc.name,
               kc.type_desc                        AS type,
               STUFF((SELECT ',' + c.name
                      FROM sys.index_columns ic
                               INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                      WHERE ic.object_id = kc.parent_object_id
                        AND ic.index_id = kc.unique_index_id
                      ORDER BY ic.key_ordinal
                      FOR XML PATH('')), 1, 1, '') AS keys
        FROM sys.key_constraints kc
        WHERE kc.parent_object_id = (SELECT object_id
                                     FROM sys.tables t
                                              INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
                                     WHERE t.name = @tableName
                                       and (s.name = @tableSchema or @tableSchema IS NULL))
        """;


    private readonly string _foreignKeysMetadataSqlQuery =
        """
        SELECT fk.name                                                                    AS name,
               SCHEMA_NAME(tp.schema_id)                                                  AS [schema],
               tp.name                                                                    AS table_name,
               STRING_AGG(cp.name, ', ') WITHIN GROUP (ORDER BY fkc.constraint_column_id) AS column_names,
               SCHEMA_NAME(tr.schema_id)                                                  AS referenced_schema,
               tr.name                                                                    AS referenced_table,
               STRING_AGG(cr.name, ', ') WITHIN GROUP (ORDER BY fkc.constraint_column_id) AS referenced_column_names
        FROM sys.foreign_keys AS fk
                 JOIN
             sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
                 JOIN
             sys.tables AS tp ON fkc.parent_object_id = tp.object_id
                 JOIN
             sys.columns AS cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
                 JOIN
             sys.tables AS tr ON fkc.referenced_object_id = tr.object_id
                 JOIN
             sys.columns AS cr ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id
        WHERE (SCHEMA_NAME(tp.schema_id) = @tableSchema OR @tableSchema IS NULL)
          AND tp.name = @tableName
        GROUP BY fk.name, tp.schema_id, tp.name, tr.schema_id, tr.name;
        """;

    #endregion Metadata SQL Queries
}
