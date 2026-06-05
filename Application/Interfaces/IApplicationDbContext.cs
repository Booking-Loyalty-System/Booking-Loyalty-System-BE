using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Vehicle> Vehicles { get; }
    DbSet<WashPackage> WashPackages { get; }
    DbSet<WashBay> WashBays { get; }
    DbSet<TimeSlot> TimeSlots { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<Tier> Tiers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
