using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedmanyToManyBetweenAmenityAndFlights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Amenities_FlightAmenityId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_FlightAmenityId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FlightAmenityId",
                table: "Flights");

            migrationBuilder.CreateTable(
                name: "FlightAmenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AmenityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightAmenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightAmenities_Amenities_AmenityId",
                        column: x => x.AmenityId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightAmenities_Flights_FlightNumber",
                        column: x => x.FlightNumber,
                        principalTable: "Flights",
                        principalColumn: "FlightNumber");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlightAmenities_AmenityId",
                table: "FlightAmenities",
                column: "AmenityId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightAmenities_FlightNumber",
                table: "FlightAmenities",
                column: "FlightNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightAmenities");

            migrationBuilder.AddColumn<int>(
                name: "FlightAmenityId",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightAmenityId",
                table: "Flights",
                column: "FlightAmenityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Amenities_FlightAmenityId",
                table: "Flights",
                column: "FlightAmenityId",
                principalTable: "Amenities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
