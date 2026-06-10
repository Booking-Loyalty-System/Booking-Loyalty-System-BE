using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTierThresholds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add new columns to Tiers table
            migrationBuilder.AddColumn<int>(
                name: "MaintenancePoints",
                table: "Tiers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinPointsRequired",
                table: "Tiers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Update Tier seed data with new column values and name changes
            migrationBuilder.UpdateData(
                table: "Tiers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "MaintenancePoints", "MinPointsRequired", "TierName" },
                values: new object[] { 0, 0, "Member" });

            migrationBuilder.UpdateData(
                table: "Tiers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "MaintenancePoints", "MinPointsRequired" },
                values: new object[] { 300, 500 });

            migrationBuilder.UpdateData(
                table: "Tiers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "MaintenancePoints", "MinPointsRequired" },
                values: new object[] { 1000, 1500 });

            migrationBuilder.UpdateData(
                table: "Tiers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "MaintenancePoints", "MinPointsRequired", "TierName" },
                values: new object[] { 3000, 5000, "Platinum" });

            // Fix customer seed: disable all FK constraints, update PK, re-enable
            migrationBuilder.Sql(@"
                -- Disable all FK constraints
                EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

                -- Update customer seed record ID if needed
                DECLARE @OldId UNIQUEIDENTIFIER;
                SELECT @OldId = [Id] FROM [Customers] WHERE [UserId] = 'cccccccc-cccc-cccc-cccc-cccccccccccc';

                IF @OldId IS NOT NULL AND @OldId != 'dddddddd-dddd-dddd-dddd-dddddddddddd'
                BEGIN
                    UPDATE [Bookings] SET [CustomerId] = 'dddddddd-dddd-dddd-dddd-dddddddddddd' WHERE [CustomerId] = @OldId;
                    UPDATE [Vehicles] SET [CustomerId] = 'dddddddd-dddd-dddd-dddd-dddddddddddd' WHERE [CustomerId] = @OldId;
                    UPDATE [Customers] SET [Id] = 'dddddddd-dddd-dddd-dddd-dddddddddddd' WHERE [Id] = @OldId;
                END

                -- Re-enable all FK constraints
                EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaintenancePoints",
                table: "Tiers");

            migrationBuilder.DropColumn(
                name: "MinPointsRequired",
                table: "Tiers");

            migrationBuilder.UpdateData(
                table: "Tiers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "TierName",
                value: "Bronze");

            migrationBuilder.UpdateData(
                table: "Tiers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "TierName",
                value: "Diamond");
        }
    }
}
