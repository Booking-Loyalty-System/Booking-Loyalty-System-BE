using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVoucherCodeAndFreeWash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Rewards",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsFreeWash",
                table: "Rewards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "WashPackageId",
                table: "Rewards",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 7, 133, DateTimeKind.Utc).AddTicks(6333));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 7, 133, DateTimeKind.Utc).AddTicks(6340));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 7, 133, DateTimeKind.Utc).AddTicks(6345));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 7, 133, DateTimeKind.Utc).AddTicks(6350));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 7, 133, DateTimeKind.Utc).AddTicks(6356));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 7, 133, DateTimeKind.Utc).AddTicks(6360));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 7, 133, DateTimeKind.Utc).AddTicks(6365));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 7, 133, DateTimeKind.Utc).AddTicks(6370));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 7, 133, DateTimeKind.Utc).AddTicks(6375));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 7, 133, DateTimeKind.Utc).AddTicks(6381));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                columns: new[] { "Code", "CreatedAt", "IsFreeWash", "WashPackageId" },
                values: new object[] { "VOUCHER_10K", new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7050), false, null });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "Code", "CreatedAt", "IsFreeWash", "WashPackageId" },
                values: new object[] { "VOUCHER_20K", new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7103), false, null });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "Code", "CreatedAt", "IsFreeWash", "WashPackageId" },
                values: new object[] { "VOUCHER_50K", new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7108), false, null });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                columns: new[] { "Code", "CreatedAt", "IsFreeWash", "WashPackageId" },
                values: new object[] { "VOUCHER_100K", new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7112), false, null });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                columns: new[] { "Code", "CreatedAt", "IsFreeWash", "WashPackageId" },
                values: new object[] { "VOUCHER_150K", new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7118), false, null });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                columns: new[] { "Code", "CreatedAt", "IsFreeWash", "WashPackageId" },
                values: new object[] { "VOUCHER_200K", new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7121), false, null });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                columns: new[] { "Code", "CreatedAt", "IsFreeWash", "WashPackageId" },
                values: new object[] { "VOUCHER_250K", new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7125), false, null });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                columns: new[] { "Code", "CreatedAt", "IsFreeWash", "WashPackageId" },
                values: new object[] { "VOUCHER_300K", new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7132), false, null });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                columns: new[] { "Code", "CreatedAt", "IsFreeWash", "WashPackageId" },
                values: new object[] { "VOUCHER_400K", new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7136), false, null });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                columns: new[] { "Code", "CreatedAt", "IsFreeWash", "WashPackageId" },
                values: new object[] { "VOUCHER_500K", new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7139), false, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 350, DateTimeKind.Utc).AddTicks(192), "$2a$11$l6.JWVdYM6Xcaw9dQwKFYO5y6npJmBDWkKi2vJKjKLZGHAefoVhwG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 547, DateTimeKind.Utc).AddTicks(153), "$2a$11$Wv22pW6dGW3V9KNm5kSlC.A5DS4FdBVblwFebVaAsebVpVxh.E1we" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 873, DateTimeKind.Utc).AddTicks(3232), "$2a$11$dZt6bz9EwSCDWsTkpiQV9eucPtecjdCODACnB.b0/wzwnFwzwadn." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 8, 138, DateTimeKind.Utc).AddTicks(3623), "$2a$11$cd/QF6paFaBDwOCXQa/t4.bNFLMFtbcbvP/zH0JFGE6C14VwyyiCq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 8, 367, DateTimeKind.Utc).AddTicks(6762), "$2a$11$c3buPj6L.1DYuOvQYXdMUu/OGT0NaUXGzIpPwLgSroHLpYNgctLse" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 8, 575, DateTimeKind.Utc).AddTicks(556), "$2a$11$08vajqB5DaXMcwHYLv.6M..XAsOdX2pP.Bt60VKCh6LmeezY88Qkm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 8, 772, DateTimeKind.Utc).AddTicks(5172), "$2a$11$/23iCPUcjMFgT31rp9vq9ug9.hYCA9Bftxf.gnpXCMOJKCymYq.3W" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 9, 174, DateTimeKind.Utc).AddTicks(6551), "$2a$11$EGYD97BYCNYXHfB/e6yXt.invaMMgZFkGtV2Y9ncpBnvaKAO/4R5a" });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 9, 176, DateTimeKind.Utc).AddTicks(2707));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 9, 176, DateTimeKind.Utc).AddTicks(2742));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 9, 176, DateTimeKind.Utc).AddTicks(2747));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 9, 176, DateTimeKind.Utc).AddTicks(2750));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 9, 176, DateTimeKind.Utc).AddTicks(2767));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 9, 176, DateTimeKind.Utc).AddTicks(2772));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 9, 176, DateTimeKind.Utc).AddTicks(2775));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 9, 176, DateTimeKind.Utc).AddTicks(2778));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 9, 176, DateTimeKind.Utc).AddTicks(2754));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 9, 176, DateTimeKind.Utc).AddTicks(2758));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 9, 176, DateTimeKind.Utc).AddTicks(2761));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 4, 55, 9, 176, DateTimeKind.Utc).AddTicks(2764));

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_WashPackageId",
                table: "Rewards",
                column: "WashPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_WashPackages_WashPackageId",
                table: "Rewards",
                column: "WashPackageId",
                principalTable: "WashPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_WashPackages_WashPackageId",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_WashPackageId",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "IsFreeWash",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "WashPackageId",
                table: "Rewards");

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 232, DateTimeKind.Utc).AddTicks(4319));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 232, DateTimeKind.Utc).AddTicks(4331));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 232, DateTimeKind.Utc).AddTicks(4339));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 232, DateTimeKind.Utc).AddTicks(4346));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 232, DateTimeKind.Utc).AddTicks(4354));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 232, DateTimeKind.Utc).AddTicks(4361));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 232, DateTimeKind.Utc).AddTicks(4367));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 232, DateTimeKind.Utc).AddTicks(4373));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 232, DateTimeKind.Utc).AddTicks(4380));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 232, DateTimeKind.Utc).AddTicks(4387));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 236, DateTimeKind.Utc).AddTicks(1685));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 236, DateTimeKind.Utc).AddTicks(1738));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 236, DateTimeKind.Utc).AddTicks(1744));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 236, DateTimeKind.Utc).AddTicks(1748));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 236, DateTimeKind.Utc).AddTicks(1751));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 236, DateTimeKind.Utc).AddTicks(1756));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 236, DateTimeKind.Utc).AddTicks(1761));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 236, DateTimeKind.Utc).AddTicks(1764));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 236, DateTimeKind.Utc).AddTicks(1768));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 0, 236, DateTimeKind.Utc).AddTicks(1772));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 23, 8, 22, 0, 429, DateTimeKind.Utc).AddTicks(1604), "$2a$11$BdVSeDdRPBwuEpoFgIOl6uaJdfAScyVJKkZCAKmizhF0zTbzln7v6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 23, 8, 22, 0, 615, DateTimeKind.Utc).AddTicks(7337), "$2a$11$/qGSfGzw20ZdqEBurTe90Oeh99wTPokDBahL27dYPPGisTN6ioL.W" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 23, 8, 22, 0, 811, DateTimeKind.Utc).AddTicks(3091), "$2a$11$Zqnb.F.hZPzDugqxtNSdg.Dc3TeWgKDrXwCmot4RNSuJmRn.ldF52" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 23, 8, 22, 0, 999, DateTimeKind.Utc).AddTicks(7281), "$2a$11$GwMemSFZrN.h7g9se70ux.tChf7qMvRgybMzr5qOzx5GF.R74LrM2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 23, 8, 22, 1, 182, DateTimeKind.Utc).AddTicks(1751), "$2a$11$apVO5ormGNb2eFVj1WuqUuhCS7w.wciZL1USP95N8lRXNkrc.Gnyq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 23, 8, 22, 1, 369, DateTimeKind.Utc).AddTicks(7337), "$2a$11$C1X2xnM38iycFQoUXFFH..XjW69m90Hs5xyhuW3RRzhlU.WLWjGAK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 23, 8, 22, 1, 557, DateTimeKind.Utc).AddTicks(2570), "$2a$11$AaZ0xLb1Tz2o7N2bjTiAhexcwlnOSrhaE/cOnAhY04ab6IpCpoJSO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 23, 8, 22, 1, 750, DateTimeKind.Utc).AddTicks(5721), "$2a$11$t0QGUk4gKjz6AeVII.Kbfe2OcTAazoYes/CK5ENMfuWnxNNdAZeOq" });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 1, 751, DateTimeKind.Utc).AddTicks(8185));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 1, 751, DateTimeKind.Utc).AddTicks(8200));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 1, 751, DateTimeKind.Utc).AddTicks(8203));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 1, 751, DateTimeKind.Utc).AddTicks(8206));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 1, 751, DateTimeKind.Utc).AddTicks(8220));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 1, 751, DateTimeKind.Utc).AddTicks(8223));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 1, 751, DateTimeKind.Utc).AddTicks(8226));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 1, 751, DateTimeKind.Utc).AddTicks(8229));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 1, 751, DateTimeKind.Utc).AddTicks(8209));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 1, 751, DateTimeKind.Utc).AddTicks(8212));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 1, 751, DateTimeKind.Utc).AddTicks(8215));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 8, 22, 1, 751, DateTimeKind.Utc).AddTicks(8218));
        }
    }
}
