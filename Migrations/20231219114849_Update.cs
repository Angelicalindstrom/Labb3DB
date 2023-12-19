using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Labb3DB.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klasser", x => x.ClassID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "Professions",
                columns: table => new
                {
                    ProfessionTitleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfessionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roller", x => x.ProfessionTitleID);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elever", x => x.StudentID);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FKEmployeeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectID);
                    table.ForeignKey(
                        name: "FK_Subjects_Employees",
                        column: x => x.FKEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeProfessions",
                columns: table => new
                {
                    FKEmployeeID = table.Column<int>(type: "int", nullable: false),
                    FKProfessionTitleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_EmployeeProfessions_Employees",
                        column: x => x.FKEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_EmployeeProfessions_Professions",
                        column: x => x.FKProfessionTitleID,
                        principalTable: "Professions",
                        principalColumn: "ProfessionTitleID");
                });

            migrationBuilder.CreateTable(
                name: "ClassStudents",
                columns: table => new
                {
                    FKClassID = table.Column<int>(type: "int", nullable: false),
                    FKStudentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_ClassStudents_Classes",
                        column: x => x.FKClassID,
                        principalTable: "Classes",
                        principalColumn: "ClassID");
                    table.ForeignKey(
                        name: "FK_ClassStudents_Students",
                        column: x => x.FKStudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    EnrollmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FKStudentID = table.Column<int>(type: "int", nullable: false),
                    FKSubjectID = table.Column<int>(type: "int", nullable: false),
                    FKEmployeeID = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.EnrollmentID);
                    table.ForeignKey(
                        name: "FK_Enrollments_Employees",
                        column: x => x.FKEmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Enrollments_Students",
                        column: x => x.FKStudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK_Enrollments_Subjects",
                        column: x => x.FKSubjectID,
                        principalTable: "Subjects",
                        principalColumn: "SubjectID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudents_FKClassID",
                table: "ClassStudents",
                column: "FKClassID");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudents_FKStudentID",
                table: "ClassStudents",
                column: "FKStudentID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfessions_FKEmployeeID",
                table: "EmployeeProfessions",
                column: "FKEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfessions_FKProfessionTitleID",
                table: "EmployeeProfessions",
                column: "FKProfessionTitleID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_FKEmployeeID",
                table: "Enrollments",
                column: "FKEmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_FKStudentID",
                table: "Enrollments",
                column: "FKStudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_FKSubjectID",
                table: "Enrollments",
                column: "FKSubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_FKEmployeeID",
                table: "Subjects",
                column: "FKEmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassStudents");

            migrationBuilder.DropTable(
                name: "EmployeeProfessions");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Professions");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
