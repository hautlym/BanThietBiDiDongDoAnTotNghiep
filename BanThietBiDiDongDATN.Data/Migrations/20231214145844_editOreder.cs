using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanThietBiDiDongDATN.Data.Migrations
{
    public partial class editOreder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
                name: "shipStatus",
                table: "Orders");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "shipStatus",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

          
        }
    }
}
