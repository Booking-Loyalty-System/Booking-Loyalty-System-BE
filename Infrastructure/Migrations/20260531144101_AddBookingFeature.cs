using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Vehicles",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleName",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OpenTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    CloseTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    SlotCapacity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Bookings_BookingCode",
                table: "Bookings",
                column: "BookingCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ServiceId",
                table: "Bookings",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_StoreId_BookingDate_StartTime",
                table: "Bookings",
                columns: new[] { "StoreId", "BookingDate", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VehicleId",
                table: "Bookings",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFeatures_ServiceId",
                table: "ServiceFeatures",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "ServiceFeatures");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleName",
                table: "Vehicles");
        }
    }
}
