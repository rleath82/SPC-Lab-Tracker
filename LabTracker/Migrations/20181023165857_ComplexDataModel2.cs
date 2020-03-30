using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabTracker.Migrations
{
    public partial class ComplexDataModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.CreateTable(
                name: "CourseEnrollment",
                columns: table => new
                {
                    CourseEnrollmentID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentID = table.Column<int>(nullable: false),
                    CourseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEnrollment", x => x.CourseEnrollmentID);
                    table.ForeignKey(
                        name: "FK_CourseEnrollment_Course_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Course",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrollment_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabEnrollment",
                columns: table => new
                {
                    LabEnrollmentID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LabID = table.Column<int>(nullable: false),
                    StudentID = table.Column<int>(nullable: false),
                    LabSignIn = table.Column<DateTime>(nullable: true),
                    LabSignOut = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabEnrollment", x => x.LabEnrollmentID);
                    table.ForeignKey(
                        name: "FK_LabEnrollment_Lab_LabID",
                        column: x => x.LabID,
                        principalTable: "Lab",
                        principalColumn: "LabID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabEnrollment_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollment_CourseID",
                table: "CourseEnrollment",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollment_StudentID",
                table: "CourseEnrollment",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_LabEnrollment_LabID",
                table: "LabEnrollment",
                column: "LabID");

            migrationBuilder.CreateIndex(
                name: "IX_LabEnrollment_StudentID",
                table: "LabEnrollment",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseEnrollment");

            migrationBuilder.DropTable(
                name: "LabEnrollment");

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    EnrollmentID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseID = table.Column<int>(nullable: false),
                    LabID = table.Column<int>(nullable: false),
                    LabSignIn = table.Column<DateTime>(nullable: true),
                    LabSignOut = table.Column<DateTime>(nullable: true),
                    StudentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.EnrollmentID);
                    table.ForeignKey(
                        name: "FK_Enrollment_Course_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Course",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollment_Lab_LabID",
                        column: x => x.LabID,
                        principalTable: "Lab",
                        principalColumn: "LabID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollment_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseID",
                table: "Enrollment",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_LabID",
                table: "Enrollment",
                column: "LabID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_StudentID",
                table: "Enrollment",
                column: "StudentID");
        }
    }
}
