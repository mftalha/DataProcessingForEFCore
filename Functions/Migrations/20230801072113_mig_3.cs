using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Functions.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                Create Function bestSellingStaff(@totalOrderPrice INT = 0)
	                Returns Table 
                As
                Return 
                select top 1 p.Name, Count(*) OrderCount, Sum(o.Price) TotalOrderPrice from Persons p
                join Orders o
	                on p.PersonId = o.PersonId
                Group by p.Name
                Having Sum(o.Price) > @totalOrderPrice
                order by OrderCount desc 
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"Drop Function bestSellingStaff");
        }
    }
}
