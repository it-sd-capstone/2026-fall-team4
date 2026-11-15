using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTicketFlow.Migrations
{
    /// <inheritdoc />
    public partial class FixedVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchasedTickets_ConfirmationCode",
                table: "PurchasedTickets");

            migrationBuilder.DropColumn(
                name: "ConfirmationCode",
                table: "PurchasedTickets");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Customers",
                newName: "Zip");

            migrationBuilder.AddColumn<Guid>(
                name: "CheckinUUID",
                table: "PurchasedTickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "PurchasedTickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CheckinUuid",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            

           

        

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedTickets_OrderId",
                table: "PurchasedTickets",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedTickets_Order_OrderId",
                table: "PurchasedTickets",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedTickets_Order_OrderId",
                table: "PurchasedTickets");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedTickets_OrderId",
                table: "PurchasedTickets");

            migrationBuilder.DropColumn(
                name: "CheckinUUID",
                table: "PurchasedTickets");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "PurchasedTickets");

            migrationBuilder.DropColumn(
                name: "CheckinUuid",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "Zip",
                table: "Customers",
                newName: "ZipCode");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmationCode",
                table: "PurchasedTickets",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            

            

            

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedTickets_ConfirmationCode",
                table: "PurchasedTickets",
                column: "ConfirmationCode",
                unique: true);
        }
    }
}
