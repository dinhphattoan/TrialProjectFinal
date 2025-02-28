using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class GlossaryForeignKeyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Glossaries_AspNetUsers_UserCreatedById",
                table: "Glossaries");

            migrationBuilder.RenameColumn(
                name: "UserCreatedById",
                table: "Glossaries",
                newName: "CreateById");

            migrationBuilder.RenameIndex(
                name: "IX_Glossaries_UserCreatedById",
                table: "Glossaries",
                newName: "IX_Glossaries_CreateById");

            migrationBuilder.AddForeignKey(
                name: "FK_Glossaries_AspNetUsers_CreateById",
                table: "Glossaries",
                column: "CreateById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Glossaries_AspNetUsers_CreateById",
                table: "Glossaries");

            migrationBuilder.RenameColumn(
                name: "CreateById",
                table: "Glossaries",
                newName: "UserCreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Glossaries_CreateById",
                table: "Glossaries",
                newName: "IX_Glossaries_UserCreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Glossaries_AspNetUsers_UserCreatedById",
                table: "Glossaries",
                column: "UserCreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
