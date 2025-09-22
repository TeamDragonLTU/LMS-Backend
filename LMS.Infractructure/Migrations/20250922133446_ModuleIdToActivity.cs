using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Companies.Infractructure.Migrations
{
    /// <inheritdoc />
    public partial class ModuleIdToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ModuleID",
                table: "Activities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ModuleID",
                table: "Activities",
                column: "ModuleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Modules_ModuleID",
                table: "Activities",
                column: "ModuleID",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Modules_ModuleID",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ModuleID",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ModuleID",
                table: "Activities");
        }
    }
}
