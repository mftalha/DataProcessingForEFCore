using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Views.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                Create VIEW vm_PersonOrders
                As
	                select Top 100 p.Name, Count(*) [Count] from persons p
	                inner join Orders o
		                ON p.PersonId = o.PersonId
	                group by p.Name
	                Order by [Count] Desc
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"DROP VIEW vm_PersonOrders");
        }
    }
}
