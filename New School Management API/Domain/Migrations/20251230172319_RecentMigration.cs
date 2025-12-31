using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace New_School_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class RecentMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SurName",
                table: "StudentRecords",
                newName: "FirsName");

            migrationBuilder.RenameColumn(
                name: "Sex",
                table: "StudentRecords",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "CourseRgistration_Id",
                table: "StudentRecords",
                newName: "CourseRegistration_Id");

            migrationBuilder.AddColumn<int>(
                name: "Credit",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Credit",
                table: "CourseRegistrations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Credit",
                table: "CourseRegistrations");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "StudentRecords",
                newName: "Sex");

            migrationBuilder.RenameColumn(
                name: "FirsName",
                table: "StudentRecords",
                newName: "SurName");

            migrationBuilder.RenameColumn(
                name: "CourseRegistration_Id",
                table: "StudentRecords",
                newName: "CourseRgistration_Id");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FeeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VAT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_StudentRecords_StudentId",
                        column: x => x.StudentId,
                        principalTable: "StudentRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StudentId",
                table: "Transactions",
                column: "StudentId");
        }
    }
}
