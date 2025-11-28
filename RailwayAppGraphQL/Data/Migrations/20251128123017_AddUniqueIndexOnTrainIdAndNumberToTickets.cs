using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayAppGraphQL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexOnTrainIdAndNumberToTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TrainId_Number",
                table: "Tickets",
                columns: new[] { "TrainId", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_TrainId_Number",
                table: "Tickets");
        }
    }
}
