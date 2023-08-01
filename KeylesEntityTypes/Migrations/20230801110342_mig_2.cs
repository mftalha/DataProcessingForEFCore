using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeylessEntityTypes.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                Create View vw_PersonOrderCount
                AS
	                select p.Name, Count(*) [Count] from Persons p
	                join Orders o
	                  on p.PersonId = o.PersonId
	                Group by p.Name"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"DROP VİEW vw_PersonOrderCount");
        }
    }
}
