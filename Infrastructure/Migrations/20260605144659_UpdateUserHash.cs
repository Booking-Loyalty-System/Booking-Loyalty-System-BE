using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 5, 14, 46, 58, 481, DateTimeKind.Utc).AddTicks(5544), "$2a$11$nuWCTOFz8ExOZYYqB0/9pOMAFonpfZ7jtGi/ThJrgXLy1miCQFkN2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 5, 14, 46, 58, 663, DateTimeKind.Utc).AddTicks(1083), "$2a$11$YwK0/23qxuVFfL6EJwmse.wtUkgLvG92yxH9PCD4Bv9zJz7kf6HU2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 5, 14, 40, 0, 865, DateTimeKind.Utc).AddTicks(992), "$2a$11$wS5L7C442v43.hHhQ4z9hO8C0P1.n0mS0p9N5T/G1J5F0mU7C5mSy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 6, 5, 14, 40, 0, 865, DateTimeKind.Utc).AddTicks(996), "$2a$11$q9hHhQ4z9hO8C0P1.n0mS0p9N5T/G1J5F0mU7C5mSywS5L7C442v4" });
        }
    }
}
