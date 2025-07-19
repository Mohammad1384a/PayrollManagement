using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class patch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "EmployeePayrolls");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "EmployeePayrolls");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "EmployeePayrolls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "EmployeePayrolls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
