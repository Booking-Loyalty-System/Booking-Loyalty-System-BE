using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddOns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Hotline = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OperatingHours = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriorityLevel = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxUses = table.Column<int>(type: "int", nullable: true),
                    UsedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MinSpend = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TierName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PointRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingWindow = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    MinPointsRequired = table.Column<int>(type: "int", nullable: false),
                    MaintenancePoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GoogleId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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
                name: "WashBays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SupportedTypes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WashBays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WashBays_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionBranches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionBranches_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionBranches_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TierPromotions",
                columns: table => new
                {
                    TierPromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TierPromotions", x => x.TierPromotionId);
                    table.ForeignKey(
                        name: "FK_TierPromotions_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TierPromotions_Tiers_TierId",
                        column: x => x.TierId,
                        principalTable: "Tiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchTimeSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaxCapacity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchTimeSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchTimeSlots_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchTimeSlots_TimeSlots_TimeSlotId",
                        column: x => x.TimeSlotId,
                        principalTable: "TimeSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    IsPhoneNumberVerified = table.Column<bool>(type: "bit", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalWashes = table.Column<int>(type: "int", nullable: false),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Tiers_TierId",
                        column: x => x.TierId,
                        principalTable: "Tiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvailablePoints = table.Column<int>(type: "int", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Points_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staffs_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staffs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointsRequired = table.Column<int>(type: "int", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointsCost = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsFreeWash = table.Column<bool>(type: "bit", nullable: false),
                    WashPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rewards_WashPackages_WashPackageId",
                        column: x => x.WashPackageId,
                        principalTable: "WashPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPromotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPromotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPromotions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerPromotions_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    VehicleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WashPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchTimeSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BayId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    RewardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    QrData = table.Column<string>(type: "text", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_BranchTimeSlots_BranchTimeSlotId",
                        column: x => x.BranchTimeSlotId,
                        principalTable: "BranchTimeSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Rewards_RewardId",
                        column: x => x.RewardId,
                        principalTable: "Rewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_WashBays_BayId",
                        column: x => x.BayId,
                        principalTable: "WashBays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bookings_WashPackages_WashPackageId",
                        column: x => x.WashPackageId,
                        principalTable: "WashPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingAddOns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddOnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingAddOns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingAddOns_AddOns_AddOnId",
                        column: x => x.AddOnId,
                        principalTable: "AddOns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookingAddOns_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Gateway = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransactionRef = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TransactionNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ResponseCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PointHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BalanceAfter = table.Column<int>(type: "int", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RewardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointHistories_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PointHistories_Points_PointId",
                        column: x => x.PointId,
                        principalTable: "Points",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointHistories_Rewards_RewardId",
                        column: x => x.RewardId,
                        principalTable: "Rewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RewardRedemptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RewardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointsSpent = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FulfilledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardRedemptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardRedemptions_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RewardRedemptions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RewardRedemptions_Rewards_RewardId",
                        column: x => x.RewardId,
                        principalTable: "Rewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AddOns",
                columns: new[] { "Id", "CreatedAt", "Description", "DurationMinutes", "IsActive", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("a1110001-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Phủ wax bảo vệ sơn", 15, true, "Phủ wax", 50000m },
                    { new Guid("a1110001-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hút bụi, lau sạch nội thất", 20, true, "Vệ sinh nội thất", 80000m },
                    { new Guid("a1110001-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Khử mùi khoang xe", 10, true, "Khử mùi", 30000m },
                    { new Guid("a1110001-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Đánh bóng pha đèn ố mờ", 15, true, "Đánh bóng đèn", 40000m }
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "Address", "BranchName", "Hotline", "Latitude", "Longitude", "OperatingHours", "Status" },
                values: new object[,]
                {
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02"), "19Bis Cộng Hòa, Bảy Hiền, Hồ Chí Minh, Vietnam", "Tân Bình Branch", "0912345678", 10.801600799999999, 106.64823730000001, "8am-9pm", "Active" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03"), "303 Cách Mạng Tháng 8 Tổ 20, Khu phố Khu phố, Hòa Hưng, Hồ Chí Minh, Vietnam", "Quận 3 Branch", "0987654321", 10.7794176, 106.678039, "8am-8pm", "Active" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "625 Nguyễn Xiển, Long Bình, Hồ Chí Minh 70000, Vietnam", "Quận 9 Branch", "0123456789", 10.8415798, 106.8294047, "8am-9pm", "Active" }
                });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "Code", "CreatedAt", "Description", "DiscountType", "DiscountValue", "EndDate", "IsActive", "MaxUses", "MinSpend", "Name", "PriorityLevel", "StartDate" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000000001"), "TB-PERCENT", new DateTime(2026, 6, 25, 4, 11, 42, 878, DateTimeKind.Utc).AddTicks(6641), "Giảm 10% cho toàn bộ hóa đơn tại Tân Bình", 0, 10.00m, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, 500, 100000m, "Ưu đãi Tân Bình", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), "Q3-PERCENT", new DateTime(2026, 6, 25, 4, 11, 42, 878, DateTimeKind.Utc).AddTicks(6674), "Giảm 15% cho toàn bộ hóa đơn tại Quận 3", 0, 15.00m, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, 500, 150000m, "Ưu đãi Quận 3", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), "Q9-PERCENT", new DateTime(2026, 6, 25, 4, 11, 42, 878, DateTimeKind.Utc).AddTicks(6678), "Giảm 20% cho toàn bộ hóa đơn tại Quận 9", 0, 20.00m, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, 500, 200000m, "Ưu đãi Quận 9", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), "BRONZE-10K", new DateTime(2026, 6, 25, 4, 11, 42, 878, DateTimeKind.Utc).AddTicks(6682), "Giảm 5% cho thành viên Đồng", 1, 5.00m, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Ưu đãi hạng Bronze", 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), "SILVER-50K", new DateTime(2026, 6, 25, 4, 11, 42, 878, DateTimeKind.Utc).AddTicks(6686), "Giảm 10% cho thành viên Bạc", 1, 10.00m, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 150000m, "Ưu đãi hạng Silver", 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c0000000-0000-0000-0000-000000000006"), "GOLD-15", new DateTime(2026, 6, 25, 4, 11, 42, 878, DateTimeKind.Utc).AddTicks(6689), "Giảm 15% cho thành viên Vàng", 0, 15.00m, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Đặc quyền hạng Gold", 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c0000000-0000-0000-0000-000000000007"), "DIAMOND-VIP", new DateTime(2026, 6, 25, 4, 11, 42, 878, DateTimeKind.Utc).AddTicks(6692), "Giảm 25% tối đa đặc quyền Kim Cương", 0, 25.00m, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Đẳng cấp Diamond", 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c0000000-0000-0000-0000-000000000008"), "BDAY-15", new DateTime(2026, 6, 25, 4, 11, 42, 878, DateTimeKind.Utc).AddTicks(6696), "Giảm 15% trong ngày sinh nhật của bạn", 0, 15.00m, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 200000m, "Mừng Sinh Nhật 15%", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c0000000-0000-0000-0000-000000000009"), "BDAY-HAPPY", new DateTime(2026, 6, 25, 4, 11, 42, 878, DateTimeKind.Utc).AddTicks(6699), "Giảm 5% cho hóa đơn đặt trước vào tuần sinh nhật", 0, 5.00m, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 500000m, "Sinh Nhật Vui Vẻ 5%", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c0000000-0000-0000-0000-000000000010"), "BDAY-MEGA", new DateTime(2026, 6, 25, 4, 11, 42, 878, DateTimeKind.Utc).AddTicks(6703), "Giảm tối đa 20% cho hóa đơn đặt tiệc sinh nhật lớn", 0, 20.00m, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, null, 1000000m, "Đại Tiệc Sinh Nhật 20%", 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Rewards",
                columns: new[] { "Id", "Code", "CreatedAt", "Description", "DiscountAmount", "EndDate", "IsActive", "IsFreeWash", "Name", "PointsCost", "PointsRequired", "StartDate", "Status", "WashPackageId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "VOUCHER_10K", new DateTime(2026, 6, 25, 4, 11, 42, 880, DateTimeKind.Utc).AddTicks(5057), "Giảm 10,000đ", 10.00m, new DateOnly(2026, 12, 31), true, false, "Voucher 10k", 50, 0, new DateOnly(2026, 1, 1), true, null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "VOUCHER_20K", new DateTime(2026, 6, 25, 4, 11, 42, 880, DateTimeKind.Utc).AddTicks(5078), "Giảm 20,000đ", 20.00m, new DateOnly(2026, 12, 31), true, false, "Voucher 20k", 100, 0, new DateOnly(2026, 1, 1), true, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "VOUCHER_50K", new DateTime(2026, 6, 25, 4, 11, 42, 880, DateTimeKind.Utc).AddTicks(5081), "Giảm 50,000đ", 50.00m, new DateOnly(2026, 12, 31), true, false, "Voucher 50k", 250, 0, new DateOnly(2026, 1, 1), true, null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "VOUCHER_100K", new DateTime(2026, 6, 25, 4, 11, 42, 880, DateTimeKind.Utc).AddTicks(5084), "Giảm 100,000đ", 100.00m, new DateOnly(2026, 12, 31), true, false, "Voucher 100k", 500, 0, new DateOnly(2026, 1, 1), true, null },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "VOUCHER_150K", new DateTime(2026, 6, 25, 4, 11, 42, 880, DateTimeKind.Utc).AddTicks(5086), "Giảm 150,000đ", 150.00m, new DateOnly(2026, 12, 31), true, false, "Voucher 150k", 750, 0, new DateOnly(2026, 1, 1), true, null },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "VOUCHER_200K", new DateTime(2026, 6, 25, 4, 11, 42, 880, DateTimeKind.Utc).AddTicks(5089), "Giảm 200,000đ", 200.00m, new DateOnly(2026, 12, 31), true, false, "Voucher 200k", 1000, 0, new DateOnly(2026, 1, 1), true, null },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "VOUCHER_250K", new DateTime(2026, 6, 25, 4, 11, 42, 880, DateTimeKind.Utc).AddTicks(5092), "Giảm 250,000đ", 250.00m, new DateOnly(2026, 12, 31), true, false, "Voucher 250k", 1250, 0, new DateOnly(2026, 1, 1), true, null },
                    { new Guid("10000000-0000-0000-0000-000000000008"), "VOUCHER_300K", new DateTime(2026, 6, 25, 4, 11, 42, 880, DateTimeKind.Utc).AddTicks(5095), "Giảm 300,000đ", 300.00m, new DateOnly(2026, 12, 31), true, false, "Voucher 300k", 1500, 0, new DateOnly(2026, 1, 1), true, null },
                    { new Guid("10000000-0000-0000-0000-000000000009"), "VOUCHER_400K", new DateTime(2026, 6, 25, 4, 11, 42, 880, DateTimeKind.Utc).AddTicks(5097), "Giảm 400,000đ", 400.00m, new DateOnly(2026, 12, 31), true, false, "Voucher 400k", 2000, 0, new DateOnly(2026, 1, 1), true, null },
                    { new Guid("10000000-0000-0000-0000-000000000010"), "VOUCHER_500K", new DateTime(2026, 6, 25, 4, 11, 42, 880, DateTimeKind.Utc).AddTicks(5128), "Giảm 500,000đ", 500.00m, new DateOnly(2026, 12, 31), true, false, "Voucher 500k", 2500, 0, new DateOnly(2026, 1, 1), true, null }
                });

            migrationBuilder.InsertData(
                table: "Tiers",
                columns: new[] { "Id", "BookingWindow", "Level", "MaintenancePoints", "MinPointsRequired", "PointRate", "TierName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 7, 4, 0, 0, 1.00m, "Bronze" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 14, 3, 300, 2000, 1.50m, "Silver" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 21, 2, 1000, 6000, 2.00m, "Gold" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 30, 1, 3000, 15000, 3.00m, "Diamond" }
                });

            migrationBuilder.InsertData(
                table: "TimeSlots",
                columns: new[] { "Id", "StartTime" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000008"), new TimeOnly(8, 0, 0) },
                    { new Guid("00000000-0000-0000-0000-000000000009"), new TimeOnly(9, 0, 0) },
                    { new Guid("00000000-0000-0000-0000-000000000010"), new TimeOnly(10, 0, 0) },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new TimeOnly(11, 0, 0) },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new TimeOnly(12, 0, 0) },
                    { new Guid("00000000-0000-0000-0000-000000000013"), new TimeOnly(13, 0, 0) },
                    { new Guid("00000000-0000-0000-0000-000000000014"), new TimeOnly(14, 0, 0) },
                    { new Guid("00000000-0000-0000-0000-000000000015"), new TimeOnly(15, 0, 0) },
                    { new Guid("00000000-0000-0000-0000-000000000016"), new TimeOnly(16, 0, 0) },
                    { new Guid("00000000-0000-0000-0000-000000000017"), new TimeOnly(17, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "GoogleId", "IsActive", "PasswordHash", "RefreshToken", "RefreshTokenExpiry", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 25, 4, 11, 43, 20, DateTimeKind.Utc).AddTicks(4832), "admin@system.com", null, true, "$2a$11$0qlg24HLDfVgFqPfgBSXQu1Y6UaihGPlCSN3D649diIhuy0hAA9uu", null, null, "Admin", null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 25, 4, 11, 43, 156, DateTimeKind.Utc).AddTicks(2387), "staff@system.com", null, true, "$2a$11$HWwJ9P8z8pFCx8B8AaJHOOK/KGfiM4.xBqZXM9vwGYUOVLSSM7VRy", null, null, "Staff", null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"), new DateTime(2026, 6, 25, 4, 11, 43, 287, DateTimeKind.Utc).AddTicks(2637), "staff1@system.com", null, true, "$2a$11$dxRsrCfjH9JSOcvdzXPYsegfd1exWAcJ9BxGA0SU8wK4/HQvgECJa", null, null, "Staff", null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"), new DateTime(2026, 6, 25, 4, 11, 43, 416, DateTimeKind.Utc).AddTicks(7576), "staff2@system.com", null, true, "$2a$11$AmQ/p8mtVJLAbRCrXZoZteDDu/rH2SoPBBAVeNZ8kzMhXd4/LSUsC", null, null, "Staff", null },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2026, 6, 25, 4, 11, 43, 549, DateTimeKind.Utc).AddTicks(8308), "customer@system.com", null, true, "$2a$11$z6G1FD7kRBSNgV6tBF8T2uUl2GfGE4IcfecYbcSJtNRWskzU8SPW2", null, null, "Customer", null },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"), new DateTime(2026, 6, 25, 4, 11, 43, 680, DateTimeKind.Utc).AddTicks(2019), "cus2@system.com", null, true, "$2a$11$a1hUlC9HA6MOvZauyEORie0/5tZM58aavYLtm.yaPblVeewt93USS", null, null, "Customer", null },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"), new DateTime(2026, 6, 25, 4, 11, 43, 811, DateTimeKind.Utc).AddTicks(4328), "cus3@system.com", null, true, "$2a$11$vkZ/M1jZ4IwbWgmucQ2oWuF5r/MDZi4mB0XHYghgJiWbtRUv0C5Ba", null, null, "Customer", null },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"), new DateTime(2026, 6, 25, 4, 11, 43, 947, DateTimeKind.Utc).AddTicks(4682), "cus4@system.com", null, true, "$2a$11$LhjmQXttvINASgACNMMOseUXqvJbs/NL.kRJmnYtEaSFO47HFh9bu", null, null, "Customer", null }
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

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "FullName", "IsPhoneNumberVerified", "PhoneNumber", "TierId", "TotalSpent", "TotalWashes", "UserId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Customer Bronze Tier", false, "0901234569", new Guid("11111111-1111-1111-1111-111111111111"), 0m, 0, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Customer Silver Tier", true, "0901234568", new Guid("22222222-2222-2222-2222-222222222222"), 500m, 5, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd") },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Customer Gold Tier", true, "0901234567", new Guid("33333333-3333-3333-3333-333333333333"), 2500m, 15, new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce") },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Customer Diamond Tier", true, "0901234566", new Guid("44444444-4444-4444-4444-444444444444"), 7000m, 40, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf") }
                });

            migrationBuilder.InsertData(
                table: "PromotionBranches",
                columns: new[] { "Id", "BranchId", "ExpiryDate", "IsActive", "PromotionId" },
                values: new object[,]
                {
                    { new Guid("e0000000-0000-0000-0000-000000000001"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new Guid("c0000000-0000-0000-0000-000000000001") },
                    { new Guid("e0000000-0000-0000-0000-000000000002"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new Guid("c0000000-0000-0000-0000-000000000002") },
                    { new Guid("e0000000-0000-0000-0000-000000000003"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new Guid("c0000000-0000-0000-0000-000000000003") }
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "BranchId", "FullName", "IsAvailable", "PhoneNumber", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Nguyễn Văn Staff", true, "0901234567", new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
                    { new Guid("11111111-1111-1111-1111-111111111112"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03"), "Trần Thị Staff Một", true, "0907654321", new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc") },
                    { new Guid("11111111-1111-1111-1111-111111111113"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02"), "Lê Hoàng Staff Hai", true, "0911223344", new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd") }
                });

            migrationBuilder.InsertData(
                table: "TierPromotions",
                columns: new[] { "TierPromotionId", "PromotionId", "TierId" },
                values: new object[,]
                {
                    { new Guid("d0000000-0000-0000-0000-000000000001"), new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("d0000000-0000-0000-0000-000000000002"), new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("d0000000-0000-0000-0000-000000000003"), new Guid("c0000000-0000-0000-0000-000000000006"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("d0000000-0000-0000-0000-000000000004"), new Guid("c0000000-0000-0000-0000-000000000007"), new Guid("44444444-4444-4444-4444-444444444444") }
                });

            migrationBuilder.InsertData(
                table: "WashBays",
                columns: new[] { "Id", "BranchId", "CreatedAt", "Name", "Status", "SupportedTypes" },
                values: new object[,]
                {
                    { new Guid("b1b2c3d4-0001-0001-0001-000000000001"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 25, 4, 11, 43, 948, DateTimeKind.Utc).AddTicks(7921), "Bay A1 (Q9)", "Available", "Small,Medium" },
                    { new Guid("b1b2c3d4-0001-0001-0001-000000000002"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 25, 4, 11, 43, 948, DateTimeKind.Utc).AddTicks(7956), "Bay A2 (Q9)", "Available", "Small,Medium" },
                    { new Guid("b1b2c3d4-0001-0001-0001-000000000003"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 25, 4, 11, 43, 948, DateTimeKind.Utc).AddTicks(7960), "Bay B1 (Q9)", "Available", "Small,Medium,Large" },
                    { new Guid("b1b2c3d4-0001-0001-0001-000000000004"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 25, 4, 11, 43, 948, DateTimeKind.Utc).AddTicks(7962), "Bay B2 (Q9)", "Available", "Small,Medium,Large" },
                    { new Guid("b1b2c3d4-0002-0001-0001-000000000001"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02"), new DateTime(2026, 6, 25, 4, 11, 43, 948, DateTimeKind.Utc).AddTicks(8026), "Bay A1 (TB)", "Available", "Small,Medium" },
                    { new Guid("b1b2c3d4-0002-0001-0001-000000000002"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02"), new DateTime(2026, 6, 25, 4, 11, 43, 948, DateTimeKind.Utc).AddTicks(8029), "Bay A2 (TB)", "Available", "Small,Medium" },
                    { new Guid("b1b2c3d4-0002-0001-0001-000000000003"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02"), new DateTime(2026, 6, 25, 4, 11, 43, 948, DateTimeKind.Utc).AddTicks(8031), "Bay B1 (TB)", "Available", "Small,Medium,Large" },
                    { new Guid("b1b2c3d4-0002-0001-0001-000000000004"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb02"), new DateTime(2026, 6, 25, 4, 11, 43, 948, DateTimeKind.Utc).AddTicks(8033), "Bay B2 (TB)", "Available", "Small,Medium,Large" },
                    { new Guid("b1b2c3d4-0003-0001-0001-000000000001"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03"), new DateTime(2026, 6, 25, 4, 11, 43, 948, DateTimeKind.Utc).AddTicks(7964), "Bay A1 (Q3)", "Available", "Small,Medium" },
                    { new Guid("b1b2c3d4-0003-0001-0001-000000000002"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03"), new DateTime(2026, 6, 25, 4, 11, 43, 948, DateTimeKind.Utc).AddTicks(7967), "Bay A2 (Q3)", "Available", "Small,Medium" },
                    { new Guid("b1b2c3d4-0003-0001-0001-000000000003"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03"), new DateTime(2026, 6, 25, 4, 11, 43, 948, DateTimeKind.Utc).AddTicks(7969), "Bay B1 (Q3)", "Available", "Small,Medium,Large" },
                    { new Guid("b1b2c3d4-0003-0001-0001-000000000004"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbb03"), new DateTime(2026, 6, 25, 4, 11, 43, 948, DateTimeKind.Utc).AddTicks(8024), "Bay B2 (Q3)", "Available", "Small,Medium,Large" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingAddOns_AddOnId",
                table: "BookingAddOns",
                column: "AddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingAddOns_BookingId_AddOnId",
                table: "BookingAddOns",
                columns: new[] { "BookingId", "AddOnId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BayId",
                table: "Bookings",
                column: "BayId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingCode",
                table: "Bookings",
                column: "BookingCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BranchTimeSlotId",
                table: "Bookings",
                column: "BranchTimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PromotionId",
                table: "Bookings",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RewardId",
                table: "Bookings",
                column: "RewardId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_StaffId",
                table: "Bookings",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VehicleId",
                table: "Bookings",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_WashPackageId",
                table: "Bookings",
                column: "WashPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchTimeSlots_BranchId_TimeSlotId",
                table: "BranchTimeSlots",
                columns: new[] { "BranchId", "TimeSlotId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BranchTimeSlots_TimeSlotId",
                table: "BranchTimeSlots",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPromotions_CustomerId",
                table: "CustomerPromotions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPromotions_PromotionId",
                table: "CustomerPromotions",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TierId",
                table: "Customers",
                column: "TierId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionRef",
                table: "Payments",
                column: "TransactionRef",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PointHistories_BookingId",
                table: "PointHistories",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_PointHistories_PointId_CreatedAt",
                table: "PointHistories",
                columns: new[] { "PointId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PointHistories_RewardId",
                table: "PointHistories",
                column: "RewardId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_UserId",
                table: "Points",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromotionBranches_BranchId",
                table: "PromotionBranches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionBranches_PromotionId",
                table: "PromotionBranches",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Code",
                table: "Promotions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RewardRedemptions_BookingId",
                table: "RewardRedemptions",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardRedemptions_CustomerId",
                table: "RewardRedemptions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardRedemptions_RewardId",
                table: "RewardRedemptions",
                column: "RewardId");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_WashPackageId",
                table: "Rewards",
                column: "WashPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_BranchId",
                table: "Staffs",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_UserId",
                table: "Staffs",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TierPromotions_PromotionId",
                table: "TierPromotions",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_TierPromotions_TierId",
                table: "TierPromotions",
                column: "TierId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BookingId",
                table: "Transactions",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionCode",
                table: "Transactions",
                column: "TransactionCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CustomerId",
                table: "Vehicles",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_LicensePlate",
                table: "Vehicles",
                column: "LicensePlate",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_WashBays_BranchId",
                table: "WashBays",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingAddOns");

            migrationBuilder.DropTable(
                name: "CustomerPromotions");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PointHistories");

            migrationBuilder.DropTable(
                name: "PromotionBranches");

            migrationBuilder.DropTable(
                name: "RewardRedemptions");

            migrationBuilder.DropTable(
                name: "TierPromotions");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "AddOns");

            migrationBuilder.DropTable(
                name: "Points");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "BranchTimeSlots");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "WashBays");

            migrationBuilder.DropTable(
                name: "TimeSlots");

            migrationBuilder.DropTable(
                name: "WashPackages");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Tiers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
