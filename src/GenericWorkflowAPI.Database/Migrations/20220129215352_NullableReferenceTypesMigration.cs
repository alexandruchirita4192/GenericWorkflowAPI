using Microsoft.EntityFrameworkCore.Migrations;

namespace GenericWorkflowAPI.Database.Migrations
{
    public partial class NullableReferenceTypesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInputCodeTypes_AspNetUsers_ChangedByUserId",
                table: "WorkflowInputCodeTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstanceHistory_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstanceHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstanceHistoryInputCodes_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstanceHistoryInputCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstanceInputCodes_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstanceInputCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstances_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_Workflows_AspNetUsers_ChangedByUserId",
                table: "Workflows");

            migrationBuilder.DropForeignKey(
                name: "FK_Workflows_WorkflowTypes_TypeId",
                table: "Workflows");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStateInputCodeTypes_AspNetUsers_ChangedByUserId",
                table: "WorkflowStateInputCodeTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStates_AspNetUsers_ChangedByUserId",
                table: "WorkflowStates");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTransitions_AspNetRoles_RoleId",
                table: "WorkflowTransitions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTransitions_AspNetUsers_ChangedByUserId",
                table: "WorkflowTransitions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTypes_AspNetUsers_ChangedByUserId",
                table: "WorkflowTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkflowTypes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WorkflowTypes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowTypes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "RoleId",
                table: "WorkflowTransitions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowTransitions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkflowStates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WorkflowStates",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowStates",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowStateInputCodeTypes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "TypeId",
                table: "Workflows",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Workflows",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Workflows",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "Workflows",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowInstances",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowInstanceInputCodes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "WorkflowInstanceHistoryInputCodes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowInstanceHistoryInputCodes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowInstanceHistory",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkflowInputCodeTypes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WorkflowInputCodeTypes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowInputCodeTypes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInputCodeTypes_AspNetUsers_ChangedByUserId",
                table: "WorkflowInputCodeTypes",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstanceHistory_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstanceHistory",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstanceHistoryInputCodes_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstanceHistoryInputCodes",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstanceInputCodes_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstanceInputCodes",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstances_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstances",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workflows_AspNetUsers_ChangedByUserId",
                table: "Workflows",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workflows_WorkflowTypes_TypeId",
                table: "Workflows",
                column: "TypeId",
                principalTable: "WorkflowTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStateInputCodeTypes_AspNetUsers_ChangedByUserId",
                table: "WorkflowStateInputCodeTypes",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStates_AspNetUsers_ChangedByUserId",
                table: "WorkflowStates",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTransitions_AspNetRoles_RoleId",
                table: "WorkflowTransitions",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTransitions_AspNetUsers_ChangedByUserId",
                table: "WorkflowTransitions",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTypes_AspNetUsers_ChangedByUserId",
                table: "WorkflowTypes",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInputCodeTypes_AspNetUsers_ChangedByUserId",
                table: "WorkflowInputCodeTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstanceHistory_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstanceHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstanceHistoryInputCodes_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstanceHistoryInputCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstanceInputCodes_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstanceInputCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstances_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_Workflows_AspNetUsers_ChangedByUserId",
                table: "Workflows");

            migrationBuilder.DropForeignKey(
                name: "FK_Workflows_WorkflowTypes_TypeId",
                table: "Workflows");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStateInputCodeTypes_AspNetUsers_ChangedByUserId",
                table: "WorkflowStateInputCodeTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStates_AspNetUsers_ChangedByUserId",
                table: "WorkflowStates");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTransitions_AspNetRoles_RoleId",
                table: "WorkflowTransitions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTransitions_AspNetUsers_ChangedByUserId",
                table: "WorkflowTransitions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTypes_AspNetUsers_ChangedByUserId",
                table: "WorkflowTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkflowTypes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WorkflowTypes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RoleId",
                table: "WorkflowTransitions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowTransitions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkflowStates",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WorkflowStates",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowStates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowStateInputCodeTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TypeId",
                table: "Workflows",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Workflows",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Workflows",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "Workflows",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowInstances",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowInstanceInputCodes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "WorkflowInstanceHistoryInputCodes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowInstanceHistoryInputCodes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowInstanceHistory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkflowInputCodeTypes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WorkflowInputCodeTypes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ChangedByUserId",
                table: "WorkflowInputCodeTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInputCodeTypes_AspNetUsers_ChangedByUserId",
                table: "WorkflowInputCodeTypes",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstanceHistory_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstanceHistory",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstanceHistoryInputCodes_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstanceHistoryInputCodes",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstanceInputCodes_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstanceInputCodes",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstances_AspNetUsers_ChangedByUserId",
                table: "WorkflowInstances",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workflows_AspNetUsers_ChangedByUserId",
                table: "Workflows",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workflows_WorkflowTypes_TypeId",
                table: "Workflows",
                column: "TypeId",
                principalTable: "WorkflowTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStateInputCodeTypes_AspNetUsers_ChangedByUserId",
                table: "WorkflowStateInputCodeTypes",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStates_AspNetUsers_ChangedByUserId",
                table: "WorkflowStates",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTransitions_AspNetRoles_RoleId",
                table: "WorkflowTransitions",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTransitions_AspNetUsers_ChangedByUserId",
                table: "WorkflowTransitions",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTypes_AspNetUsers_ChangedByUserId",
                table: "WorkflowTypes",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
