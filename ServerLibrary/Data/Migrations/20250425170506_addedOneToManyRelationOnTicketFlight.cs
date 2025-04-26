using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedOneToManyRelationOnTicketFlight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_FlightNumber",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FlightNumber",
                table: "Tickets",
                column: "FlightNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_FlightNumber",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FlightNumber",
                table: "Tickets",
                column: "FlightNumber",
                unique: true);
        }
    }
}
