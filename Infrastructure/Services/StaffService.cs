using Application.DTOs.Staff;
using Application.DTOs.WashBay;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StaffService : IStaffService
{
    private readonly IApplicationDbContext _context;

    public StaffService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StaffProfileResponse> GetProfileByUserIdAsync(Guid userId)
    {
        var staff = await _context.Staffs
                        .Include(s => s.User)
                        .Include(s => s.Branch)
                        .FirstOrDefaultAsync(s => s.UserId == userId)
                    ?? throw new AppException("Staff profile not found.", 404);

        return MapToResponse(staff);
    }

    public async Task<StaffProfileResponse> GetStaffByIdAsync(Guid staffId)
    {
        var staff = await _context.Staffs
                        .Include(s => s.User)
                        .Include(s => s.Branch)
                        .FirstOrDefaultAsync(s => s.Id == staffId)
                    ?? throw new AppException("Staff member not found.", 404);

        return MapToResponse(staff);
    }

    public async Task<StaffProfileResponse> CreateStaffAsync(CreateStaffRequest request)
    {
        // 1. Check trùng Email trong hệ thống
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new AppException("Email này đã được sử dụng trong hệ thống.", 409);

        // 2. Check chi nhánh (Branch) có tồn tại thực hay không
        var branch = await _context.Branches.FindAsync(request.BranchId)
                     ?? throw new AppException("Chi nhánh được chỉ định không tồn tại.", 404);

        // 3. Khởi tạo đối tượng User làm tài khoản đăng nhập cho Staff
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Staff, // Ép cứng Role là Staff luôn
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // 4. Khởi tạo thực thể Staff nối với User và Branch
        var staff = new Staff()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            BranchId = branch.Id,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            IsAvailable = true
        };

        // Gán ngược reference nếu cần thiết cho EF Core theo dõi trạng thái thực thể chặt chẽ hơn
        staff.User = user; 
        staff.Branch = branch;

        // 5. Lưu vào Database thông qua Unit of Work / DBContext
        _context.Users.Add(user);
        _context.Staffs.Add(staff);
        await _context.SaveChangesAsync();

        // 6. Map kết quả trả về cho Client
        return MapToResponse(staff);
    }
    
    public async Task<List<WashBayResponse>> GetWashBaysByBranchIdAsync(Guid branchId)
    {
        // Kiểm tra Branch có tồn tại không (Tuỳ chọn, giúp báo lỗi rõ ràng hơn)
        var branchExists = await _context.Branches.AnyAsync(b => b.Id == branchId);
        if (!branchExists)
            throw new AppException("Chi nhánh không tồn tại.", 404);

        var washBays = await _context.WashBays
            .AsNoTracking() // Dùng AsNoTracking để tối ưu hiệu suất khi chỉ đọc dữ liệu
            .Where(wb => wb.BranchId == branchId)
            .ToListAsync();

        return washBays.Select(MapToResponse).ToList();
    }

    /// <summary>
    /// Xử lý sự kiện khi Staff kéo 1 Booking vào WashBay
    /// </summary>
    public async Task AssignBookingToWashBayAsync(Guid washBayId, Guid bookingId)
    {
        // 1. Tìm WashBay
        var washBay = await _context.WashBays.FindAsync(washBayId)
                      ?? throw new AppException("Không tìm thấy khu vực rửa xe (WashBay).", 404);

        // 2. Không cho phép nhận Booking nếu đang bảo trì
        if (washBay.Status == WashBayStatus.Maintenance)
            throw new AppException("Khu vực rửa xe này đang được bảo trì, không thể nhận thêm xe.", 400);

        // 3. Đổi trạng thái WashBay sang InProgress
        // (Do 1 WashBay có thể nhận nhiều booking, nên dù đang Available hay InProgress, cứ có booking mới vào thì chắc chắn nó phải là InProgress)
        washBay.Status = WashBayStatus.InProgress;

        // 4. Tìm Booking và gán WashBayId cho nó (Giả định bạn có thực thể Booking)
        /* var booking = await _context.Bookings.FindAsync(bookingId)
            ?? throw new AppException("Không tìm thấy Booking.", 404);

        booking.WashBayId = washBay.Id;
        booking.Status = BookingStatus.Processing; // Hoặc trạng thái tương ứng của bạn
        */

        // 5. Lưu thay đổi
        await _context.SaveChangesAsync();
    }
    
    private static WashBayResponse MapToResponse(WashBay washBay)
    {
        return new WashBayResponse
        {
            Id = washBay.Id,
            Name = washBay.Name,
            Status = washBay.Status.ToString(),
            SupportedTypes = string.IsNullOrWhiteSpace(washBay.SupportedTypes) 
                ? new List<string>() 
                : washBay.SupportedTypes
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(type => type.Trim())
                    .ToList(),
            CreatedAt = washBay.CreatedAt,
            BranchId = washBay.BranchId
        };
    }
    
    // Hàm Helper chuyển đổi dữ liệu từ Entity sang DTO an toàn
    private static StaffProfileResponse MapToResponse(Domain.Entities.Staff staff)
    {
        return new StaffProfileResponse
        {
            Id = staff.Id,
            UserId = staff.UserId,
            Email = staff.User.Email ?? "No Email",
            FullName = staff.FullName,
            PhoneNumber = staff.PhoneNumber,
            IsAvailable = staff.IsAvailable,
            Role = staff.User.Role.ToString(),
            Branch = new StaffBranchResponse
            {
                Id = staff.Branch.Id,
                BranchName = staff.Branch.BranchName,
                Address = staff.Branch.Address,
                Hotline = staff.Branch.Hotline
            }
        };
    }
}