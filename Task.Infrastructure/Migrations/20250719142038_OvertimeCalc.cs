using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OvertimeCalc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "OverTimeCalc",
                table: "EmployeePayrolls",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverTimeCalc",
                table: "EmployeePayrolls");
        }
    }
}
