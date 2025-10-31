using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorrespondenceTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class ResponsibleUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Correspondences_Users_AssignedUserId",
                table: "Correspondences");

            migrationBuilder.RenameColumn(
                name: "AssignedUserId",
                table: "Correspondences",
                newName: "ResponsibleUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Correspondences_AssignedUserId",
                table: "Correspondences",
                newName: "IX_Correspondences_ResponsibleUserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsFollowUpManager",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFollowUpUser",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ReminderId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Correspondents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FinalAction",
                table: "Correspondences",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FollowUpUserId",
                table: "Correspondences",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReminderId",
                table: "Users",
                column: "ReminderId");

            migrationBuilder.CreateIndex(
                name: "IX_Correspondences_FollowUpUserId",
                table: "Correspondences",
                column: "FollowUpUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Correspondences_Users_FollowUpUserId",
                table: "Correspondences",
                column: "FollowUpUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Correspondences_Users_ResponsibleUserId",
                table: "Correspondences",
                column: "ResponsibleUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Reminders_ReminderId",
                table: "Users",
                column: "ReminderId",
                principalTable: "Reminders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Correspondences_Users_FollowUpUserId",
                table: "Correspondences");

            migrationBuilder.DropForeignKey(
                name: "FK_Correspondences_Users_ResponsibleUserId",
                table: "Correspondences");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Reminders_ReminderId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ReminderId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Correspondences_FollowUpUserId",
                table: "Correspondences");

            migrationBuilder.DropColumn(
                name: "IsFollowUpManager",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsFollowUpUser",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReminderId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Correspondents");

            migrationBuilder.DropColumn(
                name: "FinalAction",
                table: "Correspondences");

            migrationBuilder.DropColumn(
                name: "FollowUpUserId",
                table: "Correspondences");

            migrationBuilder.RenameColumn(
                name: "ResponsibleUserId",
                table: "Correspondences",
                newName: "AssignedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Correspondences_ResponsibleUserId",
                table: "Correspondences",
                newName: "IX_Correspondences_AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Correspondences_Users_AssignedUserId",
                table: "Correspondences",
                column: "AssignedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
