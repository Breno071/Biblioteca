using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class CriaTabelaReservationBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Reservations_ReservationId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "Reservations",
                newName: "Reservation");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Client");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "Book");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_ClientId",
                table: "Reservation",
                newName: "IX_Reservation_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_ReservationId",
                table: "Book",
                newName: "IX_Book_ReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation",
                column: "ReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Client",
                table: "Client",
                column: "ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Book",
                table: "Book",
                column: "BookId");

            migrationBuilder.CreateTable(
                name: "ReservationBook",
                columns: table => new
                {
                    ReservationBookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationBook", x => x.ReservationBookId);
                    table.ForeignKey(
                        name: "FK_ReservationBook_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationBook_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservation",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationBook_BookId",
                table: "ReservationBook",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationBook_ReservationId",
                table: "ReservationBook",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Reservation_ReservationId",
                table: "Book",
                column: "ReservationId",
                principalTable: "Reservation",
                principalColumn: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Client_ClientId",
                table: "Reservation",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Reservation_ReservationId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Client_ClientId",
                table: "Reservation");

            migrationBuilder.DropTable(
                name: "ReservationBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Client",
                table: "Client");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Book",
                table: "Book");

            migrationBuilder.RenameTable(
                name: "Reservation",
                newName: "Reservations");

            migrationBuilder.RenameTable(
                name: "Client",
                newName: "Clients");

            migrationBuilder.RenameTable(
                name: "Book",
                newName: "Books");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_ClientId",
                table: "Reservations",
                newName: "IX_Reservations_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_ReservationId",
                table: "Books",
                newName: "IX_Books_ReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "ReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Reservations_ReservationId",
                table: "Books",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
