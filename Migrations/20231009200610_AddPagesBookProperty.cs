using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoCSplitQueries.Migrations
{
    /// <inheritdoc />
    public partial class AddPagesBookProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookId",
                table: "Pages",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_BookId",
                table: "Pages",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_Books_BookId",
                table: "Pages",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Books_BookId",
                table: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Pages_BookId",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Pages");
        }
    }
}
