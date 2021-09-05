using Microsoft.EntityFrameworkCore.Migrations;

namespace WebChat.Migrations
{
    public partial class capnhapdatabase05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderAt",
                table: "AppMessage",
                newName: "SendAt");

            migrationBuilder.RenameColumn(
                name: "Messages",
                table: "AppMessage",
                newName: "Message");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SendAt",
                table: "AppMessage",
                newName: "SenderAt");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "AppMessage",
                newName: "Messages");
        }
    }
}
