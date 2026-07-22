using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotionPerCustomerLimitAndTierDowngradeStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxUsesPerCustomer",
                table: "Promotions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTierDowngradeAt",
                table: "Customers",
                type: "timestamp with time zone",
                nullable: true);

            // Đã bỏ 5 khối UpdateData "LastTierDowngradeAt = null" cho Customers do EF tự sinh:
            // cột vừa thêm đã null sẵn nên chúng thừa, và UpdateData 1 cột value null khiến Npgsql
            // sinh câu "SET" rỗng -> lỗi syntax Postgres, rollback cả migration (gotcha đã gặp).

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedAt", "MaxUsesPerCustomer" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 17, 787, DateTimeKind.Utc).AddTicks(7307), null });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedAt", "MaxUsesPerCustomer" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 17, 787, DateTimeKind.Utc).AddTicks(7313), null });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                columns: new[] { "CreatedAt", "MaxUsesPerCustomer" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 17, 787, DateTimeKind.Utc).AddTicks(7318), null });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                columns: new[] { "CreatedAt", "MaxUsesPerCustomer" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 17, 787, DateTimeKind.Utc).AddTicks(7323), null });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                columns: new[] { "CreatedAt", "MaxUsesPerCustomer" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 17, 787, DateTimeKind.Utc).AddTicks(7327), null });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                columns: new[] { "CreatedAt", "MaxUsesPerCustomer" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 17, 787, DateTimeKind.Utc).AddTicks(7331), null });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                columns: new[] { "CreatedAt", "MaxUsesPerCustomer" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 17, 787, DateTimeKind.Utc).AddTicks(7336), null });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                columns: new[] { "CreatedAt", "MaxUsesPerCustomer" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 17, 787, DateTimeKind.Utc).AddTicks(7340), null });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                columns: new[] { "CreatedAt", "MaxUsesPerCustomer" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 17, 787, DateTimeKind.Utc).AddTicks(7345), null });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                columns: new[] { "CreatedAt", "MaxUsesPerCustomer" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 17, 787, DateTimeKind.Utc).AddTicks(7351), null });

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 17, 789, DateTimeKind.Utc).AddTicks(8012));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 17, 789, DateTimeKind.Utc).AddTicks(8062));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 17, 789, DateTimeKind.Utc).AddTicks(8066));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 17, 789, DateTimeKind.Utc).AddTicks(8069));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 17, 789, DateTimeKind.Utc).AddTicks(8072));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 17, 789, DateTimeKind.Utc).AddTicks(8075));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 17, 789, DateTimeKind.Utc).AddTicks(8079));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 17, 789, DateTimeKind.Utc).AddTicks(8083));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 17, 789, DateTimeKind.Utc).AddTicks(8086));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 17, 789, DateTimeKind.Utc).AddTicks(8089));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 18, 31, DateTimeKind.Utc).AddTicks(5270), "$2a$11$NlarBmnv0JDupxpsCGn0DOSzcyUkHkjczax1jgfDvr78iPykSHtGa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 18, 249, DateTimeKind.Utc).AddTicks(217), "$2a$11$XykHHSn3lOXOiBM8cvp2Zu5aQGGbq6brnmIunWEeaxd4bgb/uWd/y" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 18, 469, DateTimeKind.Utc).AddTicks(8541), "$2a$11$jDVUcXQNMjVE6R4zgf2ghO/4Ui6H.41WCDodYpsLKwh6d/NPYNW8e" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 18, 786, DateTimeKind.Utc).AddTicks(8870), "$2a$11$Hy6TymuDwdXV5AeXfhfVzefMtrlWTJdvIH6LqSDKsh/FimV0G9TCe" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 19, 23, DateTimeKind.Utc).AddTicks(4847), "$2a$11$Q3ZIDYHl0mvi7O65/Fv7gOwEL0LW81zmyvcNLZbcu/o.zk1ivHWje" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 19, 220, DateTimeKind.Utc).AddTicks(6326), "$2a$11$76pHt6rkH15wU1Ykq07kAuoaciVE95d3D2oJ07eDjcVg48g9KnjGG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 19, 441, DateTimeKind.Utc).AddTicks(3259), "$2a$11$7PaUaG/a2BDrQA0Tgjjjq.RPab75YO2a7wHWDz0k2xGfjxwa96XwO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 19, 643, DateTimeKind.Utc).AddTicks(6766), "$2a$11$JDDa.VSXn.RjhJrU1aWMZuhWkvZo25/6eQHTF6WkWXF9BGwz6iCE2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 22, 12, 44, 19, 954, DateTimeKind.Utc).AddTicks(9008), "$2a$11$ozcDfJWyPuMLumBlGWZcruUP3vKxsDYgPGgZGIh3rlt6zh27DaSge" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("fb9bd07a-5f09-43cc-9ae9-7d3d7d05e128"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 959, DateTimeKind.Utc).AddTicks(6411));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 961, DateTimeKind.Utc).AddTicks(4472));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 961, DateTimeKind.Utc).AddTicks(4582));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 961, DateTimeKind.Utc).AddTicks(4586));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 961, DateTimeKind.Utc).AddTicks(4590));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 961, DateTimeKind.Utc).AddTicks(4607));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 961, DateTimeKind.Utc).AddTicks(4610));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 961, DateTimeKind.Utc).AddTicks(4613));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 961, DateTimeKind.Utc).AddTicks(4616));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 961, DateTimeKind.Utc).AddTicks(4593));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 961, DateTimeKind.Utc).AddTicks(4597));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 961, DateTimeKind.Utc).AddTicks(4601));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 12, 44, 19, 961, DateTimeKind.Utc).AddTicks(4604));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxUsesPerCustomer",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "LastTierDowngradeAt",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 540, DateTimeKind.Utc).AddTicks(3720));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 540, DateTimeKind.Utc).AddTicks(3729));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 540, DateTimeKind.Utc).AddTicks(3737));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 540, DateTimeKind.Utc).AddTicks(3743));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 540, DateTimeKind.Utc).AddTicks(3749));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 540, DateTimeKind.Utc).AddTicks(3756));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 540, DateTimeKind.Utc).AddTicks(3762));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 540, DateTimeKind.Utc).AddTicks(3769));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 540, DateTimeKind.Utc).AddTicks(3775));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 540, DateTimeKind.Utc).AddTicks(3782));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 545, DateTimeKind.Utc).AddTicks(2643));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 545, DateTimeKind.Utc).AddTicks(2740));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 545, DateTimeKind.Utc).AddTicks(2746));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 545, DateTimeKind.Utc).AddTicks(2750));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 545, DateTimeKind.Utc).AddTicks(2755));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 545, DateTimeKind.Utc).AddTicks(2760));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 545, DateTimeKind.Utc).AddTicks(2764));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 545, DateTimeKind.Utc).AddTicks(2769));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 545, DateTimeKind.Utc).AddTicks(2773));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 44, 545, DateTimeKind.Utc).AddTicks(2777));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 9, 9, 28, 44, 961, DateTimeKind.Utc).AddTicks(2960), "$2a$11$H6KFeWFaTFzYPW..951Fd.avtQo2i9liVaVpE7ATBxJverGNFhbnO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 9, 9, 28, 45, 233, DateTimeKind.Utc).AddTicks(4278), "$2a$11$al4A8YNhwyYLE9GIL0yFS.cSd8T/roUNjJMZfcxRYsqEY5pjEBNI2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 9, 9, 28, 45, 496, DateTimeKind.Utc).AddTicks(7202), "$2a$11$s371sdQPciJFfyAdm/sF3.JHYS2qrJfzDwk7lczYzQWPaquYo4sEK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 9, 9, 28, 45, 762, DateTimeKind.Utc).AddTicks(9346), "$2a$11$QGgDFMCIZmd19LE8KmG/i.iVTJdtegJwyCEI2g/dB1XiHR.VfCQh." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 9, 9, 28, 46, 25, DateTimeKind.Utc).AddTicks(6190), "$2a$11$MzG62VhAcOyHMaDE9Xg9iOu/V9EK37wChW0JHC/5OlahQWcIip5ha" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 9, 9, 28, 46, 279, DateTimeKind.Utc).AddTicks(5861), "$2a$11$7fkdQliMRBMgqVTDzQnRVO.SAIY3aAM5BBI6IBD5xzFydPmUAEIxC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 9, 9, 28, 46, 532, DateTimeKind.Utc).AddTicks(9153), "$2a$11$i.KrsEhipEKZm9ekL75kO.fM14AqJhh6lhdgf8iArHiIE2jS3I/xC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 9, 9, 28, 46, 803, DateTimeKind.Utc).AddTicks(4479), "$2a$11$YHUred9BkswDwteigXqzZ.JIbKRKMH61s36OjBE/qcdKM2r8C615e" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 9, 9, 28, 47, 60, DateTimeKind.Utc).AddTicks(1655), "$2a$11$chEvsmq.d5YtSFbcxcZhmO6.Cwe7Ow6cbu2Eg.i97bwWBI29jT6.u" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("fb9bd07a-5f09-43cc-9ae9-7d3d7d05e128"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 61, DateTimeKind.Utc).AddTicks(9939));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 62, DateTimeKind.Utc).AddTicks(6591));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 62, DateTimeKind.Utc).AddTicks(6610));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 62, DateTimeKind.Utc).AddTicks(6615));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 62, DateTimeKind.Utc).AddTicks(6619));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 62, DateTimeKind.Utc).AddTicks(6639));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 62, DateTimeKind.Utc).AddTicks(6642));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 62, DateTimeKind.Utc).AddTicks(6646));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 62, DateTimeKind.Utc).AddTicks(6650));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 62, DateTimeKind.Utc).AddTicks(6623));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 62, DateTimeKind.Utc).AddTicks(6628));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 62, DateTimeKind.Utc).AddTicks(6632));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 9, 28, 47, 62, DateTimeKind.Utc).AddTicks(6636));
        }
    }
}
