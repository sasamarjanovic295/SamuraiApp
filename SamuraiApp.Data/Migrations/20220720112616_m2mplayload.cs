using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamuraiApp.Data.Migrations
{
    public partial class m2mplayload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurai_Battle_BattlesBattleId",
                table: "BattleSamurai");

            migrationBuilder.RenameColumn(
                name: "BattlesBattleId",
                table: "BattleSamurai",
                newName: "BattlesId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataJoined",
                table: "BattleSamurai",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurai_Battle_BattlesId",
                table: "BattleSamurai",
                column: "BattlesId",
                principalTable: "Battle",
                principalColumn: "BattleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurai_Battle_BattlesId",
                table: "BattleSamurai");

            migrationBuilder.DropColumn(
                name: "DataJoined",
                table: "BattleSamurai");

            migrationBuilder.RenameColumn(
                name: "BattlesId",
                table: "BattleSamurai",
                newName: "BattlesBattleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurai_Battle_BattlesBattleId",
                table: "BattleSamurai",
                column: "BattlesBattleId",
                principalTable: "Battle",
                principalColumn: "BattleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
