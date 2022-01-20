using Microsoft.EntityFrameworkCore.Migrations;

namespace GenericWorkflowAPI.Database.Migrations
{
    public partial class ChangesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstanceHistoryInputCodes_WorkflowInstances_InstanceId",
                table: "WorkflowInstanceHistoryInputCodes");

            migrationBuilder.RenameColumn(
                name: "InstanceId",
                table: "WorkflowInstanceHistoryInputCodes",
                newName: "HistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowInstanceHistoryInputCodes_InstanceId",
                table: "WorkflowInstanceHistoryInputCodes",
                newName: "IX_WorkflowInstanceHistoryInputCodes_HistoryId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "IdentityUserClaim<string>",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserClaim<string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityUserClaim<string>_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim<string>_IdentityUserId",
                table: "IdentityUserClaim<string>",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstanceHistoryInputCodes_WorkflowInstanceHistory_HistoryId",
                table: "WorkflowInstanceHistoryInputCodes",
                column: "HistoryId",
                principalTable: "WorkflowInstanceHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstanceHistoryInputCodes_WorkflowInstanceHistory_HistoryId",
                table: "WorkflowInstanceHistoryInputCodes");

            migrationBuilder.DropTable(
                name: "IdentityUserClaim<string>");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "HistoryId",
                table: "WorkflowInstanceHistoryInputCodes",
                newName: "InstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowInstanceHistoryInputCodes_HistoryId",
                table: "WorkflowInstanceHistoryInputCodes",
                newName: "IX_WorkflowInstanceHistoryInputCodes_InstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstanceHistoryInputCodes_WorkflowInstances_InstanceId",
                table: "WorkflowInstanceHistoryInputCodes",
                column: "InstanceId",
                principalTable: "WorkflowInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
