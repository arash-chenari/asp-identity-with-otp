using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspIdentityWithOtp.Mvc.Migrations
{
    public partial class TempUserRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GeneratedDateTime",
                table: "UserVerificationCodes",
                newName: "GeneratedCodeDateTime");

            migrationBuilder.CreateTable(
                name: "TempUserRegistrations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedCodeDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempUserRegistrations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempUserRegistrations");

            migrationBuilder.RenameColumn(
                name: "GeneratedCodeDateTime",
                table: "UserVerificationCodes",
                newName: "GeneratedDateTime");
        }
    }
}
