using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeRent.Migrations
{
    public partial class add_agent_user_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "agent",
                table: "HReUsers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "agent",
                table: "HReUsers");
        }
    }
}
