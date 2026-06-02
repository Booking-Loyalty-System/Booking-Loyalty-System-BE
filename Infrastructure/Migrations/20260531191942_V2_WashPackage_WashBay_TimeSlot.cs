using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2_WashPackage_WashBay_TimeSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Pre-production: clear existing bookings that reference old schema
            migrationBuilder.Sql("DELETE FROM Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Services_ServiceId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Stores_StoreId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "ServiceFeatures");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ServiceId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_StoreId_BookingDate_StartTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "StoreId",
                table: "Bookings",
                newName: "WashPackageId");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Vehicles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Vehicles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BookingCode",
                table: "Bookings",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "QrData",
                table: "Bookings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TimeSlotId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WashBays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SupportedTypes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WashBays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WashPackages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    Features = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    VehicleType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WashPackages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WashBayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSlots_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TimeSlots_WashBays_WashBayId",
                        column: x => x.WashBayId,
                        principalTable: "WashBays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "WashBays",
                columns: new[] { "Id", "CreatedAt", "Name", "Status", "SupportedTypes" },
                values: new object[,]
                {
                    { new Guid("b1b2c3d4-0001-0001-0001-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bay A1", "Available", "Small,Medium,Large" },
                    { new Guid("b1b2c3d4-0001-0001-0001-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bay A2", "Available", "Small,Medium" },
                    { new Guid("b1b2c3d4-0001-0001-0001-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bay B1", "Available", "Small,Medium,Large" }
                });

            migrationBuilder.InsertData(
                table: "WashPackages",
                columns: new[] { "Id", "CreatedAt", "Description", "DurationMinutes", "Features", "IsActive", "Name", "Price", "VehicleType" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-0001-0001-0001-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Exterior wash with basic cleaning", 20, "[\"Exterior wash\",\"Tire cleaning\",\"Window cleaning\"]", true, "Basic Wash", 80000m, null },
                    { new Guid("a1b2c3d4-0001-0001-0001-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Full interior and exterior cleaning", 30, "[\"Exterior wash\",\"Interior vacuum\",\"Dashboard polish\",\"Tire shine\",\"Air freshener\"]", true, "Premium Wash", 120000m, null },
                    { new Guid("a1b2c3d4-0001-0001-0001-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Complete detailing with wax coating and leather care", 45, "[\"Full exterior wash\",\"Interior deep clean\",\"Wax coating\",\"Leather conditioning\",\"Engine bay cleaning\",\"Ceramic spray\"]", true, "VIP Detailing", 200000m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_WashPackageId",
                table: "Bookings",
                column: "WashPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_BookingId",
                table: "TimeSlots",
                column: "BookingId",
                unique: true,
                filter: "[BookingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_WashBayId_Date_StartTime",
                table: "TimeSlots",
                columns: new[] { "WashBayId", "Date", "StartTime" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_WashPackages_WashPackageId",
                table: "Bookings",
                column: "WashPackageId",
                principalTable: "WashPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_WashPackages_WashPackageId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "TimeSlots");

            migrationBuilder.DropTable(
                name: "WashPackages");

            migrationBuilder.DropTable(
                name: "WashBays");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_WashPackageId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "QrData",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TimeSlotId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "WashPackageId",
                table: "Bookings",
                newName: "StoreId");

            migrationBuilder.AlterColumn<string>(
                name: "BookingCode",
                table: "Bookings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "Bookings",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CloseTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OpenTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    SlotCapacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceFeatures_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "BasePrice", "CreatedAt", "Description", "DurationMinutes", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567801"), 50000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A thorough exterior wash with basic interior cleaning to keep your car fresh and clean.", 30, true, "Basic Wash" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567802"), 100000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Complete interior and exterior detailing with wax application for a showroom finish.", 60, true, "Premium Wash" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567803"), 200000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Our premium service with ceramic coating and paint protection for long-lasting shine.", 90, true, "Ceramic Wash" }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "Address", "City", "CloseTime", "CreatedAt", "IsActive", "Name", "OpenTime", "SlotCapacity" },
                values: new object[] { new Guid("c1000001-0000-0000-0000-000000000001"), "123 Clean Street, Wash City, WC 12345", "Wash City", new TimeOnly(18, 0, 0), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "AutoWash Pro - Main Branch", new TimeOnly(8, 0, 0), 4 });

            migrationBuilder.InsertData(
                table: "ServiceFeatures",
                columns: new[] { "Id", "FeatureDescription", "ServiceId", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("b1000001-0000-0000-0000-000000000001"), "Exterior wash", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567801"), 1 },
                    { new Guid("b1000001-0000-0000-0000-000000000002"), "Tire cleaning", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567801"), 2 },
                    { new Guid("b1000001-0000-0000-0000-000000000003"), "Basic interior vacuum", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567801"), 3 },
                    { new Guid("b1000002-0000-0000-0000-000000000001"), "Everything in Basic", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567802"), 1 },
                    { new Guid("b1000002-0000-0000-0000-000000000002"), "Interior detailing", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567802"), 2 },
                    { new Guid("b1000002-0000-0000-0000-000000000003"), "Wax application", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567802"), 3 },
                    { new Guid("b1000002-0000-0000-0000-000000000004"), "Dashboard polish", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567802"), 4 },
                    { new Guid("b1000003-0000-0000-0000-000000000001"), "Everything in Premium", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567803"), 1 },
                    { new Guid("b1000003-0000-0000-0000-000000000002"), "Ceramic coating", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567803"), 2 },
                    { new Guid("b1000003-0000-0000-0000-000000000003"), "Paint protection", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567803"), 3 },
                    { new Guid("b1000003-0000-0000-0000-000000000004"), "Premium fragrance", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567803"), 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ServiceId",
                table: "Bookings",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_StoreId_BookingDate_StartTime",
                table: "Bookings",
                columns: new[] { "StoreId", "BookingDate", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFeatures_ServiceId",
                table: "ServiceFeatures",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Services_ServiceId",
                table: "Bookings",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Stores_StoreId",
                table: "Bookings",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
