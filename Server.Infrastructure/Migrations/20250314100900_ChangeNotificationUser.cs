using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNotificationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User_UserId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_UserId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notification");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_CreatedBy",
                table: "Notification",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User_CreatedBy",
                table: "Notification",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User_CreatedBy",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_CreatedBy",
                table: "Notification");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
