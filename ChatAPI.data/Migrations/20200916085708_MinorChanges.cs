using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatAPI.Data.Migrations
{
    public partial class MinorChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Passcode",
                table: "Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Passcode",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
