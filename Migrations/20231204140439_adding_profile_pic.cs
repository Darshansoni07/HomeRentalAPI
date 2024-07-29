using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeRent.Migrations
{
    public partial class adding_profile_pic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
                
            migrationBuilder.AddColumn<string>(
                name: "profile_Img",
                table: "HReUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profile_Img",
                table: "HReUsers");
            
        }
    }
}
