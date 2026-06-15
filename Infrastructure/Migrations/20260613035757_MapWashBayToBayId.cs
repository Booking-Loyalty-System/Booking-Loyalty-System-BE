using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MapWashBayToBayId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Branches_BranchId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_WashBays_WashBayId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_WashBayId",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("ef8d989f-e50c-4c12-86fe-0404c97df152"));

            migrationBuilder.DropColumn(
                name: "WashBayId",
                table: "Bookings");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "LifetimePoints", "PhoneNumber", "TierId", "TotalPoints", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("16afc246-f830-4110-bc24-8eae0556a732"), new DateTime(2026, 6, 13, 3, 57, 55, 836, DateTimeKind.Utc).AddTicks(3988), null, "Customer User", false, 0, "0901234567", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 13, 3, 57, 56, 60, DateTimeKind.Utc).AddTicks(2648), "$2a$11$8kxuGmYUgMEeN8PakkisgeAjQI4UatHxosmlxxtk6omjloCKDf.ea" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 13, 3, 57, 56, 346, DateTimeKind.Utc).AddTicks(1804), "$2a$11$wkK4g0m8L5RXTEZV0WeGhejslOIbHmdPZfZv.sy2gMmv2UezlGJKa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 13, 3, 57, 56, 624, DateTimeKind.Utc).AddTicks(1838), "$2a$11$LRvjeGtO2thu7nO48ipyku2cR8Sof0KY0mV/P4Nz5yg7ckihylZ9." });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BayId",
                table: "Bookings",
                column: "BayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Branches_BranchId",
                table: "Bookings",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_WashBays_BayId",
                table: "Bookings",
                column: "BayId",
                principalTable: "WashBays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Branches_BranchId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_WashBays_BayId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BayId",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("16afc246-f830-4110-bc24-8eae0556a732"));

            migrationBuilder.AddColumn<Guid>(
                name: "WashBayId",
                table: "Bookings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "LifetimePoints", "PhoneNumber", "TierId", "TotalPoints", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("ef8d989f-e50c-4c12-86fe-0404c97df152"), new DateTime(2026, 6, 13, 3, 41, 45, 742, DateTimeKind.Utc).AddTicks(6499), null, "Customer User", false, 0, "0901234567", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 13, 3, 41, 46, 15, DateTimeKind.Utc).AddTicks(9596), "$2a$11$JeZW6nwreeGbinCNUaGjMOWjJEsztgQ1bAYzLhsCC9NhSoFZ8q.Ya" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 13, 3, 41, 46, 385, DateTimeKind.Utc).AddTicks(989), "$2a$11$SbcKYP8zgHK/za/lWH362uYwsH9xGmWHfJxp5pslsMxkT2cZQgFlm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 13, 3, 41, 46, 716, DateTimeKind.Utc).AddTicks(9352), "$2a$11$0VVLOuMtu5BMSOPZjk2GGO/rt.w18Y.gn4ZJ6tJcJmk9IkMyYjT9W" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_WashBayId",
                table: "Bookings",
                column: "WashBayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Branches_BranchId",
                table: "Bookings",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_WashBays_WashBayId",
                table: "Bookings",
                column: "WashBayId",
                principalTable: "WashBays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
