using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeRent.Migrations
{
    public partial class UpdateUserForPasswardEncr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Password", "HReUsers");
            migrationBuilder.AddColumn<byte[]>(
                name: "Password",
                table: "HReUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue:"Darshan"
                //oldClrType: typeof(string),
                //oldType: "nvarchar(max)"
                );

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordKey",
                table: "HReUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordKey",
                table: "HReUsers");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "HReUsers",
                type: "nvarchar(max)",
                nullable: false
                //oldClrType: typeof(byte[]),
                //oldType: "varbinary(max)"
                );
        }
    }
}
