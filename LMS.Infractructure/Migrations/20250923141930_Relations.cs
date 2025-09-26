using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Companies.Infractructure.Migrations
{
    /// <inheritdoc />
    public partial class Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ActivityTypes_ActivityTypeID",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules");

            migrationBuilder.RenameColumn(
                name: "ActivityTypeID",
                table: "Activities",
                newName: "ActivityTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_ActivityTypeID",
                table: "Activities",
                newName: "IX_Activities_ActivityTypeId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourseId",
                table: "Modules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModuleId",
                table: "Activities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ModuleId",
                table: "Activities",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ActivityTypes_ActivityTypeId",
                table: "Activities",
                column: "ActivityTypeId",
                principalTable: "ActivityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Modules_ModuleId",
                table: "Activities",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ActivityTypes_ActivityTypeId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Modules_ModuleId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ModuleId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "ActivityTypeId",
                table: "Activities",
                newName: "ActivityTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_ActivityTypeId",
                table: "Activities",
                newName: "IX_Activities_ActivityTypeID");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourseId",
                table: "Modules",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ActivityTypes_ActivityTypeID",
                table: "Activities",
                column: "ActivityTypeID",
                principalTable: "ActivityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
