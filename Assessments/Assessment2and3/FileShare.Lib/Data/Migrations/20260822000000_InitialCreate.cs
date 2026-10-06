using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileShare.Lib.Data.Migrations;

public partial class InitialCreate : Migration
{
    // This method defines the operations to apply the migration, creating the necessary tables and columns in the database.
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Files",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Identifier = table.Column<string>(type: "TEXT", nullable: false),
                FileName = table.Column<string>(type: "TEXT", nullable: false),
                TargetPath = table.Column<string>(type: "TEXT", nullable: false),
                FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                CompletedBlocks = table.Column<int>(type: "INTEGER", nullable: false),
                TotalBlocks = table.Column<int>(type: "INTEGER", nullable: false),
                Completed = table.Column<bool>(type: "INTEGER", nullable: false),
                AddedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Files", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Peers",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                RuntimeId = table.Column<string>(type: "TEXT", nullable: false),
                Nickname = table.Column<string>(type: "TEXT", nullable: false),
                IPAddress = table.Column<string>(type: "TEXT", nullable: false),
                Port = table.Column<int>(type: "INTEGER", nullable: false),
                Connected = table.Column<bool>(type: "INTEGER", nullable: false),
                LastSeen = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Peers", x => x.Id);
            });
    }
    // This method defines the operations to revert the migration, dropping the tables created in the Up method.
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Files");
        migrationBuilder.DropTable(name: "Peers");
    }
}
