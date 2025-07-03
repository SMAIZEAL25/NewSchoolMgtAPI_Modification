using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace New_School_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class UploadImagerefferencetoStudentrecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_StudentRecords_identityUser",
                table: "Uploads");

            migrationBuilder.RenameColumn(
                name: "identityUser",
                table: "Uploads",
                newName: "StudentRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_Uploads_identityUser",
                table: "Uploads",
                newName: "IX_Uploads_StudentRecordId");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Uploads",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileExtension",
                table: "Uploads",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileDescription",
                table: "Uploads",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_StudentRecords_StudentRecordId",
                table: "Uploads",
                column: "StudentRecordId",
                principalTable: "StudentRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_StudentRecords_StudentRecordId",
                table: "Uploads");

            migrationBuilder.RenameColumn(
                name: "StudentRecordId",
                table: "Uploads",
                newName: "identityUser");

            migrationBuilder.RenameIndex(
                name: "IX_Uploads_StudentRecordId",
                table: "Uploads",
                newName: "IX_Uploads_identityUser");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FileExtension",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "FileDescription",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_StudentRecords_identityUser",
                table: "Uploads",
                column: "identityUser",
                principalTable: "StudentRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
