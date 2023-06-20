using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tracking.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Books",
                newName: "BookName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Authors",
                newName: "AuthorName");

            migrationBuilder.AddColumn<int>(
                name: "PageNumber",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageNumber",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "BookName",
                table: "Books",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "Authors",
                newName: "Name");
        }
    }
}
