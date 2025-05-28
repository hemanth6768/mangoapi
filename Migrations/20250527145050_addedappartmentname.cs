using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangoApi.Migrations
{
    /// <inheritdoc />
    public partial class addedappartmentname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerAppartmentName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerAppartmentName",
                table: "Orders");
        }
    }
}
