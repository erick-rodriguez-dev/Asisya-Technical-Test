using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace asisya.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuantityPerUnit_ToShort : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"Products\" SET \"QuantityPerUnit\" = NULL;");
            migrationBuilder.Sql("ALTER TABLE \"Products\" ALTER COLUMN \"QuantityPerUnit\" TYPE smallint USING \"QuantityPerUnit\"::smallint;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QuantityPerUnit",
                table: "Products",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
