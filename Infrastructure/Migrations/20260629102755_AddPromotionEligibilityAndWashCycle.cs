using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotionEligibilityAndWashCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequiresBirthday",
                table: "Promotions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CurrentCycleWashes",
                table: "Customers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // (Đã bỏ 4 UpdateData "Customers" cột rỗng do EF tự sinh khi thêm cột mới có default —
            //  chúng tạo ra "UPDATE ... SET WHERE" sai cú pháp làm rollback cả migration.)

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
                columns: new[] { "CreatedAt", "RequiresBirthday" },
                values: new object[] { new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3112), true });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                columns: new[] { "CreatedAt", "RequiresBirthday" },
                values: new object[] { new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3118), true });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                columns: new[] { "CreatedAt", "RequiresBirthday" },
                values: new object[] { new DateTime(2026, 6, 29, 10, 27, 51, 169, DateTimeKind.Utc).AddTicks(3136), true });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiresBirthday",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "CurrentCycleWashes",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 440, DateTimeKind.Utc).AddTicks(9658));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 440, DateTimeKind.Utc).AddTicks(9668));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 440, DateTimeKind.Utc).AddTicks(9674));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 440, DateTimeKind.Utc).AddTicks(9685));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 440, DateTimeKind.Utc).AddTicks(9690));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 440, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 440, DateTimeKind.Utc).AddTicks(9701));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 440, DateTimeKind.Utc).AddTicks(9706));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 440, DateTimeKind.Utc).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 440, DateTimeKind.Utc).AddTicks(9717));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6292));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6387));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6394));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6400));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6405));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6409));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6414));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6419));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6424));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6428));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 16, 790, DateTimeKind.Utc).AddTicks(439), "$2a$11$nCb1JYKXVJfkvlW6RyLSxO232mZ2eDRx21P7Fs9TUaQxSkX9GFQym" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 17, 63, DateTimeKind.Utc).AddTicks(6847), "$2a$11$CebZWMiOqdUG9U.0Szt4weQq0UKlnME80m.MsumqH5qdMz3UEASLm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 17, 343, DateTimeKind.Utc).AddTicks(2308), "$2a$11$H2U6ebXKX9.gD1FlVQvHZeg3jjMV1iHCmeJxgA4nxs83.UWV1czBW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 17, 573, DateTimeKind.Utc).AddTicks(9937), "$2a$11$z.xqnbsu0LAu91SQ2BYhe.snCsbABVg2uNqtQB4nWvkEnPdZdKnzG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 17, 784, DateTimeKind.Utc).AddTicks(3650), "$2a$11$7IatJiCstLZZsdvuKt.USen9iw8bWVt7vTWf6Git8qKNlApU1aL.W" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 18, 143, DateTimeKind.Utc).AddTicks(9505), "$2a$11$Qw8gXIq6dMIUVpEN6YaBPOBFmpxtIi4dyo6zwo/XKJfVUF3j4fyBu" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 18, 385, DateTimeKind.Utc).AddTicks(5214), "$2a$11$4AFGrI5q7o6Z46wD0yQK3OxNnA/s.1.7xBnq/I8asVYYwspnKSmUy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 18, 721, DateTimeKind.Utc).AddTicks(2846), "$2a$11$uwmBQDswvbPIQTghfQWt1.HuqyOI1f.pKW6DrkL3OlNswg2sESRpC" });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 18, 722, DateTimeKind.Utc).AddTicks(9100));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 18, 722, DateTimeKind.Utc).AddTicks(9136));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 18, 722, DateTimeKind.Utc).AddTicks(9141));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 18, 722, DateTimeKind.Utc).AddTicks(9144));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 18, 722, DateTimeKind.Utc).AddTicks(9160));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 18, 722, DateTimeKind.Utc).AddTicks(9163));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 18, 722, DateTimeKind.Utc).AddTicks(9166));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 18, 722, DateTimeKind.Utc).AddTicks(9169));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 18, 722, DateTimeKind.Utc).AddTicks(9147));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 18, 722, DateTimeKind.Utc).AddTicks(9150));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 18, 722, DateTimeKind.Utc).AddTicks(9153));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 25, 14, 10, 18, 722, DateTimeKind.Utc).AddTicks(9157));
        }
    }
}
