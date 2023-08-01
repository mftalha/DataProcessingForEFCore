using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stored_Procedures.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
            Create procedure sp_PersonOrders
            As
	            select p.Name, Count(*) [Count] from Persons p
	            Join Orders o
		            ON p.PersonId = o.PersonId
	            Group By p.Name
	            Order by Count(*) Desc
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Drop Procedure sp_PersonOrders");
        }
    }
}
