using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeuEstoque.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Order_Price = table.Column<decimal>(type: "TEXT", nullable: true),
                    Order_Quantity = table.Column<long>(type: "INTEGER", nullable: true),
                    ProductId = table.Column<string>(type: "TEXT", nullable: true),
                    Order_OwnerId = table.Column<string>(type: "TEXT", nullable: true),
                    Product_Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: true),
                    Quantity = table.Column<long>(type: "INTEGER", nullable: true),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entity_Entity_Order_OwnerId",
                        column: x => x.Order_OwnerId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entity_Entity_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entity_Entity_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entity_Email",
                table: "Entity",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entity_Order_OwnerId",
                table: "Entity",
                column: "Order_OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity_OwnerId",
                table: "Entity",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity_ProductId",
                table: "Entity",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity_Username",
                table: "Entity",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entity");
        }
    }
}
