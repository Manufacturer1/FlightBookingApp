using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedRelationWithFlightsAndTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FlightNumber",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FlightNumber",
                table: "Tickets",
                column: "FlightNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Flights_FlightNumber",
                table: "Tickets",
                column: "FlightNumber",
                principalTable: "Flights",
                principalColumn: "FlightNumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Flights_FlightNumber",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_FlightNumber",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "FlightNumber",
                table: "Tickets");
        }
    }
}
