using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixVoucherDiscountAmountUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6292), 10000.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6387), 20000.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6394), 50000.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6400), 100000.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6405), 150000.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6409), 200000.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6414), 250000.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6419), 300000.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6424), 400000.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 25, 14, 10, 16, 446, DateTimeKind.Utc).AddTicks(6428), 500000.00m });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7050), 10.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7103), 20.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7108), 50.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7112), 100.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7118), 150.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7121), 200.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7125), 250.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7132), 300.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7136), 400.00m });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                columns: new[] { "CreatedAt", "DiscountAmount" },
                values: new object[] { new DateTime(2026, 6, 24, 4, 55, 7, 136, DateTimeKind.Utc).AddTicks(7139), 500.00m });

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
        }
    }
}
