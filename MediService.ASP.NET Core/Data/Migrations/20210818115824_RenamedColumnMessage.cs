using Microsoft.EntityFrameworkCore.Migrations;

namespace MediService.ASP.NET_Core.Data.Migrations
{
    public partial class RenamedColumnMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Seen",
                table: "Messages",
                newName: "IsSeen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSeen",
                table: "Messages",
                newName: "Seen");
        }
    }
}
