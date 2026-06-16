using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingAssignedStaffId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("77fbca87-3666-4c12-8363-1f871dd39b57"));

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedStaffId",
                table: "Bookings",
                type: "uuid",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("ef8d989f-e50c-4c12-86fe-0404c97df152"));

            migrationBuilder.DropColumn(
                name: "AssignedStaffId",
                table: "Bookings");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "LifetimePoints", "PhoneNumber", "TierId", "TotalPoints", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[] { new Guid("77fbca87-3666-4c12-8363-1f871dd39b57"), new DateTime(2026, 6, 9, 5, 37, 31, 925, DateTimeKind.Utc).AddTicks(346), null, "Customer User", false, 0, "0901234567", new Guid("11111111-1111-1111-1111-111111111111"), 0, 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 9, 5, 37, 32, 103, DateTimeKind.Utc).AddTicks(5913), "$2a$11$s6Rk6RwLfmwRC3yUJLlW7.K6AKe8xXBq3OoqzPW8U5Mh23N1cXojm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 9, 5, 37, 32, 279, DateTimeKind.Utc).AddTicks(45), "$2a$11$tI2w7UNsGDrYrKARA5RnveSrXvQH6cQwCH0kpSrKk0NRW4SluVEF2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 9, 5, 37, 32, 447, DateTimeKind.Utc).AddTicks(7246), "$2a$11$YmZjbEk253Pnq2UsECCm4.Dy1hCqM0igISBARCf68O2sHeoEqimWi" });
        }
    }
}
