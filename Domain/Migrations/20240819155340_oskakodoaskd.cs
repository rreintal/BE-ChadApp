using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class oskakodoaskd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRefreshToken_AspNetUsers_AppUserId",
                table: "AppRefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppRefreshToken",
                table: "AppRefreshToken");

            migrationBuilder.RenameTable(
                name: "AppRefreshToken",
                newName: "RefreshTokens");

            migrationBuilder.RenameIndex(
                name: "IX_AppRefreshToken_AppUserId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_AppUserId",
                table: "RefreshTokens",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_AppUserId",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "AppRefreshToken");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_AppUserId",
                table: "AppRefreshToken",
                newName: "IX_AppRefreshToken_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppRefreshToken",
                table: "AppRefreshToken",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRefreshToken_AspNetUsers_AppUserId",
                table: "AppRefreshToken",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
