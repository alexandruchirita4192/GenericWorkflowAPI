using Microsoft.EntityFrameworkCore.Migrations;

namespace GenericWorkflowAPI.Database.Migrations
{
    public partial class AddCodeToAllEditedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "WorkflowTransitions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "WorkflowStateInputCodeTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "WorkflowInstanceInputCodes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "WorkflowTransitions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "WorkflowStateInputCodeTypes");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "WorkflowInstanceInputCodes");
        }
    }
}
