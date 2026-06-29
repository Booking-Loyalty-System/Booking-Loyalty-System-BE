using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestCurrentCycleWashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CurrentCycleWashes", "TotalWashes" },
                values: new object[] { 6, 6 });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CurrentCycleWashes",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CurrentCycleWashes",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CurrentCycleWashes",
                value: 2);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CurrentCycleWashes", "TotalWashes" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CurrentCycleWashes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CurrentCycleWashes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CurrentCycleWashes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 350, DateTimeKind.Utc).AddTicks(5878));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 350, DateTimeKind.Utc).AddTicks(5886));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 350, DateTimeKind.Utc).AddTicks(5892));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 350, DateTimeKind.Utc).AddTicks(5897));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 350, DateTimeKind.Utc).AddTicks(5903));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 350, DateTimeKind.Utc).AddTicks(5908));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 350, DateTimeKind.Utc).AddTicks(5912));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 350, DateTimeKind.Utc).AddTicks(5918));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 350, DateTimeKind.Utc).AddTicks(5923));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 350, DateTimeKind.Utc).AddTicks(5928));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 354, DateTimeKind.Utc).AddTicks(2393));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 354, DateTimeKind.Utc).AddTicks(2462));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 354, DateTimeKind.Utc).AddTicks(2468));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 354, DateTimeKind.Utc).AddTicks(2472));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 354, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 354, DateTimeKind.Utc).AddTicks(2481));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 354, DateTimeKind.Utc).AddTicks(2497));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 354, DateTimeKind.Utc).AddTicks(2501));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 354, DateTimeKind.Utc).AddTicks(2505));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 17, 354, DateTimeKind.Utc).AddTicks(2510));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 12, 32, 17, 563, DateTimeKind.Utc).AddTicks(5419), "$2a$11$W/iu9EC8/uS7uPMKtpia3OXb76wyXyiUpx08AjAtT9AbLpnM4q89C" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 12, 32, 17, 750, DateTimeKind.Utc).AddTicks(5043), "$2a$11$SEhhwCdvxP4xuH5jkqa4feV6XjWz2YkF9kgV7cuHvwSMEGgXg0jhK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 12, 32, 17, 936, DateTimeKind.Utc).AddTicks(9544), "$2a$11$UcKWtPAUPPy95MHeUkEbeujd6Fo2FnFtFDHLWNEM4xYMt.fMrvD7u" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 12, 32, 18, 132, DateTimeKind.Utc).AddTicks(1160), "$2a$11$INp363TfPpx4NRb6JC4Hle.g7Lmk/vRSG6TJkFwE4XiT4O5jV4dAC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 12, 32, 18, 435, DateTimeKind.Utc).AddTicks(4823), "$2a$11$o7irJHpTOHVX3xpSdPgTr.BaP9oodw6osyCiFvOgXQBYFmwXZM2fy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 12, 32, 18, 633, DateTimeKind.Utc).AddTicks(7104), "$2a$11$sFmXHRZi.cBjGzNW6GJr7ek7K84EQy7lO5atdQssQXCCaU1rZpWKy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 12, 32, 18, 825, DateTimeKind.Utc).AddTicks(6920), "$2a$11$0z2eoYfm3ta89CzqkSEuLOhWSOd/WIhkblklPaV6UlMLRKfl1Lq8e" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 29, 12, 32, 18, 995, DateTimeKind.Utc).AddTicks(1460), "$2a$11$uIO16qn4yLOdSVikm9UuK.zs8MhKzwjHU0ECmN8sGyN82oPQwfpWO" });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 18, 996, DateTimeKind.Utc).AddTicks(1320));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 18, 996, DateTimeKind.Utc).AddTicks(1340));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 18, 996, DateTimeKind.Utc).AddTicks(1342));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 18, 996, DateTimeKind.Utc).AddTicks(1345));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 18, 996, DateTimeKind.Utc).AddTicks(1358));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 18, 996, DateTimeKind.Utc).AddTicks(1365));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 18, 996, DateTimeKind.Utc).AddTicks(1367));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 18, 996, DateTimeKind.Utc).AddTicks(1369));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 18, 996, DateTimeKind.Utc).AddTicks(1347));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 18, 996, DateTimeKind.Utc).AddTicks(1350));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 18, 996, DateTimeKind.Utc).AddTicks(1353));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 12, 32, 18, 996, DateTimeKind.Utc).AddTicks(1356));
        }
    }
}
