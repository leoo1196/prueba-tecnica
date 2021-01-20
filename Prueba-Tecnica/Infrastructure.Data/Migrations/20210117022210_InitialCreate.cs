using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservationStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationStates", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO ReservationStates VALUES ('Pendiente')");
            migrationBuilder.Sql("INSERT INTO ReservationStates VALUES ('Cancelada')");
            migrationBuilder.Sql("INSERT INTO ReservationStates VALUES ('Finalizada')");

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    HasProjector = table.Column<bool>(type: "bit", nullable: false),
                    HasBlackboard = table.Column<bool>(type: "bit", nullable: false),
                    HasInternet = table.Column<bool>(type: "bit", nullable: false),
                    IsReserved = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfAssistants = table.Column<int>(type: "int", nullable: false),
                    UseProjector = table.Column<bool>(type: "bit", nullable: false),
                    UseBlackboard = table.Column<bool>(type: "bit", nullable: false),
                    UseInternet = table.Column<bool>(type: "bit", nullable: false),
                    IdRoom = table.Column<int>(type: "int", nullable: true),
                    IdReservationState = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_ReservationStates_IdReservationState",
                        column: x => x.IdReservationState,
                        principalTable: "ReservationStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Rooms_IdRoom",
                        column: x => x.IdRoom,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_IdReservationState",
                table: "Reservations",
                column: "IdReservationState");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_IdRoom",
                table: "Reservations",
                column: "IdRoom");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "ReservationStates");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
