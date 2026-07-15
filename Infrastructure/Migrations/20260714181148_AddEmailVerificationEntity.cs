using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailVerificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EmailVerifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    VerificationToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    VerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVerifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 0, DateTimeKind.Utc).AddTicks(9040));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 0, DateTimeKind.Utc).AddTicks(9046));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 0, DateTimeKind.Utc).AddTicks(9052));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 0, DateTimeKind.Utc).AddTicks(9057));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 0, DateTimeKind.Utc).AddTicks(9062));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 0, DateTimeKind.Utc).AddTicks(9066));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 0, DateTimeKind.Utc).AddTicks(9071));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 0, DateTimeKind.Utc).AddTicks(9076));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 0, DateTimeKind.Utc).AddTicks(9081));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 0, DateTimeKind.Utc).AddTicks(9087));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 4, DateTimeKind.Utc).AddTicks(168));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 4, DateTimeKind.Utc).AddTicks(190));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 4, DateTimeKind.Utc).AddTicks(195));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 4, DateTimeKind.Utc).AddTicks(202));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 4, DateTimeKind.Utc).AddTicks(206));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 4, DateTimeKind.Utc).AddTicks(209));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 4, DateTimeKind.Utc).AddTicks(213));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 4, DateTimeKind.Utc).AddTicks(279));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 4, DateTimeKind.Utc).AddTicks(285));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 46, 4, DateTimeKind.Utc).AddTicks(288));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "IsEmailConfirmed", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 18, 11, 46, 188, DateTimeKind.Utc).AddTicks(6245), true, "$2a$11$n6RRHXk12aNzI5vVhqIm7OuFnSZjBvfxTxfyg3CmPgSd6aQIIOVk2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "IsEmailConfirmed", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 18, 11, 46, 369, DateTimeKind.Utc).AddTicks(3753), true, "$2a$11$98.GKEdCkHyE5jTEPOzvwO8p4B52v.QCuGun2eG1ziqzHxfEbVkhm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "IsEmailConfirmed", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 18, 11, 46, 551, DateTimeKind.Utc).AddTicks(3089), true, "$2a$11$lClKth59hFCxy4rsZgs9neieaMxAipUysWoqL7YCs2ga9lrlKIbD6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "IsEmailConfirmed", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 18, 11, 46, 732, DateTimeKind.Utc).AddTicks(8460), true, "$2a$11$lSf7JPhKOhXS9oxP3fyRV.oA9rfHxEj8.q2X/9RCkWtEyGvqWhGxm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 18, 11, 46, 913, DateTimeKind.Utc).AddTicks(5456), "$2a$11$slqpL9gdYzpOkOW.5TzVr.I.MlqUBpYs/vSOBmvF5gfhBneqm7NhK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "IsEmailConfirmed", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 18, 11, 47, 94, DateTimeKind.Utc).AddTicks(9095), true, "$2a$11$gWsqPDVPIZ2vCNxn23I0NOgh/lRKIa3dTw20Q.roLj.s40qIfC16i" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "IsEmailConfirmed", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 18, 11, 47, 274, DateTimeKind.Utc).AddTicks(8659), true, "$2a$11$4Wb7iOne4hkcotbmJ2gQq.EiHBI5oA4BKxIx5u6kGKuTyqgYwYTAW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "IsEmailConfirmed", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 18, 11, 47, 454, DateTimeKind.Utc).AddTicks(8067), true, "$2a$11$C4sITlKMvfalAcmZlZ8b7eSYRmpwVop0aWwPDIIrYSQ95cn0BKwVa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                columns: new[] { "CreatedAt", "IsEmailConfirmed", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 18, 11, 47, 636, DateTimeKind.Utc).AddTicks(2876), true, "$2a$11$nmWifi0EoPNEu.jKHBATnOexiI7ht79OcDgisZsw4LId66u23F3vu" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("fb9bd07a-5f09-43cc-9ae9-7d3d7d05e128"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(2519));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(5443));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(5455));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(5459));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(5462));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(5476));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(5480));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(5482));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(5485));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(5465));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(5468));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(5471));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 18, 11, 47, 637, DateTimeKind.Utc).AddTicks(5474));

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerifications_UserId",
                table: "EmailVerifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailVerifications");

            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 646, DateTimeKind.Utc).AddTicks(9783));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 646, DateTimeKind.Utc).AddTicks(9793));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 646, DateTimeKind.Utc).AddTicks(9800));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 646, DateTimeKind.Utc).AddTicks(9806));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 646, DateTimeKind.Utc).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 646, DateTimeKind.Utc).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 646, DateTimeKind.Utc).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 646, DateTimeKind.Utc).AddTicks(9831));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 646, DateTimeKind.Utc).AddTicks(9837));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 646, DateTimeKind.Utc).AddTicks(9846));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 650, DateTimeKind.Utc).AddTicks(3590));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 650, DateTimeKind.Utc).AddTicks(3622));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 650, DateTimeKind.Utc).AddTicks(3627));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 650, DateTimeKind.Utc).AddTicks(3630));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 650, DateTimeKind.Utc).AddTicks(3634));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 650, DateTimeKind.Utc).AddTicks(3637));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 650, DateTimeKind.Utc).AddTicks(3642));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 650, DateTimeKind.Utc).AddTicks(3646));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 650, DateTimeKind.Utc).AddTicks(3652));

            migrationBuilder.UpdateData(
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 52, 650, DateTimeKind.Utc).AddTicks(3656));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 8, 4, 52, 836, DateTimeKind.Utc).AddTicks(7047), "$2a$11$GpmMxxdidrbTPdc9BbQfgetCaGkaVEWx65pGME3aqOWoUGrTS8OUq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 8, 4, 53, 28, DateTimeKind.Utc).AddTicks(357), "$2a$11$OTQifo5UDsFi5HxT9OrvEOaijQIoMhWKJT6HfAQMpQok3Kv/tv0nK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 8, 4, 53, 211, DateTimeKind.Utc).AddTicks(870), "$2a$11$FRI11ZpCILKPUpfnOaxmweowqtrpc0hMyZ4Fu/QDneOQBoWorsKiK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 8, 4, 53, 391, DateTimeKind.Utc).AddTicks(2357), "$2a$11$RTSPK9DENXG3.8O5gctMXechT5Ds6k4mCTMf3F7dSBfEO86x2.xFW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 8, 4, 53, 571, DateTimeKind.Utc).AddTicks(5185), "$2a$11$yi3REadl8nMa/Z1a6r5X.OOT4vdb3jIDli9G3GdbMZUH.H4n9TZkS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 8, 4, 53, 754, DateTimeKind.Utc).AddTicks(2454), "$2a$11$JoQBA2LHFDJ5tlLosQoveuJRRijiYstJJWT4X/gLqcMAUxLLQ4vDC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 8, 4, 53, 936, DateTimeKind.Utc).AddTicks(5532), "$2a$11$Jl3H0Cz7DdrSENo7TalwbOshX8LIL48tnzZKIQrPLRz.sDcLBukm2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 8, 4, 54, 116, DateTimeKind.Utc).AddTicks(7972), "$2a$11$OJrLqTxSLoTNBphCTbAXROn8D2wFJzkkt6AZHjAgQzTn1vnMonSKq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 14, 8, 4, 54, 296, DateTimeKind.Utc).AddTicks(3859), "$2a$11$eiqaXFmvv5/Rf4Qhqcn/6eIMeyV8EDWfWdqpa0WDXNxNtgVpbBQHi" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("fb9bd07a-5f09-43cc-9ae9-7d3d7d05e128"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(3185));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(6293));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(6305));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(6308));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(6311));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(6327));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(6331));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(6333));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0002-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(6336));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(6314));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(6318));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(6321));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0003-0001-0001-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 8, 4, 54, 297, DateTimeKind.Utc).AddTicks(6323));
        }
    }
}
