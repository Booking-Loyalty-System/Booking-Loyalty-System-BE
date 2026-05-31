using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Vehicle> Vehicles { get; }
    DbSet<Service> Services { get; }
    DbSet<ServiceFeature> ServiceFeatures { get; }
    DbSet<Store> Stores { get; }
    DbSet<Booking> Bookings { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
