using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeparateVoucherFromPromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("a6581345-8fad-49b4-b6ea-8622a720f33d"));

            migrationBuilder.DropColumn(
                name: "IsVoucher",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "PointsCost",
                table: "Promotions");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "LifetimePoints", "PhoneNumber", "TierId", "TotalPoints", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("18375525-1b7e-444c-859f-dfaab1d86efd"), new DateTime(2026, 6, 22, 13, 9, 27, 952, DateTimeKind.Utc).AddTicks(5928), null, "Customer User", false, 0, "0901234569", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 13, 9, 28, 272, DateTimeKind.Utc).AddTicks(155), "$2a$11$A4b25X0wfWewYRxCUyLxYeGw.D0szkju/Xt7/LkwwnuFsHSuMYN9C" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 13, 9, 28, 507, DateTimeKind.Utc).AddTicks(9609), "$2a$11$FAj/JxCn1ia4zuPraXiIV.TPxB13QXE/LwDUEzmi5i1Ksbi7YBMDK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 13, 9, 28, 822, DateTimeKind.Utc).AddTicks(314), "$2a$11$rPMepPCsLDxq0rTA9XiyOOMGG1azf90gqZ1u395j0L.G0P.zC6scC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 13, 9, 29, 86, DateTimeKind.Utc).AddTicks(3907), "$2a$11$AC0wQoPieLklQKpWFIjAA.ygKkP3UdIEPbtb17m/hqsvA7LDjkpT6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 13, 9, 29, 348, DateTimeKind.Utc).AddTicks(1142), "$2a$11$2Ib7DgDBbUrI1MwstaXb2e32i.X24t41zw2WxjUZrJI3LnamaRiui" });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 13, 9, 29, 354, DateTimeKind.Utc).AddTicks(5328));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 13, 9, 29, 354, DateTimeKind.Utc).AddTicks(5395));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 13, 9, 29, 354, DateTimeKind.Utc).AddTicks(5406));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 13, 9, 29, 354, DateTimeKind.Utc).AddTicks(5414));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 13, 9, 29, 354, DateTimeKind.Utc).AddTicks(5543));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 13, 9, 29, 354, DateTimeKind.Utc).AddTicks(5556));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 13, 9, 29, 354, DateTimeKind.Utc).AddTicks(5560));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 13, 9, 29, 354, DateTimeKind.Utc).AddTicks(5566));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 13, 9, 29, 354, DateTimeKind.Utc).AddTicks(5418));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 13, 9, 29, 354, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 13, 9, 29, 354, DateTimeKind.Utc).AddTicks(5532));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 13, 9, 29, 354, DateTimeKind.Utc).AddTicks(5539));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("18375525-1b7e-444c-859f-dfaab1d86efd"));

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
                values: new object[] { new Guid("a6581345-8fad-49b4-b6ea-8622a720f33d"), new DateTime(2026, 6, 22, 6, 47, 48, 774, DateTimeKind.Utc).AddTicks(5773), null, "Customer User", false, 0, "0901234569", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 6, 47, 48, 976, DateTimeKind.Utc).AddTicks(8152), "$2a$11$vJwHizeagMPdgp1DWtoJEeK62hGgctMYm5KHf1iu2FT5frrFzaXCi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 6, 47, 49, 178, DateTimeKind.Utc).AddTicks(7640), "$2a$11$bzWm7rl/Ty6AIwKRTlCffO/5mBPIt4zpgen03BBqrtXm4b5gs/w.S" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 6, 47, 49, 363, DateTimeKind.Utc).AddTicks(940), "$2a$11$OjK1kUIy/PG0XA3EYQ2ZjuRpCk8HXLmqE25zAdkwW8Z5j95pKgUqC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 6, 47, 49, 545, DateTimeKind.Utc).AddTicks(8099), "$2a$11$YUf.tUvs/lZIi.66sK8L.uEtO4adJCT6pYhmI/tCmDIRn8kNWeyUu" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 22, 6, 47, 49, 736, DateTimeKind.Utc).AddTicks(7382), "$2a$11$b88k0ldRKrGS1b0dJMvDJeWQoGGYt4PNjLKcGHyG8tfLuWziSc4S2" });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 6, 47, 49, 738, DateTimeKind.Utc).AddTicks(8775));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 6, 47, 49, 738, DateTimeKind.Utc).AddTicks(8823));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 6, 47, 49, 738, DateTimeKind.Utc).AddTicks(8827));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 6, 47, 49, 738, DateTimeKind.Utc).AddTicks(8829));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 6, 47, 49, 738, DateTimeKind.Utc).AddTicks(8843));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 6, 47, 49, 738, DateTimeKind.Utc).AddTicks(8846));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 6, 47, 49, 738, DateTimeKind.Utc).AddTicks(8849));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 6, 47, 49, 738, DateTimeKind.Utc).AddTicks(8851));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 6, 47, 49, 738, DateTimeKind.Utc).AddTicks(8832));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 6, 47, 49, 738, DateTimeKind.Utc).AddTicks(8835));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 6, 47, 49, 738, DateTimeKind.Utc).AddTicks(8838));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 6, 47, 49, 738, DateTimeKind.Utc).AddTicks(8841));
        }
    }
}
