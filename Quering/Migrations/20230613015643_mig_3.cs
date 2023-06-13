using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quering.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPart_Parts_PartId",
                table: "ProductPart");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPart_Products_ProductId",
                table: "ProductPart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPart",
                table: "ProductPart");

            migrationBuilder.RenameTable(
                name: "ProductPart",
                newName: "ProductParts");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPart_PartId",
                table: "ProductParts",
                newName: "IX_ProductParts_PartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductParts",
                table: "ProductParts",
                columns: new[] { "ProductId", "PartId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductParts_Parts_PartId",
                table: "ProductParts",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductParts_Products_ProductId",
                table: "ProductParts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductParts_Parts_PartId",
                table: "ProductParts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductParts_Products_ProductId",
                table: "ProductParts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductParts",
                table: "ProductParts");

            migrationBuilder.RenameTable(
                name: "ProductParts",
                newName: "ProductPart");

            migrationBuilder.RenameIndex(
                name: "IX_ProductParts_PartId",
                table: "ProductPart",
                newName: "IX_ProductPart_PartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPart",
                table: "ProductPart",
                columns: new[] { "ProductId", "PartId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPart_Parts_PartId",
                table: "ProductPart",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPart_Products_ProductId",
                table: "ProductPart",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
