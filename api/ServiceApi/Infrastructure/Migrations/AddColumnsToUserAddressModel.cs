using Domain.User;
using FluentMigrator;

namespace Infrastructure.Migrations;

[Migration(202601042045)]
public class AddColumnsToUserAddressModel : Migration
{
    private readonly string _tableName = nameof(UserAddressModel);
    private readonly string _floorColumnName = nameof(UserAddressModel.Floor);
    private readonly string _entranceColumnName = nameof(UserAddressModel.Entrance);
    
    /// <inheritdoc/>
    public override void Up()
    {
        if (Schema.Table(_tableName).Exists())
        {
            Create.Column(_floorColumnName).OnTable(_tableName).AsInt32().Nullable();
            Create.Column(_entranceColumnName).OnTable(_tableName).AsString(10).Nullable();
        }
    }

    /// <inheritdoc/>
    public override void Down()
    {
        if (Schema.Table(_tableName).Exists())
        {
            Delete.Column(_floorColumnName).FromTable(_tableName);
            Delete.Column(_entranceColumnName).FromTable(_tableName);
        }
    }
}