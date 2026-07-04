using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TierMaintenanceBookingsAndDemo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaintenanceBookings",
                table: "Tiers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 913, DateTimeKind.Utc).AddTicks(5942));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 913, DateTimeKind.Utc).AddTicks(5949));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 913, DateTimeKind.Utc).AddTicks(5955));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 913, DateTimeKind.Utc).AddTicks(5960));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 913, DateTimeKind.Utc).AddTicks(5965));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 913, DateTimeKind.Utc).AddTicks(5970));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 913, DateTimeKind.Utc).AddTicks(5974));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 913, DateTimeKind.Utc).AddTicks(5979));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 913, DateTimeKind.Utc).AddTicks(5984));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 913, DateTimeKind.Utc).AddTicks(5989));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 916, DateTimeKind.Utc).AddTicks(7428));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 916, DateTimeKind.Utc).AddTicks(7486));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 916, DateTimeKind.Utc).AddTicks(7491));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 916, DateTimeKind.Utc).AddTicks(7494));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 916, DateTimeKind.Utc).AddTicks(7499));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 916, DateTimeKind.Utc).AddTicks(7503));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 916, DateTimeKind.Utc).AddTicks(7506));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 916, DateTimeKind.Utc).AddTicks(7510));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 916, DateTimeKind.Utc).AddTicks(7513));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 53, 916, DateTimeKind.Utc).AddTicks(7517));

            migrationBuilder.UpdateData(
                table: "Tiers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "MaintenanceBookings",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tiers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "MaintenanceBookings",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Tiers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "MaintenanceBookings",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Tiers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "MaintenanceBookings",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 2, 6, 40, 54, 119, DateTimeKind.Utc).AddTicks(8493), "$2a$11$jef0g9od4ufXTv2PLvFVX.zSEN.wtHfzyjkXmnhoEr.LWlc0nXSeW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 2, 6, 40, 54, 315, DateTimeKind.Utc).AddTicks(175), "$2a$11$heX4Yy2YFkVnVGTAhjwliua/kYJR4d8oq1Vc3yUoVTwNh6ISzfNqO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 2, 6, 40, 54, 486, DateTimeKind.Utc).AddTicks(7159), "$2a$11$XRIesgwwj.xzkP/K7CMkSOLdU2VHFnd3INMMxpRPaPrnrvBMLwHJy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 2, 6, 40, 54, 831, DateTimeKind.Utc).AddTicks(9668), "$2a$11$5DvGi8ZXX3W3d/JGyS.u9OyEwvnQU1i3dcmrnWz9A.ctvCpGg4lCW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 2, 6, 40, 55, 27, DateTimeKind.Utc).AddTicks(2078), "$2a$11$kifzOayRDl5DSNkNUTkuEukUbGwFp0t.wzoKM9A2hpvNd1pAR24Tq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 2, 6, 40, 55, 212, DateTimeKind.Utc).AddTicks(8857), "$2a$11$9FK.1iLdF5WqzfX739dmRuIByC8x6KGm4fFHJ5vp2bAPRYFPfAPlK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 2, 6, 40, 55, 403, DateTimeKind.Utc).AddTicks(5924), "$2a$11$bFSSVlwS3iHOtb2FpZuzt.h/EaVeqxlyJ9PocG5Lm2cxpy/ED2jmy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 2, 6, 40, 55, 581, DateTimeKind.Utc).AddTicks(2791), "$2a$11$ngyTCgt8lqABddgsSX94TupZDTcevaSpq8dghjPRlHfwBlaAvLIhe" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "GoogleId", "IsActive", "PasswordHash", "RefreshToken", "RefreshTokenExpiry", "Role", "UpdatedAt" },
                values: new object[] { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2026, 7, 2, 6, 40, 55, 790, DateTimeKind.Utc).AddTicks(5039), "downgrade@system.com", null, true, "$2a$11$9mLehFFFQsQHdCzKSLHXKek6u0TLs/mN/5BBwG.aEKIflPimFANh.", null, null, "Customer", null });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 55, 792, DateTimeKind.Utc).AddTicks(5758));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 55, 792, DateTimeKind.Utc).AddTicks(5781));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 55, 792, DateTimeKind.Utc).AddTicks(5785));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 55, 792, DateTimeKind.Utc).AddTicks(5788));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 55, 792, DateTimeKind.Utc).AddTicks(5804));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 55, 792, DateTimeKind.Utc).AddTicks(5807));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 55, 792, DateTimeKind.Utc).AddTicks(5810));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 55, 792, DateTimeKind.Utc).AddTicks(5813));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 55, 792, DateTimeKind.Utc).AddTicks(5791));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 55, 792, DateTimeKind.Utc).AddTicks(5795));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 55, 792, DateTimeKind.Utc).AddTicks(5798));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 6, 40, 55, 792, DateTimeKind.Utc).AddTicks(5801));

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "PhoneNumber", "TierId", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("eeeeeeee-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Customer Downgrade Demo", true, "0900000009", new Guid("44444444-4444-4444-4444-444444444444"), 15000m, 50, new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.InsertData(
                table: "Points",
                columns: new[] { "Id", "AvailablePoints", "TotalPoints", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("99999999-0000-0000-0000-000000000005"), 20000, 20000, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Brand", "Color", "CreatedAt", "CustomerId", "IsDeleted", "IsPrimary", "LicensePlate", "Model", "Type", "VehicleName" },
                values: new object[] { new Guid("99999999-1111-1111-1111-000000000005"), "Toyota", "White", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-1111-1111-1111-111111111111"), false, true, "51D-99999", "2024", "Medium", "Demo Downgrade Car" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Points",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-000000000005"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DropColumn(
                name: "MaintenanceBookings",
                table: "Tiers");

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 184, DateTimeKind.Utc).AddTicks(2675));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 184, DateTimeKind.Utc).AddTicks(2681));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 184, DateTimeKind.Utc).AddTicks(2686));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 184, DateTimeKind.Utc).AddTicks(2690));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 184, DateTimeKind.Utc).AddTicks(2695));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 184, DateTimeKind.Utc).AddTicks(2699));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 184, DateTimeKind.Utc).AddTicks(2703));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 184, DateTimeKind.Utc).AddTicks(2708));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 184, DateTimeKind.Utc).AddTicks(2713));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 184, DateTimeKind.Utc).AddTicks(2717));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 186, DateTimeKind.Utc).AddTicks(7407));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 186, DateTimeKind.Utc).AddTicks(7451));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 186, DateTimeKind.Utc).AddTicks(7456));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 186, DateTimeKind.Utc).AddTicks(7459));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 186, DateTimeKind.Utc).AddTicks(7466));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 186, DateTimeKind.Utc).AddTicks(7470));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 186, DateTimeKind.Utc).AddTicks(7473));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 186, DateTimeKind.Utc).AddTicks(7477));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 186, DateTimeKind.Utc).AddTicks(7480));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 3, 186, DateTimeKind.Utc).AddTicks(7483));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 13, 55, 3, 395, DateTimeKind.Utc).AddTicks(4167), "$2a$11$DqWOHypDsV2MzmYW5Mf8QOLupjqcpUF7VyX8L7TfjrBcO9By1clve" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 13, 55, 3, 583, DateTimeKind.Utc).AddTicks(4715), "$2a$11$iUoZ4wE0/jm4OaEmZL3E7O/tsBd9kUp1YeZ0cpD3XWPAxm7m6y.tK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 13, 55, 3, 800, DateTimeKind.Utc).AddTicks(6888), "$2a$11$amF3OLCkuT8YyK/TvQoYreQhd2PHxOvL3Z8VS/9eyc7d1V76trGAe" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 13, 55, 3, 975, DateTimeKind.Utc).AddTicks(114), "$2a$11$3npIcdyLEm1QePNurUjkPuFaP5n4KnPTL/PmjqvSZT3UBdqC2NHaS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 13, 55, 4, 144, DateTimeKind.Utc).AddTicks(7811), "$2a$11$NLKifkIqXbRVBzOwhzIbSOPB.V8HO07nT/Xz2jce0AS62XjNecvxS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 13, 55, 4, 347, DateTimeKind.Utc).AddTicks(1910), "$2a$11$bo.5RlCdSLI5MEib14uxQ.vqyBX59po3naRibOqH6NsXT5.OqGef6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 13, 55, 4, 551, DateTimeKind.Utc).AddTicks(2525), "$2a$11$SkmwE8ols0zkNkDenK8i3.YsSKV/7rzfkOu3J7WOYB6loX8q.SYGK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 13, 55, 4, 771, DateTimeKind.Utc).AddTicks(8876), "$2a$11$QaZDntnTCpYpRoZlEWQXgeByGSNnXC3qFT3NgWHWb/RH8Yc8wBfwC" });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 4, 774, DateTimeKind.Utc).AddTicks(1193));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 4, 774, DateTimeKind.Utc).AddTicks(1219));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 4, 774, DateTimeKind.Utc).AddTicks(1224));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 4, 774, DateTimeKind.Utc).AddTicks(1228));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 4, 774, DateTimeKind.Utc).AddTicks(1279));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 4, 774, DateTimeKind.Utc).AddTicks(1282));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 4, 774, DateTimeKind.Utc).AddTicks(1286));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 4, 774, DateTimeKind.Utc).AddTicks(1289));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 4, 774, DateTimeKind.Utc).AddTicks(1232));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 4, 774, DateTimeKind.Utc).AddTicks(1238));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 4, 774, DateTimeKind.Utc).AddTicks(1242));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 13, 55, 4, 774, DateTimeKind.Utc).AddTicks(1274));
        }
    }
}
