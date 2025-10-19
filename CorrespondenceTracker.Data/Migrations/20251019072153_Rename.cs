using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorrespondenceTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Correspondences_FileRecords_MainFileId",
                table: "Correspondences");

            migrationBuilder.RenameColumn(
                name: "RelativePath",
                table: "FileRecords",
                newName: "FullPath");

            migrationBuilder.RenameColumn(
                name: "MainFileId",
                table: "Correspondences",
                newName: "FileId");

            migrationBuilder.RenameIndex(
                name: "IX_Correspondences_MainFileId",
                table: "Correspondences",
                newName: "IX_Correspondences_FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Correspondences_FileRecords_FileId",
                table: "Correspondences",
                column: "FileId",
                principalTable: "FileRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Correspondences_FileRecords_FileId",
                table: "Correspondences");

            migrationBuilder.RenameColumn(
                name: "FullPath",
                table: "FileRecords",
                newName: "RelativePath");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Correspondences",
                newName: "MainFileId");

            migrationBuilder.RenameIndex(
                name: "IX_Correspondences_FileId",
                table: "Correspondences",
                newName: "IX_Correspondences_MainFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Correspondences_FileRecords_MainFileId",
                table: "Correspondences",
                column: "MainFileId",
                principalTable: "FileRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
