using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaRelacionamentoManyToManyReservationBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Reservation_ReservationId",
                table: "Book");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationBook",
                table: "ReservationBook");

            migrationBuilder.DropIndex(
                name: "IX_ReservationBook_BookId",
                table: "ReservationBook");

            migrationBuilder.DropIndex(
                name: "IX_Book_ReservationId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "ReservationBookId",
                table: "ReservationBook");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Book");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationBook",
                table: "ReservationBook",
                columns: new[] { "BookId", "ReservationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationBook",
                table: "ReservationBook");

            migrationBuilder.AddColumn<Guid>(
                name: "ReservationBookId",
                table: "ReservationBook",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReservationId",
                table: "Book",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationBook",
                table: "ReservationBook",
                column: "ReservationBookId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationBook_BookId",
                table: "ReservationBook",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_ReservationId",
                table: "Book",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Reservation_ReservationId",
                table: "Book",
                column: "ReservationId",
                principalTable: "Reservation",
                principalColumn: "ReservationId");
        }
    }
}
