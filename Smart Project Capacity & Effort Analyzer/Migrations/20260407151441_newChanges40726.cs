using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Project_Capacity___Effort_Analyzer.Migrations
{
    /// <inheritdoc />
    public partial class newChanges40726 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Workspace",
                table: "NotesMasters",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Workspace",
                table: "NotesMasters");
        }
    }
}
