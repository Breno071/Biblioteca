using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class refatoracaoNosIdsDasEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Reservations_ReservationCode",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Clients_ClientCode",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "ClientCode",
                table: "Reservations",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Reservations",
                newName: "ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_ClientCode",
                table: "Reservations",
                newName: "IX_Reservations_ClientId");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Clients",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "ReservationCode",
                table: "Books",
                newName: "ReservationId");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Books",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_ReservationCode",
                table: "Books",
                newName: "IX_Books_ReservationId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Reservations_ReservationId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Reservations",
                newName: "ClientCode");

            migrationBuilder.RenameColumn(
                name: "ReservationId",
                table: "Reservations",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_ClientId",
                table: "Reservations",
                newName: "IX_Reservations_ClientCode");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Clients",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "ReservationId",
                table: "Books",
                newName: "ReservationCode");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Books",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_Books_ReservationId",
                table: "Books",
                newName: "IX_Books_ReservationCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Reservations_ReservationCode",
                table: "Books",
                column: "ReservationCode",
                principalTable: "Reservations",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Clients_ClientCode",
                table: "Reservations",
                column: "ClientCode",
                principalTable: "Clients",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
