using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "EmployeePayrolls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayrolls_EmployeeId",
                table: "EmployeePayrolls",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeePayrolls_Employee_EmployeeId",
                table: "EmployeePayrolls",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeePayrolls_Employee_EmployeeId",
                table: "EmployeePayrolls");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_EmployeePayrolls_EmployeeId",
                table: "EmployeePayrolls");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "EmployeePayrolls");
        }
    }
}
