using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanThietBiDiDongDATN.Data.Migrations
{
    public partial class updateVoucherDBset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RatingProduct_products_productId",
                table: "RatingProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatingProduct",
                table: "RatingProduct");

            migrationBuilder.RenameTable(
                name: "RatingProduct",
                newName: "ratingProducts");

            migrationBuilder.RenameIndex(
                name: "IX_RatingProduct_productId",
                table: "ratingProducts",
                newName: "IX_ratingProducts_productId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ratingProducts",
                table: "ratingProducts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "vouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VoucherCode = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    BeginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifyBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vouchers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ratingProducts_products_productId",
                table: "ratingProducts",
                column: "productId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ratingProducts_products_productId",
                table: "ratingProducts");

            migrationBuilder.DropTable(
                name: "vouchers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ratingProducts",
                table: "ratingProducts");

            migrationBuilder.RenameTable(
                name: "ratingProducts",
                newName: "RatingProduct");

            migrationBuilder.RenameIndex(
                name: "IX_ratingProducts_productId",
                table: "RatingProduct",
                newName: "IX_RatingProduct_productId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatingProduct",
                table: "RatingProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RatingProduct_products_productId",
                table: "RatingProduct",
                column: "productId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
