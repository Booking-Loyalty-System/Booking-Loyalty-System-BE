using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLoyaltyTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("c601249d-6385-4bb7-98b8-85ad1a205500"));

            migrationBuilder.CreateTable(
                name: "LoyaltyTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    BalanceAfter = table.Column<int>(type: "integer", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoyaltyTransactions_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LoyaltyTransactions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "LifetimePoints", "PhoneNumber", "TierId", "TotalPoints", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("77fbca87-3666-4c12-8363-1f871dd39b57"), new DateTime(2026, 6, 9, 5, 37, 31, 925, DateTimeKind.Utc).AddTicks(346), null, "Customer User", false, 0, "0901234567", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 9, 5, 37, 32, 103, DateTimeKind.Utc).AddTicks(5913), "$2a$11$s6Rk6RwLfmwRC3yUJLlW7.K6AKe8xXBq3OoqzPW8U5Mh23N1cXojm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 9, 5, 37, 32, 279, DateTimeKind.Utc).AddTicks(45), "$2a$11$tI2w7UNsGDrYrKARA5RnveSrXvQH6cQwCH0kpSrKk0NRW4SluVEF2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 9, 5, 37, 32, 447, DateTimeKind.Utc).AddTicks(7246), "$2a$11$YmZjbEk253Pnq2UsECCm4.Dy1hCqM0igISBARCf68O2sHeoEqimWi" });

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyTransactions_BookingId",
                table: "LoyaltyTransactions",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyTransactions_CustomerId_CreatedAt",
                table: "LoyaltyTransactions",
                columns: new[] { "CustomerId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoyaltyTransactions");

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("77fbca87-3666-4c12-8363-1f871dd39b57"));

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "LifetimePoints", "PhoneNumber", "TierId", "TotalPoints", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("c601249d-6385-4bb7-98b8-85ad1a205500"), new DateTime(2026, 6, 6, 17, 15, 0, 61, DateTimeKind.Utc).AddTicks(905), null, "Customer User", false, 0, "0901234567", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 6, 17, 15, 0, 246, DateTimeKind.Utc).AddTicks(3355), "$2a$11$3lnBZY5MzN.Zcij2G7hPSO5A4zt9TenRWZi9mL90/ubMz8YWWBaOm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 6, 17, 15, 0, 428, DateTimeKind.Utc).AddTicks(1068), "$2a$11$yjUU3pxWXk8n0Kfu3teSKe3Y2kble578QL58uEF/GAYpVXh3yg0Eu" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 6, 17, 15, 0, 608, DateTimeKind.Utc).AddTicks(5517), "$2a$11$uckH484liwHQMfTqN4LuVOZ6T5t/vzkgJJJBt0CvKWqg9oQkNEEbe" });
        }
    }
}
