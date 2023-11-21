using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanThietBiDiDongDATN.Data.Migrations
{
    public partial class editUserAndCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BehindCamera",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Bluetooth",
                table: "products");

            migrationBuilder.DropColumn(
                name: "CPU",
                table: "products");

            migrationBuilder.DropColumn(
                name: "CapacityPin",
                table: "products");

            migrationBuilder.DropColumn(
                name: "ColorScreen",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Core",
                table: "products");

            migrationBuilder.DropColumn(
                name: "FrontCamera",
                table: "products");

            migrationBuilder.DropColumn(
                name: "GPS",
                table: "products");

            migrationBuilder.DropColumn(
                name: "GPU",
                table: "products");

            migrationBuilder.DropColumn(
                name: "HeadPhone",
                table: "products");

            migrationBuilder.DropColumn(
                name: "NumberSim",
                table: "products");

            migrationBuilder.DropColumn(
                name: "OtherConnect",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Ram",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Screen",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "products");

            migrationBuilder.DropColumn(
                name: "StandScreen",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Style",
                table: "products");

            migrationBuilder.DropColumn(
                name: "TouchTech",
                table: "products");

            migrationBuilder.DropColumn(
                name: "TypeCharge",
                table: "products");

            migrationBuilder.DropColumn(
                name: "TypePin",
                table: "products");

            migrationBuilder.DropColumn(
                name: "TypeSim",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Wifi",
                table: "products");

            migrationBuilder.AlterColumn<string>(
                name: "logo",
                table: "Categories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "AppUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSignIn",
                table: "AppUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "AppUser",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "LastSignIn",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "AppUser");

            migrationBuilder.AddColumn<string>(
                name: "BehindCamera",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Bluetooth",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CPU",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CapacityPin",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ColorScreen",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Core",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FrontCamera",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GPS",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GPU",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeadPhone",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumberSim",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OtherConnect",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ram",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Resolution",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Screen",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StandScreen",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TouchTech",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypeCharge",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypePin",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypeSim",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Width",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Wifi",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "logo",
                table: "Categories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
