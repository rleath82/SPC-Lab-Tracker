using Microsoft.EntityFrameworkCore.Migrations;

namespace LabTracker.Migrations
{
    public partial class LabTrackerMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabEnrollment_Course_CourseID",
                table: "LabEnrollment");

            migrationBuilder.DropIndex(
                name: "IX_LabEnrollment_CourseID",
                table: "LabEnrollment");

            migrationBuilder.DropColumn(
                name: "CourseID",
                table: "LabEnrollment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseID",
                table: "LabEnrollment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LabEnrollment_CourseID",
                table: "LabEnrollment",
                column: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_LabEnrollment_Course_CourseID",
                table: "LabEnrollment",
                column: "CourseID",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
