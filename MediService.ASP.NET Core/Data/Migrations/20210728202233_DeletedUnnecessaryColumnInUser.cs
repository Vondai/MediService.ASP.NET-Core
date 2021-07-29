using Microsoft.EntityFrameworkCore.Migrations;

namespace MediService.ASP.NET_Core.Data.Migrations
{
    public partial class DeletedUnnecessaryColumnInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specialists_AspNetUsers_UserId",
                table: "Specialists");

            migrationBuilder.DropColumn(
                name: "IsSpecialist",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpecialistId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialists_AspNetUsers_UserId",
                table: "Specialists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specialists_AspNetUsers_UserId",
                table: "Specialists");

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecialist",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SpecialistId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialists_AspNetUsers_UserId",
                table: "Specialists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
