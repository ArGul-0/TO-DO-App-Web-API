using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoApp.Infrastructure.Migrations;

/// <inheritdoc />
public partial class _20261117161542_AddIndexForTag : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Tags_OwnerId",
            table: "Tags");

        migrationBuilder.CreateIndex(
            name: "IX_Tags_OwnerId_Name",
            table: "Tags",
            columns: new[] { "OwnerId", "Name" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Tags_OwnerId_Name",
            table: "Tags");

        migrationBuilder.CreateIndex(
            name: "IX_Tags_OwnerId",
            table: "Tags",
            column: "OwnerId");
    }
}
