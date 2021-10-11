using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeuEstoque.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DBItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: true),
                    Quantity = table.Column<long>(type: "INTEGER", nullable: true),
                    ProductId = table.Column<string>(type: "TEXT", nullable: true),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Product_Price = table.Column<decimal>(type: "TEXT", nullable: true),
                    Product_Quantity = table.Column<long>(type: "INTEGER", nullable: true),
                    Product_OwnerId = table.Column<string>(type: "TEXT", nullable: true),
                    User_Name = table.Column<string>(type: "TEXT", nullable: true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DBItem_DBItem_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "DBItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DBItem_DBItem_Product_OwnerId",
                        column: x => x.Product_OwnerId,
                        principalTable: "DBItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DBItem_DBItem_ProductId",
                        column: x => x.ProductId,
                        principalTable: "DBItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DBItem_Email",
                table: "DBItem",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DBItem_OwnerId",
                table: "DBItem",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DBItem_Product_OwnerId",
                table: "DBItem",
                column: "Product_OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DBItem_ProductId",
                table: "DBItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DBItem_Username",
                table: "DBItem",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DBItem");
        }
    }
}
