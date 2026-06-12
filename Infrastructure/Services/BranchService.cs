using Application.DTOs.Branch;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BranchService : IBranchService
{
    private readonly IApplicationDbContext _context;

    public BranchService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<BranchResponse>> GetAllAsync()
    {
        var branches = await _context.Branches
            .OrderBy(b => b.BranchName)
            .ToListAsync();

        return branches.Select(MapToResponse).ToList();
    }

    public async Task<BranchResponse?> GetByIdAsync(Guid id)
    {
        var branch = await _context.Branches.FindAsync(id);
        return branch == null ? null : MapToResponse(branch);
    }

    public async Task<BranchResponse> CreateAsync(CreateBranchRequest request)
    {
        var branch = new Branch
        {
            Id = Guid.NewGuid(),
            BranchName = request.BranchName,
            Address = request.Address,
            Hotline = request.Hotline,
            OperatingHours = request.OperatingHours,
            Status = BranchStatus.Active
        };

        _context.Branches.Add(branch);
        await _context.SaveChangesAsync();

        return MapToResponse(branch);
    }

    public async Task<BranchResponse> UpdateAsync(Guid id, UpdateBranchRequest request)
    {
        var branch = await _context.Branches.FindAsync(id)
            ?? throw new AppException("Branch not found.", 404);

        if (request.BranchName != null) branch.BranchName = request.BranchName;
        if (request.Address != null) branch.Address = request.Address;
        if (request.Hotline != null) branch.Hotline = request.Hotline;
        if (request.OperatingHours != null) branch.OperatingHours = request.OperatingHours;
        if (request.Status != null) branch.Status = Enum.Parse<BranchStatus>(request.Status);

        await _context.SaveChangesAsync();

        return MapToResponse(branch);
    }

    public async Task DeleteAsync(Guid id)
    {
        var branch = await _context.Branches.FindAsync(id)
            ?? throw new AppException("Branch not found.", 404);

        _context.Branches.Remove(branch);
        await _context.SaveChangesAsync();
    }

    private static BranchResponse MapToResponse(Branch branch)
    {
        return new BranchResponse
        {
            Id = branch.Id,
            BranchName = branch.BranchName,
            Address = branch.Address,
            Hotline = branch.Hotline,
            OperatingHours = branch.OperatingHours,
            Status = branch.Status.ToString()
        };
    }
}
