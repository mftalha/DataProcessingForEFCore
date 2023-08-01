using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Functions.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                Create Function getPersonTotalOrderPrice(@personId INT)
	                RETURNS INT
                AS
                BEGIN
	                Declare  @totalPrice Int
	                select @totalPrice = Sum(o.Price) from Persons p
	                join Orders o
		                ON p.PersonId = o.PersonId 
	                where p.PersonId = @personId
	                RETURN @totalPrice
                END
                ");
        }

        /// <inheritdoc => bu functionu gerektiğinde silebilmek için. => mig_1e dönüş yaptığımda kaldıracaktır bu function'u />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"Drop Function getPersonTotalOrderPrice");
        }
    }
}

