using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VoucherRedemptionExpiryAndBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("f55f529f-00bc-4c88-9300-f03d853f5966"));

            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                table: "RewardRedemptions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "RewardRedemptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "PhoneNumber", "TierId", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("f49e138e-d4a8-4428-b715-bae867b7e4b1"), new DateTime(2026, 6, 22, 14, 54, 38, 921, DateTimeKind.Utc).AddTicks(3902), null, "Customer User", false, "0901234569", new Guid("11111111-1111-1111-1111-111111111111"), 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 54, 39, 123, DateTimeKind.Utc).AddTicks(5233), "$2a$11$MoM0KjjuuOrpoIfGxtCOa.txCeuAS/vQBRekGwk9zb3Zp0cELmKya" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 54, 39, 299, DateTimeKind.Utc).AddTicks(6171), "$2a$11$6bSYov41roBBNuZnZCV.Be9gsEfX34Z2oUr41NkZQ9mhgLVGKbd8m" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 54, 39, 464, DateTimeKind.Utc).AddTicks(6041), "$2a$11$eQqM2Rpt.Em6YJs0ZFgvc.WJeMZQ1iLKx8bMAsRGVFCfBl5QtekSm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 54, 39, 636, DateTimeKind.Utc).AddTicks(3742), "$2a$11$90BnloomNaBFp41/T5SylOm6uSato/e.ddjS8/0THdcxYph.UZA7m" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 54, 39, 798, DateTimeKind.Utc).AddTicks(401), "$2a$11$97xVajXza3YsB9tgLGqkL.JY/TQPit.UcHD13esqAqF6aO2.dYnXK" });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 54, 39, 799, DateTimeKind.Utc).AddTicks(2960));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 54, 39, 799, DateTimeKind.Utc).AddTicks(2992));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 54, 39, 799, DateTimeKind.Utc).AddTicks(2996));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 54, 39, 799, DateTimeKind.Utc).AddTicks(2999));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 54, 39, 799, DateTimeKind.Utc).AddTicks(3022));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 54, 39, 799, DateTimeKind.Utc).AddTicks(3025));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 54, 39, 799, DateTimeKind.Utc).AddTicks(3027));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 54, 39, 799, DateTimeKind.Utc).AddTicks(3030));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 54, 39, 799, DateTimeKind.Utc).AddTicks(3001));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 54, 39, 799, DateTimeKind.Utc).AddTicks(3014));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 54, 39, 799, DateTimeKind.Utc).AddTicks(3017));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 54, 39, 799, DateTimeKind.Utc).AddTicks(3020));

            migrationBuilder.CreateIndex(
                name: "IX_RewardRedemptions_BookingId",
                table: "RewardRedemptions",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_RewardRedemptions_Bookings_BookingId",
                table: "RewardRedemptions",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RewardRedemptions_Bookings_BookingId",
                table: "RewardRedemptions");

            migrationBuilder.DropIndex(
                name: "IX_RewardRedemptions_BookingId",
                table: "RewardRedemptions");

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("f49e138e-d4a8-4428-b715-bae867b7e4b1"));

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "RewardRedemptions");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "RewardRedemptions");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "PhoneNumber", "TierId", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("f55f529f-00bc-4c88-9300-f03d853f5966"), new DateTime(2026, 6, 22, 14, 30, 5, 606, DateTimeKind.Utc).AddTicks(728), null, "Customer User", false, "0901234569", new Guid("11111111-1111-1111-1111-111111111111"), 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 30, 5, 880, DateTimeKind.Utc).AddTicks(5422), "$2a$11$HvpdAB259FENASnE/KEmbOwDQg6VXdCPXbxUrV4lSe/CegJ3Zuaiy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 30, 6, 107, DateTimeKind.Utc).AddTicks(9977), "$2a$11$LICdfOTRkGmZYWh6oW2tY.PxivYhkDOAU92Oc7xkWhTlMLO1q/Wtq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 30, 6, 331, DateTimeKind.Utc).AddTicks(6255), "$2a$11$XrMlofJpVgfq2y6H.fPChudheIO804sGI9dyYBpNoKL7NF9XY9Gw." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 30, 6, 563, DateTimeKind.Utc).AddTicks(3932), "$2a$11$ci28djVqODWKB/PqUjnQT.71xLLT0akeeGiHdWSo3LNo1dBoB1NtK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 30, 6, 775, DateTimeKind.Utc).AddTicks(4675), "$2a$11$oIQ0h6OZ/vEtMwZgXiVymu3uDKFKxOQ6u0Ms3BVdEPgqj7LhdRVkK" });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 30, 6, 780, DateTimeKind.Utc).AddTicks(9798));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 30, 6, 780, DateTimeKind.Utc).AddTicks(9843));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 30, 6, 780, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 30, 6, 780, DateTimeKind.Utc).AddTicks(9852));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 30, 6, 780, DateTimeKind.Utc).AddTicks(9905));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 30, 6, 780, DateTimeKind.Utc).AddTicks(9909));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 30, 6, 780, DateTimeKind.Utc).AddTicks(9912));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 30, 6, 780, DateTimeKind.Utc).AddTicks(9916));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 30, 6, 780, DateTimeKind.Utc).AddTicks(9855));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 30, 6, 780, DateTimeKind.Utc).AddTicks(9860));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 30, 6, 780, DateTimeKind.Utc).AddTicks(9863));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 14, 30, 6, 780, DateTimeKind.Utc).AddTicks(9867));
        }
    }
}
