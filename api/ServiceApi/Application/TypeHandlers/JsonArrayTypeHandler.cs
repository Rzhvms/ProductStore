using System.Data;
using System.Text.Json.Nodes;
using Dapper;
using Npgsql;

namespace Application.TypeHandlers;

public class JsonArrayTypeHandler : SqlMapper.TypeHandler<JsonArray?>
{
    public override void SetValue(IDbDataParameter parameter, JsonArray? value)
    {
        parameter.Value = value?.ToJsonString() ?? (object)DBNull.Value;
        if (parameter is NpgsqlParameter npgsqlParameter)
        {
            npgsqlParameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb;
        }
    }

    public override JsonArray? Parse(object? value)
    {
        if (value == null || value is DBNull)
            return null;
            
        var jsonString = value.ToString();
        return JsonNode.Parse(jsonString) as JsonArray;
    }
}