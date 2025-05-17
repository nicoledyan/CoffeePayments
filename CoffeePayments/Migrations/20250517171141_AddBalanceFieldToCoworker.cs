using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeePaymentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddBalanceFieldToCoworker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Coworkers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Coworkers");
        }
    }
}
