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
                value: new DateTime(2026, 7, 1, 13, 38, 41, 831, DateTimeKind.Utc).AddTicks(2321));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 831, DateTimeKind.Utc).AddTicks(2333));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 831, DateTimeKind.Utc).AddTicks(2344));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 831, DateTimeKind.Utc).AddTicks(2352));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 831, DateTimeKind.Utc).AddTicks(2359));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 831, DateTimeKind.Utc).AddTicks(2369));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 831, DateTimeKind.Utc).AddTicks(2376));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 831, DateTimeKind.Utc).AddTicks(2384));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 831, DateTimeKind.Utc).AddTicks(2394));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 831, DateTimeKind.Utc).AddTicks(2401));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 853, DateTimeKind.Utc).AddTicks(3642));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 853, DateTimeKind.Utc).AddTicks(3782));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 853, DateTimeKind.Utc).AddTicks(3791));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 853, DateTimeKind.Utc).AddTicks(3796));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 853, DateTimeKind.Utc).AddTicks(3802));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 853, DateTimeKind.Utc).AddTicks(3808));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 853, DateTimeKind.Utc).AddTicks(3814));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 853, DateTimeKind.Utc).AddTicks(3821));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 853, DateTimeKind.Utc).AddTicks(3826));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 41, 853, DateTimeKind.Utc).AddTicks(3945));

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
                values: new object[] { new DateTime(2026, 7, 1, 13, 38, 42, 312, DateTimeKind.Utc).AddTicks(2555), "$2a$11$ynIv5o3mHPerkfTGVCqxs.qrQNxoz2x/RvIeWkumccYxH00HpE6N." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 1, 13, 38, 42, 659, DateTimeKind.Utc).AddTicks(5667), "$2a$11$bTnKeSNMGzKDrEjin.2tWO1trDtd/2xjv7HlM.//bI2v1UuU/JA7i" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 1, 13, 38, 43, 5, DateTimeKind.Utc).AddTicks(6074), "$2a$11$YQFUecMie2o8jVFaaZcbV.0o6EFaVq6aSKOsxTXetx3.7K0RZQbaG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 1, 13, 38, 43, 329, DateTimeKind.Utc).AddTicks(6242), "$2a$11$VTwoWcCnzh.gIaQ2PJQymuQC9Mnh2iOjREOTT9qFKV1lB5iPWj25y" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 1, 13, 38, 43, 660, DateTimeKind.Utc).AddTicks(3198), "$2a$11$ngsFBel5M88eXWU8FhnVfulZJoPJW7TWeetnppdnJERA4ZvIa3/NW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 1, 13, 38, 43, 941, DateTimeKind.Utc).AddTicks(2288), "$2a$11$LWxmCFERev6fwJm1MlbHm.btTGrpZMZYbHde4CmujUlnZ/IsowsvO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 1, 13, 38, 44, 264, DateTimeKind.Utc).AddTicks(4227), "$2a$11$lwJXwLqkDZA/VGzn/ii7Y.VwidRTc/XHTueS1Elo5.glwAcXX2OqW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 1, 13, 38, 44, 747, DateTimeKind.Utc).AddTicks(1045), "$2a$11$wjyu5jHWwuGobVBo6DXDSutppX4ZaPlMQ/Jywv7DJi9rwQLJ925KC" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "GoogleId", "IsActive", "PasswordHash", "RefreshToken", "RefreshTokenExpiry", "Role", "UpdatedAt" },
                values: new object[] { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2026, 7, 1, 13, 38, 45, 76, DateTimeKind.Utc).AddTicks(8331), "downgrade@system.com", null, true, "$2a$11$Ygz2btTukXyBKrBhAYfNRunyrr7.ip4tG8p4lrM6BXMzRdQvmiXbO", null, null, "Customer", null });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 45, 80, DateTimeKind.Utc).AddTicks(7515));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 45, 80, DateTimeKind.Utc).AddTicks(7564));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 45, 80, DateTimeKind.Utc).AddTicks(7573));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 45, 80, DateTimeKind.Utc).AddTicks(7580));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 45, 80, DateTimeKind.Utc).AddTicks(7606));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 45, 80, DateTimeKind.Utc).AddTicks(7610));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 45, 80, DateTimeKind.Utc).AddTicks(7615));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 45, 80, DateTimeKind.Utc).AddTicks(7619));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 45, 80, DateTimeKind.Utc).AddTicks(7584));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 45, 80, DateTimeKind.Utc).AddTicks(7589));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 45, 80, DateTimeKind.Utc).AddTicks(7594));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 13, 38, 45, 80, DateTimeKind.Utc).AddTicks(7603));

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
                value: new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3067));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3075));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3082));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3090));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3096));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3101));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3106));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3112));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3118));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3136));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 172, DateTimeKind.Utc).AddTicks(3520));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 172, DateTimeKind.Utc).AddTicks(3585));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 172, DateTimeKind.Utc).AddTicks(3593));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 172, DateTimeKind.Utc).AddTicks(3597));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 172, DateTimeKind.Utc).AddTicks(3602));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 172, DateTimeKind.Utc).AddTicks(3607));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 172, DateTimeKind.Utc).AddTicks(3612));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 172, DateTimeKind.Utc).AddTicks(3616));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 172, DateTimeKind.Utc).AddTicks(3621));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 51, 172, DateTimeKind.Utc).AddTicks(3625));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 10, 27, 51, 384, DateTimeKind.Utc).AddTicks(9350), "$2a$11$Iyk1xN/goNUx0scRNMIhuuob5hzBq36.Pl5XOy9iOEcI4Ks5Pv4kG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 10, 27, 51, 573, DateTimeKind.Utc).AddTicks(5152), "$2a$11$x9Aa1YbS7xmIvGPMOoaZ..B4ygyTMxS7Rh6H.xH9Rd0zyzdNXu32m" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 10, 27, 51, 759, DateTimeKind.Utc).AddTicks(696), "$2a$11$p/rFLYmpbMvxFwBa6cX6mOQydPm8Dzgf4u7.gutgkiiKQkXi4xiWS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 10, 27, 52, 41, DateTimeKind.Utc).AddTicks(9580), "$2a$11$anDk9g8NH7hgSxUBPtT05eCnRnPF60EWRMnPmiTKFd00Jkba3uRqq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 10, 27, 52, 338, DateTimeKind.Utc).AddTicks(4908), "$2a$11$T7ntsdbmKmjwrd/Bcz1dpu77TYyHNFwB5sPwfQKe2HIWgIPphX/PW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 10, 27, 52, 543, DateTimeKind.Utc).AddTicks(5708), "$2a$11$8waVPZYRT5MtPiAM7pIfe.anxt6mU5PmFvgOF2e/R6VOd7CyzWaDe" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 10, 27, 52, 761, DateTimeKind.Utc).AddTicks(1409), "$2a$11$GbJtR2iNBitMzkQNQrSKfOmVxY9HrIwbS7/ruH9S4hGanoLAhwAI." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 10, 27, 52, 953, DateTimeKind.Utc).AddTicks(4647), "$2a$11$DNwBE23kjpc88Nu1iwtTcO1w1AKk0aHvtnj7vhQjtetipnAFaCSxC" });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 52, 955, DateTimeKind.Utc).AddTicks(7340));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 52, 955, DateTimeKind.Utc).AddTicks(7364));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 52, 955, DateTimeKind.Utc).AddTicks(7391));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 52, 955, DateTimeKind.Utc).AddTicks(7396));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 52, 955, DateTimeKind.Utc).AddTicks(7415));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 52, 955, DateTimeKind.Utc).AddTicks(7419));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 52, 955, DateTimeKind.Utc).AddTicks(7422));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 52, 955, DateTimeKind.Utc).AddTicks(7425));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 52, 955, DateTimeKind.Utc).AddTicks(7400));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 52, 955, DateTimeKind.Utc).AddTicks(7404));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 52, 955, DateTimeKind.Utc).AddTicks(7408));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 27, 52, 955, DateTimeKind.Utc).AddTicks(7412));
        }
    }
}
