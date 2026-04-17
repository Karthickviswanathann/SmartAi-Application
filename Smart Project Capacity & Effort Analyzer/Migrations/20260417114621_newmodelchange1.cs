using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Project_Capacity___Effort_Analyzer.Migrations
{
    /// <inheritdoc />
    public partial class newmodelchange1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPinnned",
                table: "NotesMasters",
                newName: "IsPinned");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPinned",
                table: "NotesMasters",
                newName: "IsPinnned");
        }
    }
}
