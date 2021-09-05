using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebChat.Migrations
{
    public partial class updatedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(200)", maxLength: 200, nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(200)", maxLength: 200, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppMessage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Messages = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SenderAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReciverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppMessage_AppUser_ReciverId",
                        column: x => x.ReciverId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppMessage_AppUser_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppMessage_ReciverId",
                table: "AppMessage",
                column: "ReciverId");

            migrationBuilder.CreateIndex(
                name: "IX_AppMessage_SenderId",
                table: "AppMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_Username",
                table: "AppUser",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppMessage");

            migrationBuilder.DropTable(
                name: "AppUser");
        }
    }
}
