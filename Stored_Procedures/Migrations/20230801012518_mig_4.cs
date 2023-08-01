using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stored_Procedures.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
            Create procedure sp_PersonOrders2
            (
	            @PersonId INT,
	            @Name NVARCHAR(Max) Output
            )
            As
	        select @Name = p.Name from Persons p
	        Join Orders o
		        ON p.PersonId = o.PersonId
	        WHERE p.PersonId = @PersonId
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"Drop Procedure sp_PersonOrders2");
        }
    }
}
