using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendApi.Migrations
{
    /// <inheritdoc />
    public partial class addsubjectpersemester : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcademicPeriodId",
                table: "StudentSubjects",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjects_AcademicPeriodId",
                table: "StudentSubjects",
                column: "AcademicPeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_AcademicPeriods_AcademicPeriodId",
                table: "StudentSubjects",
                column: "AcademicPeriodId",
                principalTable: "AcademicPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_AcademicPeriods_AcademicPeriodId",
                table: "StudentSubjects");

            migrationBuilder.DropIndex(
                name: "IX_StudentSubjects_AcademicPeriodId",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "AcademicPeriodId",
                table: "StudentSubjects");
        }
    }
}
