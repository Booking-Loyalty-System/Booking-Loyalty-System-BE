using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVoucherFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("aaee83a2-1c01-4ee8-ad5d-1ca49a0f12fd"));

            migrationBuilder.AddColumn<bool>(
                name: "IsVoucher",
                table: "Promotions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PointsCost",
                table: "Promotions",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "LifetimePoints", "PhoneNumber", "TierId", "TotalPoints", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("124cc513-3d3f-4296-a03d-fa685348191f"), new DateTime(2026, 6, 22, 4, 18, 36, 22, DateTimeKind.Utc).AddTicks(4889), null, "Customer User", false, 0, "0901234569", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 4, 18, 36, 172, DateTimeKind.Utc).AddTicks(6819), "$2a$11$Zia39xjr4Eguw4mbh9tVxOqr.icwH.sSD9yzezMq3v/u4yNVjv3Li" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 4, 18, 36, 302, DateTimeKind.Utc).AddTicks(6660), "$2a$11$lOIbCyWV22DSImugjWNgSeZSiq5yrq71vvBs8/1dD7Qd26orrbSEy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 4, 18, 36, 435, DateTimeKind.Utc).AddTicks(4888), "$2a$11$MxcrT0AoNZ3cVWPqCsIs1eCkvHqZK2UInwodVJcCqLFdYyzskHLTy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 4, 18, 36, 566, DateTimeKind.Utc).AddTicks(9010), "$2a$11$/45iH4KfofRaIdEqZiDXyOsQuaIkkf9QKUddfld.3kLApQXkhZ0SC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 4, 18, 36, 702, DateTimeKind.Utc).AddTicks(6884), "$2a$11$Dl4hSQOY9nfBdCiXousnq.JBTDwRRBwA.ze8.nfhBpnguLCfM0zN6" });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 4, 18, 36, 704, DateTimeKind.Utc).AddTicks(1635));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 4, 18, 36, 704, DateTimeKind.Utc).AddTicks(1674));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 4, 18, 36, 704, DateTimeKind.Utc).AddTicks(1678));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 4, 18, 36, 704, DateTimeKind.Utc).AddTicks(1681));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 4, 18, 36, 704, DateTimeKind.Utc).AddTicks(1696));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 4, 18, 36, 704, DateTimeKind.Utc).AddTicks(1699));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 4, 18, 36, 704, DateTimeKind.Utc).AddTicks(1702));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 4, 18, 36, 704, DateTimeKind.Utc).AddTicks(1705));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 4, 18, 36, 704, DateTimeKind.Utc).AddTicks(1684));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 4, 18, 36, 704, DateTimeKind.Utc).AddTicks(1688));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 4, 18, 36, 704, DateTimeKind.Utc).AddTicks(1691));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 4, 18, 36, 704, DateTimeKind.Utc).AddTicks(1694));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("124cc513-3d3f-4296-a03d-fa685348191f"));

            migrationBuilder.DropColumn(
                name: "IsVoucher",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "PointsCost",
                table: "Promotions");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "LifetimePoints", "PhoneNumber", "TierId", "TotalPoints", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("aaee83a2-1c01-4ee8-ad5d-1ca49a0f12fd"), new DateTime(2026, 6, 21, 13, 26, 6, 763, DateTimeKind.Utc).AddTicks(367), null, "Customer User", false, 0, "0901234569", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 21, 13, 26, 6, 906, DateTimeKind.Utc).AddTicks(7782), "$2a$11$/o1z/CLdz8kWYTEVFpHI3OnDcgRh9xhZe9oNGXf8vPXfhIDYbW6w2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 21, 13, 26, 7, 33, DateTimeKind.Utc).AddTicks(9613), "$2a$11$ZQj6jt4AUh.HT34m.ynhjOvKtbzMXFaaKxVDOUWdvvsQca.myJ6tW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 21, 13, 26, 7, 161, DateTimeKind.Utc).AddTicks(1549), "$2a$11$A.743pwk2N4D8IOBq5q6ounmrwdEkJMFty1MrljETcOsiBIaP0H9u" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 21, 13, 26, 7, 288, DateTimeKind.Utc).AddTicks(577), "$2a$11$KXoLDSdv3ZMtzPZ7FShgy.jf/jbJV31Ws7eAF2giQPP2Uv4QMSp8u" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 21, 13, 26, 7, 415, DateTimeKind.Utc).AddTicks(307), "$2a$11$HOUQRkQeZm1V9ujX/H0xD.DoNugjK8k7EW0j2imqjBhn9SLo4Pwbq" });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 13, 26, 7, 416, DateTimeKind.Utc).AddTicks(1662));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 13, 26, 7, 416, DateTimeKind.Utc).AddTicks(1740));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 13, 26, 7, 416, DateTimeKind.Utc).AddTicks(1743));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 13, 26, 7, 416, DateTimeKind.Utc).AddTicks(1745));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 13, 26, 7, 416, DateTimeKind.Utc).AddTicks(1786));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 13, 26, 7, 416, DateTimeKind.Utc).AddTicks(1788));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 13, 26, 7, 416, DateTimeKind.Utc).AddTicks(1790));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 13, 26, 7, 416, DateTimeKind.Utc).AddTicks(1792));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 13, 26, 7, 416, DateTimeKind.Utc).AddTicks(1747));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 13, 26, 7, 416, DateTimeKind.Utc).AddTicks(1749));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 13, 26, 7, 416, DateTimeKind.Utc).AddTicks(1750));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 13, 26, 7, 416, DateTimeKind.Utc).AddTicks(1752));
        }
    }
}
