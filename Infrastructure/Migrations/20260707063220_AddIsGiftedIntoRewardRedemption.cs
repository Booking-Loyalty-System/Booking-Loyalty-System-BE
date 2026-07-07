using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsGiftedIntoRewardRedemption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGifted",
                table: "RewardRedemptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 885, DateTimeKind.Utc).AddTicks(9255));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 885, DateTimeKind.Utc).AddTicks(9264));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 885, DateTimeKind.Utc).AddTicks(9271));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 885, DateTimeKind.Utc).AddTicks(9276));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 885, DateTimeKind.Utc).AddTicks(9282));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 885, DateTimeKind.Utc).AddTicks(9288));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 885, DateTimeKind.Utc).AddTicks(9292));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 885, DateTimeKind.Utc).AddTicks(9297));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 885, DateTimeKind.Utc).AddTicks(9303));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 885, DateTimeKind.Utc).AddTicks(9308));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 889, DateTimeKind.Utc).AddTicks(6898));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 889, DateTimeKind.Utc).AddTicks(6937));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 889, DateTimeKind.Utc).AddTicks(6941));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 889, DateTimeKind.Utc).AddTicks(6945));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 889, DateTimeKind.Utc).AddTicks(6949));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 889, DateTimeKind.Utc).AddTicks(6953));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 889, DateTimeKind.Utc).AddTicks(6956));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 889, DateTimeKind.Utc).AddTicks(6961));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 889, DateTimeKind.Utc).AddTicks(6965));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 17, 889, DateTimeKind.Utc).AddTicks(6969));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 7, 6, 32, 18, 78, DateTimeKind.Utc).AddTicks(408), "$2a$11$V3o5Qc5cFWHVuCCvsIGPtOhs.fQBoV5tjxzf8Y/3p1AhKayz8O.GW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 7, 6, 32, 18, 266, DateTimeKind.Utc).AddTicks(1213), "$2a$11$cX5/SEtQbN/jsFueP0Tt/OBE1JGj59UpyZYBwso.dky93EFD.5abq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 7, 6, 32, 18, 449, DateTimeKind.Utc).AddTicks(8513), "$2a$11$O45LKUwcwyLLlsiLTUm3vepplJfIDU/Oi7CqpJDp82uJdsMidjJfC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 7, 6, 32, 18, 631, DateTimeKind.Utc).AddTicks(7469), "$2a$11$h/cPPJ6J1fqHtPxGPeRNoehmD2k6ARZ811qmUvFkNwL0WbNBeKtbm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 7, 6, 32, 18, 812, DateTimeKind.Utc).AddTicks(9619), "$2a$11$3p6/mH/ytMnPCStyl9lFXOSoBxN7cYKA9X9Lf5M5CAkjhweEZZ2Xu" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 7, 6, 32, 18, 994, DateTimeKind.Utc).AddTicks(9955), "$2a$11$FipR45cvRhW5ZtiALclpRuPMRbo7dYXFBKB783VKM9FZa2Wn/kY/e" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 7, 6, 32, 19, 177, DateTimeKind.Utc).AddTicks(1935), "$2a$11$j/KrwBLheGZUAvFieaNhcO8XwaZ2JcIcOJNwLc7U9hZsp/6qLX3CW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 7, 6, 32, 19, 361, DateTimeKind.Utc).AddTicks(202), "$2a$11$WTbxG1zwTkBkmC0jk/0dp.iyD99iDGgu7iSb55AcSwrSJUJowf8V2" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("fb9bd07a-5f09-43cc-9ae9-7d3d7d05e128"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(401));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(3466));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(3480));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(3483));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(3486));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(3501));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(3504));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(3507));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(3510));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(3489));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(3492));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(3495));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 6, 32, 19, 362, DateTimeKind.Utc).AddTicks(3498));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGifted",
                table: "RewardRedemptions");

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 737, DateTimeKind.Utc).AddTicks(5548));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 737, DateTimeKind.Utc).AddTicks(5556));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 737, DateTimeKind.Utc).AddTicks(5561));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 737, DateTimeKind.Utc).AddTicks(5566));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 737, DateTimeKind.Utc).AddTicks(5572));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 737, DateTimeKind.Utc).AddTicks(5576));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 737, DateTimeKind.Utc).AddTicks(5580));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 737, DateTimeKind.Utc).AddTicks(5586));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 737, DateTimeKind.Utc).AddTicks(5591));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 737, DateTimeKind.Utc).AddTicks(5596));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 740, DateTimeKind.Utc).AddTicks(7841));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 740, DateTimeKind.Utc).AddTicks(7870));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 740, DateTimeKind.Utc).AddTicks(7875));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 740, DateTimeKind.Utc).AddTicks(7879));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 740, DateTimeKind.Utc).AddTicks(7883));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 740, DateTimeKind.Utc).AddTicks(7887));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 740, DateTimeKind.Utc).AddTicks(7891));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 740, DateTimeKind.Utc).AddTicks(7958));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 740, DateTimeKind.Utc).AddTicks(7965));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 10, 740, DateTimeKind.Utc).AddTicks(7969));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 6, 8, 1, 10, 928, DateTimeKind.Utc).AddTicks(2826), "$2a$11$tsgE85HW0lK.lDpidrl3dezc8aBaAP4BQr.a1uAqSTrB.x/e/0y9G" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 6, 8, 1, 11, 110, DateTimeKind.Utc).AddTicks(2370), "$2a$11$suIJ.byzyYbBFdgTpNf0g.mdKtpGR.rmLKGkAeizJPs.4ziameqEa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 6, 8, 1, 11, 306, DateTimeKind.Utc).AddTicks(3263), "$2a$11$WCcaEDED76dTWKXYejp2KeJ0uRuXTdIjRLqsh26U5cQM6bDm6LEsW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 6, 8, 1, 11, 487, DateTimeKind.Utc).AddTicks(4081), "$2a$11$Q3bBIvtgmO.D2T0n0w3ukuafCnPKiZMxNKwNw4QXqdwBjeNwYpKle" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 6, 8, 1, 11, 668, DateTimeKind.Utc).AddTicks(3737), "$2a$11$/yT2MYzxrH4laWVereVKzuuFTfAFvenbWqqd/DmZKXQtsO4OTmTZK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 6, 8, 1, 11, 849, DateTimeKind.Utc).AddTicks(2177), "$2a$11$tfESp81/nADu7goVBaPLleM2WoQoM6DcAR1NIQ0HsbmIisr0/Eg6m" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 6, 8, 1, 12, 29, DateTimeKind.Utc).AddTicks(3590), "$2a$11$xFDKYCklwBHM3VDIPoP3iukzRNJQ3Lum4asJ24yU3D8GltTPDz88q" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 6, 8, 1, 12, 209, DateTimeKind.Utc).AddTicks(5487), "$2a$11$X1qug9N9awDReJOmCerRjeN0QOUkgsUIwWlhVUwL2qkAF3nx7vNla" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("fb9bd07a-5f09-43cc-9ae9-7d3d7d05e128"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(5084));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(8512));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(8521));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(8524));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(8527));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(8540));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(8544));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(8546));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(8549));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(8529));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(8533));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(8536));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 8, 1, 12, 210, DateTimeKind.Utc).AddTicks(8538));
        }
    }
}
