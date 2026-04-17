using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Project_Capacity___Effort_Analyzer.Migrations
{
    /// <inheritdoc />
    public partial class Noteschanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPinnned",
                table: "NotesMasters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUrcheive",
                table: "NotesMasters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPinnned",
                table: "NotesMasters");

            migrationBuilder.DropColumn(
                name: "IsUrcheive",
                table: "NotesMasters");
        }
    }
}
