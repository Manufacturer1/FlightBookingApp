using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedBookingRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Flights_FlightNumber",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FlightNumber",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FlightNumber",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "ItineraryId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ItineraryId",
                table: "Bookings",
                column: "ItineraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Itineraries_ItineraryId",
                table: "Bookings",
                column: "ItineraryId",
                principalTable: "Itineraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Itineraries_ItineraryId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ItineraryId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ItineraryId",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "FlightNumber",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FlightNumber",
                table: "Bookings",
                column: "FlightNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Flights_FlightNumber",
                table: "Bookings",
                column: "FlightNumber",
                principalTable: "Flights",
                principalColumn: "FlightNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
