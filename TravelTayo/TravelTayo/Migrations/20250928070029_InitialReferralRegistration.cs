using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelTayo.Migrations
{
    public partial class InitialReferralRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReferralRegistrations",
                columns: table => new
                {
                    ReferralRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GCashNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GCashQRCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferralRegistrations", x => x.ReferralRegistrationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferralRegistrations");
        }
    }
}
