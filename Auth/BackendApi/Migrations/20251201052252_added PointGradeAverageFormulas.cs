using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendApi.Migrations
{
    /// <inheritdoc />
    public partial class addedPointGradeAverageFormulas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PointGradeAverageFormulas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeFormulaId = table.Column<int>(type: "int", nullable: false),
                    PercentageMultiplier = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BasePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointGradeAverageFormulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointGradeAverageFormulas_GradeFormulas_GradeFormulaId",
                        column: x => x.GradeFormulaId,
                        principalTable: "GradeFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointGradeAverageFormulas_GradeFormulaId",
                table: "PointGradeAverageFormulas",
                column: "GradeFormulaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointGradeAverageFormulas");
        }
    }
}
