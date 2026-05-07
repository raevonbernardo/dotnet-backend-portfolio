using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RTBackendAPI.Migrations.EmployeeDb
{
    /// <inheritdoc />
    public partial class InitialCreateEmployeeDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    PrivateId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Job = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    EmploymentDateTimestamp = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.PrivateId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
