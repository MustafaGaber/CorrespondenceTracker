using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorrespondenceTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseIndexs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reminders_Active_RemindTime",
                table: "Reminders",
                columns: new[] { "IsCompleted", "IsDismissed", "RemindTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Correspondences_Open_Priority_Date",
                table: "Correspondences",
                columns: new[] { "IsClosed", "PriorityLevel", "IncomingDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reminders_Active_RemindTime",
                table: "Reminders");

            migrationBuilder.DropIndex(
                name: "IX_Correspondences_Open_Priority_Date",
                table: "Correspondences");
        }
    }
}
