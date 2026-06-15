using Application.DTOs.Booking;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Services;
using MockQueryable.Moq;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class BugFixTests
{
    // ========== Bug 1: Booking should be created with Pending status ==========

    [Fact]
    public async Task CreateBooking_ShouldSetStatusToPending()
    {
        // Arrange
        var tier = new Tier
        {
            Id = Guid.NewGuid(),
            TierName = "Member",
            PointRate = 1,
            BookingWindow = 7,
            MinPointsRequired = 0,
            MaintenancePoints = 0
        };

        var userId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FullName = "Test User",
            TierId = tier.Id,
            Tier = tier
        };

        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            LicensePlate = "ABC-123",
            VehicleName = "Test Car",
            Type = VehicleType.Small,
            IsDeleted = false
        };

        var washPackage = new WashPackage
        {
            Id = Guid.NewGuid(),
            Name = "Basic Wash",
            Price = 50000m,
            DurationMinutes = 30,
            IsActive = true
        };

        var branch = new Branch
        {
            Id = Guid.NewGuid(),
            BranchName = "Branch 1",
            Address = "123 Test St",
            Hotline = "0901234567",
            OperatingHours = "8:00 - 18:00"
        };

        var washBay = new WashBay
        {
            Id = Guid.NewGuid(),
            Name = "Bay 1",
            Status = WashBayStatus.Available,
            SupportedTypes = "Sedan",
            BranchId = branch.Id,
            Branch = branch
        };

        var mockContext = new Mock<IApplicationDbContext>();

        // Setup Customers DbSet
        var customers = new List<Customer> { customer }.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Customers).Returns(customers.Object);

        // Setup Vehicles DbSet
        var vehicles = new List<Vehicle> { vehicle }.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Vehicles).Returns(vehicles.Object);

        // Setup WashPackages DbSet
        var packages = new List<WashPackage> { washPackage }.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.WashPackages).Returns(packages.Object);

        // Setup WashBays DbSet
        var bays = new List<WashBay> { washBay }.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.WashBays).Returns(bays.Object);

        // Setup Bookings DbSet - capture what gets added
        Booking? capturedBooking = null;
        var bookings = new List<Booking>().AsQueryable().BuildMockDbSet();
        bookings.Setup(b => b.Add(It.IsAny<Booking>()))
            .Callback<Booking>(b => capturedBooking = b);
        mockContext.Setup(c => c.Bookings).Returns(bookings.Object);

        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new BookingService(mockContext.Object);

        var request = new CreateBookingRequest
        {
            VehicleId = vehicle.Id,
            WashPackageId = washPackage.Id,
            BookingDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            StartTime = new TimeOnly(10, 0)
        };

        // Act
        var result = await service.CreateBookingAsync(userId, request);

        // Assert - Bug 1: Status must be Pending, not Confirmed
        Assert.NotNull(capturedBooking);
        Assert.Equal(BookingStatus.Pending, capturedBooking!.Status);
    }

    // ========== Bug 1 continued: Staff can confirm a Pending booking ==========

    [Fact]
    public async Task ConfirmBooking_WithPendingStatus_ShouldSucceed()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = CreateTestBooking(bookingId, BookingStatus.Pending);

        var mockContext = new Mock<IApplicationDbContext>();
        var bookings = new List<Booking> { booking }.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Bookings).Returns(bookings.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new StaffBookingService(mockContext.Object);

        // Act
        var result = await service.ConfirmBookingAsync(bookingId);

        // Assert - After confirm, status should be Confirmed
        Assert.Equal("Confirmed", result.Status);
    }

    [Fact]
    public async Task ConfirmBooking_WithConfirmedStatus_ShouldThrow()
    {
        // Arrange - simulate the OLD bug where booking was created as Confirmed
        var bookingId = Guid.NewGuid();
        var booking = CreateTestBooking(bookingId, BookingStatus.Confirmed);

        var mockContext = new Mock<IApplicationDbContext>();
        var bookings = new List<Booking> { booking }.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Bookings).Returns(bookings.Object);

        var service = new StaffBookingService(mockContext.Object);

        // Act & Assert - Confirming an already-Confirmed booking should fail
        var ex = await Assert.ThrowsAsync<AppException>(() => service.ConfirmBookingAsync(bookingId));
        Assert.Equal(400, ex.StatusCode);
        Assert.Contains("Pending", ex.Message);
    }

    // ========== Bug 3: SendOtpAsync should not throw ==========

    [Fact]
    public async Task SendOtpAsync_ShouldReturnTrue_NotThrow()
    {
        // Arrange
        var firebaseService = new FirebaseService();

        // Act & Assert - Bug 3: Should NOT throw NotImplementedException
        var result = await firebaseService.SendOtpAsync("+84901234567");
        Assert.True(result);
    }

    // ========== Bug 2: PointsEarned should be saved on CompleteWash ==========

    [Fact]
    public async Task CompleteWash_ShouldSavePointsEarned()
    {
        // Arrange
        var tier = new Tier
        {
            Id = Guid.NewGuid(),
            TierName = "Silver",
            PointRate = 2,
            BookingWindow = 14,
            MinPointsRequired = 100,
            MaintenancePoints = 50
        };

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            FullName = "Test User",
            TierId = tier.Id,
            Tier = tier,
            TotalPoints = 100,
            LifetimePoints = 100
        };

        var bookingId = Guid.NewGuid();
        var booking = new Booking
        {
            Id = bookingId,
            BookingCode = "ABC123",
            CustomerId = customer.Id,
            Customer = customer,
            VehicleId = Guid.NewGuid(),
            Vehicle = new Vehicle { Id = Guid.NewGuid(), CustomerId = customer.Id, VehicleName = "Car", LicensePlate = "XYZ", Type = VehicleType.Small },
            WashPackageId = Guid.NewGuid(),
            WashPackage = new WashPackage { Id = Guid.NewGuid(), Name = "Premium", Price = 100000m, DurationMinutes = 60 },
            BayId = Guid.NewGuid(),
            WashBay = new WashBay { Id = Guid.NewGuid(), Name = "Bay 1", Status = WashBayStatus.InProgress, SupportedTypes = "Sedan", BranchId = Guid.NewGuid(), Branch = new Branch { Id = Guid.NewGuid(), BranchName = "B1", Address = "Addr", Hotline = "123", OperatingHours = "8-18" } },
            BranchId = Guid.NewGuid(),
            Branch = new Branch { Id = Guid.NewGuid(), BranchName = "B1", Address = "Addr", Hotline = "123", OperatingHours = "8-18" },
            BookingDate = DateOnly.FromDateTime(DateTime.UtcNow),
            StartTime = new TimeOnly(10, 0),
            TotalPrice = 100000m,
            Status = BookingStatus.InProgress,
            PointsEarned = 0
        };

        var mockContext = new Mock<IApplicationDbContext>();

        var bookingsList = new List<Booking> { booking }.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Bookings).Returns(bookingsList.Object);

        var tiers = new List<Tier> { tier }.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Tiers).Returns(tiers.Object);

        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new StaffBookingService(mockContext.Object);

        // Act
        await service.CompleteWashAsync(bookingId);

        // Assert - Bug 2: PointsEarned should be saved on the booking
        // Price=100000, PointRate=2 → Floor(100000/1000 * 2) = 200
        Assert.Equal(200, booking.PointsEarned);
        Assert.Equal(BookingStatus.Completed, booking.Status);
    }

    [Fact]
    public void Booking_ShouldHavePointsEarnedProperty()
    {
        // Assert - Bug 2: Booking entity should have PointsEarned property
        var booking = new Booking();
        Assert.Equal(0, booking.PointsEarned);

        booking.PointsEarned = 150;
        Assert.Equal(150, booking.PointsEarned);
    }

    // ========== Full flow test: Pending → Confirmed → InProgress → Completed ==========

    [Fact]
    public async Task FullBookingFlow_PendingToCompleted_ShouldWork()
    {
        // Arrange
        var tier = new Tier
        {
            Id = Guid.NewGuid(),
            TierName = "Member",
            PointRate = 1,
            BookingWindow = 7,
            MinPointsRequired = 0,
            MaintenancePoints = 0
        };

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            FullName = "Flow Test User",
            TierId = tier.Id,
            Tier = tier
        };

        var bookingId = Guid.NewGuid();
        var booking = CreateTestBookingWithCustomer(bookingId, BookingStatus.Pending, customer, tier);

        var mockContext = new Mock<IApplicationDbContext>();
        var bookingsList = new List<Booking> { booking };
        var bookingsMock = bookingsList.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Bookings).Returns(bookingsMock.Object);
        mockContext.Setup(c => c.Tiers).Returns(new List<Tier> { tier }.AsQueryable().BuildMockDbSet().Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new StaffBookingService(mockContext.Object);

        // Act & Assert Step 1: Confirm (Pending → Confirmed)
        Assert.Equal(BookingStatus.Pending, booking.Status);
        await service.ConfirmBookingAsync(bookingId);
        Assert.Equal(BookingStatus.Confirmed, booking.Status);

        // Act & Assert Step 2: Start (Confirmed → InProgress)
        await service.StartWashAsync(bookingId);
        Assert.Equal(BookingStatus.InProgress, booking.Status);

        // Act & Assert Step 3: Complete (InProgress → Completed with PointsEarned)
        await service.CompleteWashAsync(bookingId);
        Assert.Equal(BookingStatus.Completed, booking.Status);
        Assert.True(booking.PointsEarned > 0, "PointsEarned should be calculated and saved");
    }

    // ========== Helpers ==========

    private static Booking CreateTestBooking(Guid bookingId, BookingStatus status)
    {
        var branch = new Branch { Id = Guid.NewGuid(), BranchName = "B1", Address = "Addr", Hotline = "123", OperatingHours = "8-18" };
        return new Booking
        {
            Id = bookingId,
            BookingCode = "TEST01",
            CustomerId = Guid.NewGuid(),
            Customer = new Customer { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), FullName = "Test", TierId = Guid.NewGuid(), Tier = new Tier { Id = Guid.NewGuid(), TierName = "Member", PointRate = 1, BookingWindow = 7 } },
            VehicleId = Guid.NewGuid(),
            Vehicle = new Vehicle { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), VehicleName = "Car", LicensePlate = "XYZ", Type = VehicleType.Small },
            WashPackageId = Guid.NewGuid(),
            WashPackage = new WashPackage { Id = Guid.NewGuid(), Name = "Basic", Price = 50000m, DurationMinutes = 30 },
            BayId = Guid.NewGuid(),
            WashBay = new WashBay { Id = Guid.NewGuid(), Name = "Bay 1", Status = WashBayStatus.Available, SupportedTypes = "Sedan", BranchId = branch.Id, Branch = branch },
            BranchId = branch.Id,
            Branch = branch,
            BookingDate = DateOnly.FromDateTime(DateTime.UtcNow),
            StartTime = new TimeOnly(10, 0),
            TotalPrice = 50000m,
            Status = status
        };
    }

    private static Booking CreateTestBookingWithCustomer(Guid bookingId, BookingStatus status, Customer customer, Tier tier)
    {
        var branch = new Branch { Id = Guid.NewGuid(), BranchName = "B1", Address = "Addr", Hotline = "123", OperatingHours = "8-18" };
        var washBay = new WashBay { Id = Guid.NewGuid(), Name = "Bay 1", Status = WashBayStatus.Available, SupportedTypes = "Sedan", BranchId = branch.Id, Branch = branch };
        return new Booking
        {
            Id = bookingId,
            BookingCode = "FLOW01",
            CustomerId = customer.Id,
            Customer = customer,
            VehicleId = Guid.NewGuid(),
            Vehicle = new Vehicle { Id = Guid.NewGuid(), CustomerId = customer.Id, VehicleName = "Car", LicensePlate = "XYZ", Type = VehicleType.Small },
            WashPackageId = Guid.NewGuid(),
            WashPackage = new WashPackage { Id = Guid.NewGuid(), Name = "Basic", Price = 50000m, DurationMinutes = 30 },
            BayId = washBay.Id,
            WashBay = washBay,
            BranchId = branch.Id,
            Branch = branch,
            BookingDate = DateOnly.FromDateTime(DateTime.UtcNow),
            StartTime = new TimeOnly(10, 0),
            TotalPrice = 50000m,
            Status = status
        };
    }
}
