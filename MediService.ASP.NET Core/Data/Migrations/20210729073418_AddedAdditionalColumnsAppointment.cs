using Microsoft.EntityFrameworkCore.Migrations;

namespace MediService.ASP.NET_Core.Data.Migrations
{
    public partial class AddedAdditionalColumnsAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfo",
                table: "Appointments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SpecialistId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SpecialistId",
                table: "Appointments",
                column: "SpecialistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Specialists_SpecialistId",
                table: "Appointments",
                column: "SpecialistId",
                principalTable: "Specialists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Specialists_SpecialistId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_SpecialistId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AdditionalInfo",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "SpecialistId",
                table: "Appointments");
        }
    }
}
