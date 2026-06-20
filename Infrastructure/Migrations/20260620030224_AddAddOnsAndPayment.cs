using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAddOnsAndPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("4220590e-136c-4dc0-aabd-1b0c7ca78fee"));

            migrationBuilder.CreateTable(
                name: "AddOns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Gateway = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransactionRef = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TransactionNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ResponseCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "BookingAddOns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddOnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingAddOns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingAddOns_AddOns_AddOnId",
                        column: x => x.AddOnId,
                        principalTable: "AddOns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookingAddOns_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AddOns",
                columns: new[] { "Id", "CreatedAt", "Description", "DurationMinutes", "IsActive", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("a1110001-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Phủ wax bảo vệ sơn", 15, true, "Phủ wax", 50000m },
                    { new Guid("a1110001-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hút bụi, lau sạch nội thất", 20, true, "Vệ sinh nội thất", 80000m },
                    { new Guid("a1110001-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Khử mùi khoang xe", 10, true, "Khử mùi", 30000m },
                    { new Guid("a1110001-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Đánh bóng pha đèn ố mờ", 15, true, "Đánh bóng đèn", 40000m }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "LifetimePoints", "PhoneNumber", "TierId", "TotalPoints", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("055bd3de-11b3-48ae-bd36-6510de2a65c4"), new DateTime(2026, 6, 20, 3, 2, 22, 331, DateTimeKind.Utc).AddTicks(1041), null, "Customer User", false, 0, "0901234567", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 20, 3, 2, 22, 528, DateTimeKind.Utc).AddTicks(6108), "$2a$11$ketiM.L5xl3qmN56.elkMe0zbQ9ZLKCJWk8nlcfYePf0bnr5symJu" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 20, 3, 2, 22, 706, DateTimeKind.Utc).AddTicks(19), "$2a$11$NKtGJxvXvxDiImQgbsPATucsA/wJqCrwaQvEHw.D/GXEdLAM3Mq8K" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 20, 3, 2, 22, 882, DateTimeKind.Utc).AddTicks(3070), "$2a$11$W8HqnEi345uElfrv3cfpWe9s8xSo/N7tQ1EA.iT1dd.TTCV9vTbvK" });

            migrationBuilder.CreateIndex(
                name: "IX_BookingAddOns_AddOnId",
                table: "BookingAddOns",
                column: "AddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingAddOns_BookingId_AddOnId",
                table: "BookingAddOns",
                columns: new[] { "BookingId", "AddOnId" },
                unique: true);

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
                name: "BookingAddOns");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "AddOns");

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("055bd3de-11b3-48ae-bd36-6510de2a65c4"));

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "LifetimePoints", "PhoneNumber", "TierId", "TotalPoints", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("4220590e-136c-4dc0-aabd-1b0c7ca78fee"), new DateTime(2026, 6, 17, 14, 33, 19, 547, DateTimeKind.Utc).AddTicks(1398), null, "Customer User", false, 0, "0901234567", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 17, 14, 33, 19, 724, DateTimeKind.Utc).AddTicks(1933), "$2a$11$rn6bboE7P/BuBpzCDM94tO5b6QBMCgoGgr.QmSoPLfCYc6ZiRq6rq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 17, 14, 33, 19, 884, DateTimeKind.Utc).AddTicks(9527), "$2a$11$.3.6Qs0OPHROG1c5r5Gi5uB.aB8D3rtpmC8oGKd/PXDAaHb8iapJq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 17, 14, 33, 20, 53, DateTimeKind.Utc).AddTicks(6889), "$2a$11$vL9Q5cYpIu57M6EzuZ2XLusSDQRr.AjCdLo9WzLTh4kZn2VoeUeBm" });
        }
    }
}
