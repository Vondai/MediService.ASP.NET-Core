using Microsoft.EntityFrameworkCore.Migrations;

namespace MediService.ASP.NET_Core.Data.Migrations
{
    public partial class AddedColumnInService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFree",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFree",
                table: "Services");
        }
    }
}
