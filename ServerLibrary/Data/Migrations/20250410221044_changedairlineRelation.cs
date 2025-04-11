using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class changedairlineRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Airlines_BaggagePolicyId",
                table: "Airlines");

            migrationBuilder.AlterColumn<string>(
                name: "ClassType",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "BaggagePolicyId",
                table: "Airlines",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_BaggagePolicyId",
                table: "Airlines",
                column: "BaggagePolicyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Airlines_BaggagePolicyId",
                table: "Airlines");

            migrationBuilder.AlterColumn<string>(
                name: "ClassType",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BaggagePolicyId",
                table: "Airlines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_BaggagePolicyId",
                table: "Airlines",
                column: "BaggagePolicyId",
                unique: true,
                filter: "[BaggagePolicyId] IS NOT NULL");
        }
    }
}
