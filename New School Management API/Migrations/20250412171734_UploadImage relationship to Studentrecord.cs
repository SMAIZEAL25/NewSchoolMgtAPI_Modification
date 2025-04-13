using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace New_School_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class UploadImagerelationshiptoStudentrecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_StudentRecords_StudentRecordId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_StudentRecordId",
                table: "Uploads");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Uploads",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_StudentRecords_Id",
                table: "Uploads",
                column: "Id",
                principalTable: "StudentRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_StudentRecords_Id",
                table: "Uploads");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Uploads",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_StudentRecordId",
                table: "Uploads",
                column: "StudentRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_StudentRecords_StudentRecordId",
                table: "Uploads",
                column: "StudentRecordId",
                principalTable: "StudentRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
