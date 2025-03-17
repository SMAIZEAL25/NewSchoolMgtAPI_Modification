using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace New_School_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class Secondmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_StudentRecords_StudentId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_StudentId",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "UploadedOn",
                table: "Uploads");

            migrationBuilder.RenameColumn(
                name: "filePath",
                table: "Uploads",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "fileExtension",
                table: "Uploads",
                newName: "FileExtension");

            migrationBuilder.RenameColumn(
                name: "UploadfileId",
                table: "Uploads",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "FileDescription",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddColumn<int>(
                name: "StudentRecordId",
                table: "Uploads",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_StudentRecordId",
                table: "Uploads",
                column: "StudentRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_StudentRecords_StudentRecordId",
                table: "Uploads",
                column: "StudentRecordId",
                principalTable: "StudentRecords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_StudentRecords_StudentRecordId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_StudentRecordId",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "StudentRecordId",
                table: "Uploads");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Uploads",
                newName: "filePath");

            migrationBuilder.RenameColumn(
                name: "FileExtension",
                table: "Uploads",
                newName: "fileExtension");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Uploads",
                newName: "UploadfileId");

            migrationBuilder.AlterColumn<byte[]>(
                name: "FileDescription",
                table: "Uploads",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Uploads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedOn",
                table: "Uploads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_StudentId",
                table: "Uploads",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_StudentRecords_StudentId",
                table: "Uploads",
                column: "StudentId",
                principalTable: "StudentRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
