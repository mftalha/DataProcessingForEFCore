using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stored_Procedures.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
            Create procedure sp_bestSellingStaff
            As
	            DECLARE @name NVARCHAR(Max), @count Int
	            select Top 1 @name = p.Name, @count = Count(*) from Persons p
	            Join Orders o
		            ON p.PersonId = o.PersonId
	            Group By p.Name
	            Order by Count(*) Desc
	            RETURN @count
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"Drop procedure sp_bestSellingStaff");
        }
    }
}
