using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendApi.Migrations
{
    /// <inheritdoc />
    public partial class GradeFormulaperyearlevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GradeFormulas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicPeriodId = table.Column<int>(type: "int", nullable: true),
                    YearLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeFormulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeFormulas_AcademicPeriods_AcademicPeriodId",
                        column: x => x.AcademicPeriodId,
                        principalTable: "AcademicPeriods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GradeFormulas_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GradeFormulaItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeFormulaId = table.Column<int>(type: "int", nullable: false),
                    ComponentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsSubjectSpecific = table.Column<bool>(type: "bit", nullable: false),
                    SubjectType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeFormulaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeFormulaItems_GradeFormulas_GradeFormulaId",
                        column: x => x.GradeFormulaId,
                        principalTable: "GradeFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradeFormulaItems_GradeFormulaId",
                table: "GradeFormulaItems",
                column: "GradeFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeFormulas_AcademicPeriodId",
                table: "GradeFormulas",
                column: "AcademicPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeFormulas_SubjectId",
                table: "GradeFormulas",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradeFormulaItems");

            migrationBuilder.DropTable(
                name: "GradeFormulas");
        }
    }
}
