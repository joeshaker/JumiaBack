using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jumia_Api.Infrastructure.Context.Migrations
{
    /// <inheritdoc />
    public partial class productvarint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "variationId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_variationId",
                table: "OrderItems",
                column: "variationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductVariants_variationId",
                table: "OrderItems",
                column: "variationId",
                principalTable: "ProductVariants",
                principalColumn: "VariantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductVariants_variationId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_variationId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "variationId",
                table: "OrderItems");
        }
    }
}
