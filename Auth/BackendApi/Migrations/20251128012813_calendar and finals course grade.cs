using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendApi.Migrations
{
    /// <inheritdoc />
    public partial class calendarandfinalscoursegrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FinalsSubmissionDate",
                table: "AcademicPeriods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalsSubmissionOpen",
                table: "AcademicPeriods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMidtermSubmissionOpen",
                table: "AcademicPeriods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "MidtermSubmissionDate",
                table: "AcademicPeriods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FinalCourseGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputedTotalMidtermGrade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoundedTotalMidtermGrade = table.Column<int>(type: "int", nullable: false),
                    ComputedTotalFinalGrade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoundedTotalFinalGrade = table.Column<int>(type: "int", nullable: false),
                    ComputedFinalCourseGrade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoundedFinalCourseGrade = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    GradePointEquivalent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalCourseGrades", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurriculumSubjects");

            migrationBuilder.DropTable(
                name: "FinalCourseGrades");

            migrationBuilder.DropColumn(
                name: "FinalsSubmissionDate",
                table: "AcademicPeriods");

            migrationBuilder.DropColumn(
                name: "IsFinalsSubmissionOpen",
                table: "AcademicPeriods");

            migrationBuilder.DropColumn(
                name: "IsMidtermSubmissionOpen",
                table: "AcademicPeriods");

            migrationBuilder.DropColumn(
                name: "MidtermSubmissionDate",
                table: "AcademicPeriods");
        }
    }
}
