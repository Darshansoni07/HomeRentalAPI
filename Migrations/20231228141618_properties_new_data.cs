using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeRent.Migrations
{
    public partial class properties_new_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "HReProperties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ac",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "agreement",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "avilablefor",
                table: "HReProperties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "badroom",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "balcony",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "bed",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "dining",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "exhaustfan",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fan",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "floor",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fridge",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "kitchen",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "light",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "parking",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sofa",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "stove",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "tv",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "wardrobe",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "washingMachine",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "waterpurifier",
                table: "HReProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "ac",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "agreement",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "avilablefor",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "badroom",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "balcony",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "bed",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "dining",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "exhaustfan",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "fan",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "floor",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "fridge",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "kitchen",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "light",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "parking",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "sofa",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "stove",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "tv",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "wardrobe",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "washingMachine",
                table: "HReProperties");

            migrationBuilder.DropColumn(
                name: "waterpurifier",
                table: "HReProperties");
        }
    }
}
