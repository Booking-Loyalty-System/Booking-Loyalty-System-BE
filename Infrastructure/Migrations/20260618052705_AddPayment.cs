using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("d4cfc9d9-8a9c-428c-ab55-56eb3d1710f5"));

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Method = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Gateway = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TransactionRef = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TransactionNo = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    BankCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ResponseCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "LifetimePoints", "PhoneNumber", "TierId", "TotalPoints", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("85aeacf4-ae5c-4f25-9f9f-2e9ff241ca58"), new DateTime(2026, 6, 18, 5, 27, 0, 815, DateTimeKind.Utc).AddTicks(1459), null, "Customer User", false, 0, "0901234567", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 18, 5, 27, 1, 191, DateTimeKind.Utc).AddTicks(4026), "$2a$11$arLs9rc.uwy39goGxC33zOdiHhWR5jqNiIxkaCGrsCJ82rhDE.nIi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 18, 5, 27, 1, 454, DateTimeKind.Utc).AddTicks(5660), "$2a$11$vBxu5j8eSNMk.uPigsiPxerbvSmdYSRibD0Z5jrvObuw/bttuD.Wa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 18, 5, 27, 1, 694, DateTimeKind.Utc).AddTicks(8653), "$2a$11$2Z1OfNP8Wtfu/msMKxmlX.CENiVwt9g1bM78aBw8So9SGcRnqmQue" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionRef",
                table: "Payments",
                column: "TransactionRef",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("85aeacf4-ae5c-4f25-9f9f-2e9ff241ca58"));

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "LifetimePoints", "PhoneNumber", "TierId", "TotalPoints", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("d4cfc9d9-8a9c-428c-ab55-56eb3d1710f5"), new DateTime(2026, 6, 17, 5, 20, 26, 935, DateTimeKind.Utc).AddTicks(548), null, "Customer User", false, 0, "0901234567", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 17, 5, 20, 27, 334, DateTimeKind.Utc).AddTicks(1802), "$2a$11$Pgtka9FFpdnfgjjj5LfEBeuKmSoLNwLJbQGAHAkivxANfY07GJ2FK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 17, 5, 20, 27, 610, DateTimeKind.Utc).AddTicks(1142), "$2a$11$iVZcJyaPXlJm7O9XYislEeHFC1stb/DKArpHmczbu/g.7M.Wen5xq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 17, 5, 20, 27, 988, DateTimeKind.Utc).AddTicks(7508), "$2a$11$UoEsw5f5mVVDlq83kQ6H5.3/dzvoVByt9y.iVc20eufrRd/1wfr76" });
        }
    }
}
