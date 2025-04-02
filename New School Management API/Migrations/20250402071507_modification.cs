using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace New_School_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class modification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentApproval",
                table: "CourseRegistrations");

            migrationBuilder.RenameColumn(
                name: "RegistrationId",
                table: "CourseRegistrations",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "StudentRecords",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CourseRegistrations",
                newName: "RegistrationId");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "StudentRecords",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "DepartmentApproval",
                table: "CourseRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
