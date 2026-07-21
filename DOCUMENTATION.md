# Tài liệu tổng hợp dự án - AutoWash Pro Backend

## Mục lục
1. [Tổng quan dự án](#1-tổng-quan-dự-án)
2. [Cấu trúc thư mục](#2-cấu-trúc-thư-mục)
3. [Chức năng chi tiết](#3-chức-năng-chi-tiết)
4. [Luồng chính](#4-luồng-chính)
5. [Database Entities & Relationships](#5-database-entities--relationships)
6. [Phân quyền](#6-phân-quyền)
7. [Seed Data](#7-seed-data)
8. [Chưa triển khai / Cần cải thiện](#8-chưa-triển-khai--cần-cải-thiện)
9. [Files quan trọng](#9-files-quan-trọng)

---

## 1. Tổng quan dự án

**AutoWash Pro** là hệ thống backend cho ứng dụng đặt lịch rửa xe tự động kết hợp chương trình khách hàng thân thiết (loyalty). Hệ thống phục vụ 3 vai trò: **Customer** (đặt lịch, quản lý xe), **Staff** (xử lý booking tại cửa hàng), và **Admin** (quản lý hệ thống).

### Tech Stack

| Thành phần | Công nghệ |
|---|---|
| Framework | .NET 8.0 / ASP.NET Core |
| Database | SQL Server + Entity Framework Core 8.0.11 |
| Authentication | JWT Bearer (60 phút) + Refresh Token (7 ngày) |
| Phone OTP | Firebase Authentication SDK 3.5.0 |
| Social Login | Google OAuth 2.0 (Google.Apis 1.74.0) |
| Password | BCrypt.Net-Next 4.0.3 |
| Validation | FluentValidation 11.9.0 |
| API Docs | Swashbuckle/Swagger 6.5.0 |
| Architecture | Clean Architecture (4 layers) |

---

## 2. Cấu trúc thư mục

```
Booking-Loyalty-System-BE/
├── API/                           # Entry point - Controllers, Middleware
│   ├── Controllers/               # 9 controllers
│   │   ├── AuthController.cs
│   │   ├── BookingController.cs
│   │   ├── CustomerController.cs
│   │   ├── VehicleController.cs
│   │   ├── WashPackageController.cs
│   │   ├── StaffBookingController.cs
│   │   ├── AdminUserController.cs
│   │   ├── AdminWashBayController.cs
│   │   └── AdminWashPackageController.cs
│   ├── Middleware/
│   │   └── ExceptionMiddleware.cs  # Global exception handler
│   ├── Program.cs                  # App startup, JWT config, DI
│   └── appsettings.json            # Connection string, JWT, Google keys
│
├── Application/                    # Business logic layer
│   ├── Common/
│   │   └── ApiResponse.cs          # Standard response wrapper {success, message, data}
│   ├── DTOs/                       # Data Transfer Objects (Auth, Booking, Customer, Staff, Vehicle, WashBay, WashPackage, Admin)
│   ├── Exceptions/
│   │   └── AppException.cs         # Custom exception với HTTP status code
│   ├── Interfaces/                 # 11 service interfaces
│   ├── Validators/                 # FluentValidation rules
│   └── DependencyInjection.cs
│
├── Domain/                         # Pure domain model (no dependencies)
│   ├── Entities/                   # 9 entities (User, Customer, Vehicle, Booking, WashPackage, WashBay, TimeSlot, Tier, Branch)
│   └── Enums/                      # 7 enums (UserRole, BookingStatus, VehicleType, WashBayStatus, TimeSlotStatus, BranchStatus, PriorityLevel)
│
├── Infrastructure/                 # Data access & external services
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs  # EF Core DbContext + Seed Data
│   │   └── Configurations/         # 10 entity type configurations
│   ├── Services/                   # 10 service implementations
│   ├── Migrations/
│   └── DependencyInjection.cs
│
└── Tests/                          # Unit tests
```

---

## 3. Chức năng chi tiết

### 3.1. Auth (Xác thực)
**Controller:** `api/auth`

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| POST | `/register` | Không | Đăng ký tài khoản Customer (tạo User + Customer + Vehicle nếu có) |
| POST | `/login` | Không | Đăng nhập bằng email/password, trả về AccessToken + RefreshToken |
| POST | `/refresh-token` | Không | Refresh access token (token rotation) |
| PUT | `/change-password` | Có | Đổi mật khẩu, xóa tất cả refresh token |
| POST | `/logout` | Có | Đăng xuất, xóa refresh token |
| GET | `/me` | Có | Lấy thông tin user hiện tại (UserId, Email, Role, Tier, Points, TotalWashes) |
| POST | `/google-login` | Không | Đăng nhập Google OAuth, tự tạo tài khoản nếu mới |
| POST | `/send-otp` | Không | Gửi OTP qua SMS (rate limit: 60s cooldown, 3 lần/giờ) |
| POST | `/verify-otp` | Không | Xác minh Firebase IdToken, trích xuất số điện thoại |

### 3.2. Booking - Customer (Đặt lịch)
**Controller:** `api/bookings` — Yêu cầu đăng nhập

| Method | Endpoint | Mô tả |
|---|---|---|
| POST | `/` | Tạo booking mới (tự động gán wash bay, tạo booking code 6 ký tự) |
| GET | `/{id}` | Xem chi tiết booking (chỉ chủ sở hữu) |
| GET | `/my-bookings` | Danh sách booking của customer |
| PUT | `/{id}/cancel` | Hủy booking (chỉ khi Pending/Confirmed) |

**Business rules:**
- Không đặt ngày trong quá khứ
- Booking window theo tier: Member 7 ngày, Silver 10, Gold 12, Platinum 14 ngày
- Wash bay tự động gán khi tạo booking
- Trạng thái khởi tạo: **Pending**

### 3.3. Booking - Staff (Xử lý booking)
**Controller:** `api/staff/bookings` — Yêu cầu role Staff hoặc Admin

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/today` | Danh sách booking hôm nay (filter theo branchId) |
| GET | `/search` | Tìm booking theo mã code |
| PUT | `/{id}/confirm` | Xác nhận booking: Pending → Confirmed |
| PUT | `/{id}/start` | Bắt đầu rửa: Confirmed → InProgress (bay → InProgress) |
| PUT | `/{id}/complete` | Hoàn thành: InProgress → Completed (tính điểm + đánh giá tier) |
| PUT | `/{id}/cancel` | Hủy booking (có lý do) |

### 3.4. Loyalty (Khách hàng thân thiết)
Tích hợp trong `StaffBookingService.CompleteWashAsync()`:

- **Tính điểm:** `(TotalPrice / 1000) × TierPointRate`
- **Cập nhật:** TotalPoints, LifetimePoints, TotalWashes, TotalSpent
- **Auto upgrade:** Khi LifetimePoints ≥ MinPointsRequired của tier cao hơn
- **Auto downgrade:** Khi điểm 90 ngày gần nhất < MaintenancePoints của tier hiện tại

| Tier | Point Rate | Booking Window | Min Points | Maintenance (90 ngày) |
|---|---|---|---|---|
| Member | ×1.0 | 7 ngày | 0 | 0 |
| Silver | ×1.2 | 10 ngày | 500 | 300 |
| Gold | ×1.5 | 12 ngày | 1,500 | 1,000 |
| Platinum | ×2.0 | 14 ngày | 5,000 | 3,000 |

### 3.5. Vehicle (Quản lý xe)
**Controller:** `api/vehicles` — Yêu cầu đăng nhập

| Method | Endpoint | Mô tả |
|---|---|---|
| POST | `/` | Thêm xe (tối đa 5 xe, biển số không trùng, xe đầu tiên tự đặt primary) |
| GET | `/` | Danh sách xe (ẩn xe đã xóa, sắp xếp theo primary + ngày tạo) |
| DELETE | `/{id}` | Xóa mềm xe (không xóa xe cuối cùng, không xóa nếu có booking active) |

**Vehicle types:** Small, Medium, Large

### 3.6. Profile (Hồ sơ khách hàng)
**Controller:** `api/customers` — Yêu cầu đăng nhập

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/me` | Xem hồ sơ (bao gồm Tier, Points) |
| PUT | `/me` | Cập nhật: FullName, PhoneNumber, DateOfBirth |

### 3.7. Admin - Users (Quản lý người dùng)
**Controller:** `api/admin/users` — Yêu cầu role Admin

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/` | Danh sách users (filter theo role: Customer/Staff/Admin) |
| PUT | `/{id}/status` | Kích hoạt/Vô hiệu hóa tài khoản (không áp dụng cho Admin) |

### 3.8. Admin - Packages (Quản lý gói rửa)
**Controller:** `api/admin/wash-packages` — Yêu cầu role Admin

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/` | Danh sách tất cả packages (bao gồm inactive) |
| GET | `/{id}` | Chi tiết package |
| POST | `/` | Tạo package mới (Name, Price, Duration, Features, VehicleType) |
| PUT | `/{id}` | Cập nhật package (partial update) |
| DELETE | `/{id}` | Vô hiệu hóa package (soft: IsActive = false) |

### 3.9. Admin - Bays (Quản lý khoang rửa)
**Controller:** `api/admin/wash-bays` — Yêu cầu role Admin

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/` | Danh sách wash bays |
| GET | `/{id}` | Chi tiết bay |
| POST | `/` | Tạo bay mới (Name, SupportedTypes[]) |
| PUT | `/{id}` | Cập nhật bay (Name, Status, SupportedTypes) |
| DELETE | `/{id}` | Xóa bay (hard delete) |

**Wash Package công khai** (không cần đăng nhập):

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `api/wash-packages` | Danh sách packages active (sắp theo giá) |
| GET | `api/wash-packages/{id}` | Chi tiết package |

---

## 4. Luồng chính

### 4.1. Đăng ký & Đăng nhập

```
[Đăng ký]
Client → POST /api/auth/register (Email, Password, FullName, ...)
  → Validate email/phone unique
  → Tạo User (role: Customer, hash password bằng BCrypt)
  → Tạo Customer (gán Member tier)
  → Tạo Vehicle (nếu có LicensePlate)
  → Generate AccessToken (JWT 60 phút) + RefreshToken (7 ngày)
  → Response: TokenResponse

[Đăng nhập]
Client → POST /api/auth/login (Email, Password)
  → BCrypt verify password
  → Kiểm tra IsActive
  → Generate AccessToken + RefreshToken mới (token rotation)
  → Response: TokenResponse

[Refresh Token]
Client → POST /api/auth/refresh-token (RefreshToken)
  → Validate token chưa hết hạn
  → Generate cặp token mới (rotation)
  → Response: TokenResponse
```

### 4.2. Đặt lịch (Customer)

```
Customer → POST /api/bookings
  {VehicleId, WashPackageId, BookingDate, StartTime}

  1. Validate customer tồn tại
  2. Validate vehicle thuộc customer
  3. Validate package active
  4. Kiểm tra ngày không trong quá khứ
  5. Kiểm tra booking window theo tier
  6. Tự động gán wash bay available
  7. Generate booking code (6 ký tự alphanumeric)
  8. Tạo booking với status = Pending

  → Response: BookingResponse (với BookingCode, BayName, PackageName, ...)
```

### 4.3. Staff xử lý booking

```
                    ┌─────────┐
                    │ Pending │ ← Tạo mới
                    └────┬────┘
                         │ Staff confirm
                    ┌────▼─────┐
                    │Confirmed │
                    └────┬─────┘
                         │ Staff start (bay → InProgress)
                   ┌─────▼──────┐
                   │ InProgress  │
                   └─────┬──────┘
                         │ Staff complete
                  ┌──────▼───────┐
                  │  Completed   │ → Tính điểm + Đánh giá tier
                  └──────────────┘

  * Cancelled có thể từ Pending hoặc Confirmed (bởi Customer hoặc Staff)

[Complete flow chi tiết]
  1. Booking status → Completed
  2. Wash bay status → Available
  3. Tính points: (TotalPrice / 1000) × TierPointRate
  4. Cộng: TotalPoints, LifetimePoints, TotalWashes, TotalSpent
  5. Đánh giá tier:
     - UPGRADE: Tìm tier cao nhất mà LifetimePoints ≥ MinPointsRequired
     - DOWNGRADE: Kiểm tra điểm 90 ngày < MaintenancePoints → hạ tier
```

### 4.4. Loyalty & Tier

```
[Tích điểm]
  Mỗi lần complete booking:
  Points = (TotalPrice / 1000) × TierPointRate

  Ví dụ: Premium Wash (120,000₫) + Gold tier (×1.5)
  → (120,000 / 1000) × 1.5 = 180 points

[Auto Upgrade]
  Sau mỗi complete → kiểm tra LifetimePoints
  Nếu LifetimePoints ≥ MinPointsRequired của tier cao hơn → upgrade

  Ví dụ: LifetimePoints = 1,600 ≥ Gold.MinPointsRequired (1,500) → upgrade Gold

[Auto Downgrade]
  Sau mỗi complete → tính tổng điểm 90 ngày gần nhất
  Nếu 90-day points < CurrentTier.MaintenancePoints → downgrade

  Ví dụ: Gold tier, 90-day points = 800 < Gold.MaintenancePoints (1,000) → downgrade Silver
```

### 4.5. OTP & Google Login

```
[OTP - Xác minh số điện thoại]
  1. Client → POST /api/auth/send-otp {PhoneNumber}
     → Rate limit: 60s cooldown, 3 lần/giờ (in-memory cache)
     → Backend trả success (Firebase SDK gửi OTP ở frontend)

  2. User nhập OTP trên app → Firebase verify → nhận IdToken

  3. Client → POST /api/auth/verify-otp {OtpCode: IdToken, PhoneNumber}
     → Backend verify IdToken với Firebase (Google signature)
     → Extract phone_number claim (+84...)
     → Match phone number → cập nhật IsPhoneNumberVerified = true

[Google Login]
  1. Client → Google OAuth → nhận authorization code
  2. Client → POST /api/auth/google-login {code}
     → Backend đổi code → Google token → lấy user info
     → Nếu user mới: tạo User + Customer (không password, gán GoogleId)
     → Nếu user cũ: cập nhật info
     → Generate JWT + RefreshToken
     → Response: TokenResponse
```

---

## 5. Database Entities & Relationships

### Entity Relationship Diagram (Text)

```
  User ──1:1──> Customer ──1:N──> Vehicle
                    │                  │
                    │ N:1              │ N:1
                    ▼                  ▼
                  Tier              Booking <──N:1── WashPackage
                                     │
                              N:1 ┌──┴──┐ N:1
                                  ▼     ▼
                               WashBay  Branch
                                  │
                               1:N│
                                  ▼
                              TimeSlot
```

### Chi tiết các Entity

#### User
| Field | Type | Notes |
|---|---|---|
| Id | Guid (PK) | |
| Email | string(256) | Unique index |
| PasswordHash | string? | Null cho OAuth users |
| Role | UserRole enum | Customer, Staff, Admin |
| IsActive | bool | Default: true |
| RefreshToken | string?(256) | |
| RefreshTokenExpiry | DateTime? | |
| GoogleId | string? | Google OAuth ID |
| CreatedAt | DateTime | UTC |
| UpdatedAt | DateTime? | |

#### Customer
| Field | Type | Notes |
|---|---|---|
| Id | Guid (PK) | |
| UserId | Guid (FK→User) | 1:1, cascade delete |
| FullName | string(100) | Required |
| PhoneNumber | string?(15) | Unique index |
| IsPhoneNumberVerified | bool | Default: false |
| DateOfBirth | DateTime? | |
| TierId | Guid (FK→Tier) | Restrict delete |
| TotalPoints | int | Điểm hiện tại |
| LifetimePoints | int | Tổng điểm tích lũy (dùng tính tier) |
| TotalWashes | int | |
| TotalSpent | decimal | Tổng chi tiêu |
| CreatedAt | DateTime | UTC |

#### Vehicle
| Field | Type | Notes |
|---|---|---|
| Id | Guid (PK) | |
| CustomerId | Guid (FK→Customer) | Cascade delete |
| LicensePlate | string | Required, unique per customer |
| Type | VehicleType enum | Small, Medium, Large |
| IsPrimary | bool | Default: false |
| VehicleName | string? | |
| Brand | string? | |
| Model | string? | |
| Color | string? | |
| IsDeleted | bool | Soft delete, default: false |
| CreatedAt | DateTime | UTC |

#### Booking
| Field | Type | Notes |
|---|---|---|
| Id | Guid (PK) | |
| BookingCode | string(10) | Unique, auto 6 chars |
| CustomerId | Guid (FK→Customer) | Restrict delete |
| VehicleId | Guid (FK→Vehicle) | Restrict delete |
| WashPackageId | Guid (FK→WashPackage) | Restrict delete |
| TimeSlotId | Guid? (FK→TimeSlot) | Nullable |
| BayId | Guid (FK→WashBay) | Restrict delete |
| BranchId | Guid (FK→Branch) | Restrict delete |
| BookingDate | DateOnly | |
| StartTime | TimeOnly | |
| TotalPrice | decimal | |
| Status | BookingStatus enum | Pending → Confirmed → InProgress → Completed / Cancelled |
| QrData | string?(1000) | |
| CancellationReason | string?(500) | |
| PointsEarned | int | Default: 0 |
| CreatedAt | DateTime | UTC |
| UpdatedAt | DateTime? | |

#### WashPackage
| Field | Type | Notes |
|---|---|---|
| Id | Guid (PK) | |
| Name | string(100) | Required |
| Description | string?(500) | |
| Price | decimal | VND |
| DurationMinutes | int | |
| Features | string?(2000) | JSON serialized list |
| VehicleType | VehicleType? | Null = tất cả loại xe |
| IsActive | bool | Default: true |
| CreatedAt | DateTime | UTC |

#### WashBay
| Field | Type | Notes |
|---|---|---|
| Id | Guid (PK) | |
| Name | string | Required |
| Status | WashBayStatus enum | Available, InProgress, Maintenance |
| SupportedTypes | string | Comma-separated vehicle types |
| BranchId | Guid (FK→Branch) | Cascade delete |
| CreatedAt | DateTime | UTC |

#### TimeSlot
| Field | Type | Notes |
|---|---|---|
| Id | Guid (PK) | |
| WashBayId | Guid (FK→WashBay) | |
| Date | DateOnly | |
| StartTime | TimeOnly | |
| EndTime | TimeOnly | |
| Status | TimeSlotStatus enum | Available, Booked, InProgress, Completed |
| BookingId | Guid? (FK→Booking) | |

#### Branch
| Field | Type | Notes |
|---|---|---|
| Id | Guid (PK) | |
| BranchName | string(100) | Required |
| Address | string(255) | Required |
| Hotline | string(20) | |
| OperatingHours | string | Required |
| Status | BranchStatus enum | Active, Closed |

#### Tier
| Field | Type | Notes |
|---|---|---|
| Id | Guid (PK) | |
| TierName | string(50) | Required |
| PointRate | decimal | Hệ số nhân điểm |
| BookingWindow | int | Số ngày đặt trước |
| MinPointsRequired | int | Điểm tối thiểu để đạt tier |
| MaintenancePoints | int | Điểm cần trong 90 ngày để giữ tier |
| Level | PriorityLevel enum | Platinum=1, Gold=2, Silver=3, Member=4 |

### Enums

```csharp
UserRole:       Customer, Staff, Admin
BookingStatus:  Pending, Confirmed, InProgress, Completed, Cancelled
VehicleType:    Small, Medium, Large
WashBayStatus:  Available, InProgress, Maintenance
TimeSlotStatus: Available, Booked, InProgress, Completed
BranchStatus:   Active, Closed
PriorityLevel:  Platinum=1, Gold=2, Silver=3, Member=4
```

---

## 6. Phân quyền

### Role-based Access Control

| Nhóm | Endpoints | Roles |
|---|---|---|
| Auth public | register, login, refresh-token, google-login, send-otp, verify-otp | Không cần auth |
| Auth protected | change-password, logout, me | Tất cả roles (đã đăng nhập) |
| Customer | bookings/*, vehicles/*, customers/me | Customer |
| Staff | staff/bookings/* | Staff, Admin |
| Admin Users | admin/users/* | Admin |
| Admin Packages | admin/wash-packages/* | Admin |
| Admin Bays | admin/wash-bays/* | Admin |
| Public | wash-packages (GET) | Không cần auth |

### JWT Claims
- `NameIdentifier` (sub): User ID (Guid)
- `Email`: Email người dùng
- `Role`: Tên role (Customer/Staff/Admin)

### Token Configuration
- **AccessToken:** JWT, HMAC-SHA256, 60 phút
- **RefreshToken:** 64 random bytes → Base64, 7 ngày
- **Issuer:** AutoWashPro
- **Audience:** AutoWashProClient

---

## 7. Seed Data

### Users mặc định

| Email | Password | Role |
|---|---|---|
| admin@system.com | Admin@123 | Admin |
| staff@system.com | Staff@123 | Staff |
| customer@system.com | customer | Customer |

### Tiers

| Tier | Point Rate | Booking Window | Min Points | Maintenance (90 ngày) | Level |
|---|---|---|---|---|---|
| Member | ×1.0 | 7 ngày | 0 | 0 | 4 |
| Silver | ×1.2 | 10 ngày | 500 | 300 | 3 |
| Gold | ×1.5 | 12 ngày | 1,500 | 1,000 | 2 |
| Platinum | ×2.0 | 14 ngày | 5,000 | 3,000 | 1 |

### Wash Packages

| Tên | Giá (VND) | Thời gian | Features |
|---|---|---|---|
| Basic Wash | 80,000 | 20 phút | Exterior, Tires, Windows |
| Premium Wash | 120,000 | 30 phút | Exterior, Interior Vacuum, Dashboard Polish, Tire Shine |
| VIP Detailing | 200,000 | 45 phút | Full Exterior, Deep Interior, Wax, Leather Care, Engine, Ceramic |

### Branch

| Tên | Địa chỉ | Hotline | Giờ hoạt động | Trạng thái |
|---|---|---|---|---|
| Main Branch | 123 Street | 0123456789 | 8am-9pm | Active |

---

## 8. Chưa triển khai / Cần cải thiện

- **Payment integration:** Chưa có thanh toán online (VNPay, MoMo, ZaloPay)
- **Notification:** Chưa có push notification / email thông báo booking
- **Points redemption:** Chưa có chức năng đổi điểm thành voucher/giảm giá
- **Branch management UI:** Admin chưa có API CRUD đầy đủ cho Branch
- **Staff assignment:** Chưa gán staff cho branch cụ thể
- **Reporting/Analytics:** Chưa có báo cáo doanh thu, thống kê booking
- **CORS:** Đang để AllowAll - cần restrict cho production
- **Booking time validation:** Chưa validate thời gian trong operating hours của branch
- **Rating/Review:** Chưa có đánh giá dịch vụ sau khi hoàn thành
- **Promotion/Voucher:** Chưa có mã giảm giá
- **Image upload:** Chưa có upload ảnh xe/avatar

---

## 9. Files quan trọng

### Entry Point & Config
| File | Mô tả |
|---|---|
| `API/Program.cs` | Startup, middleware pipeline, JWT config, DI registration |
| `API/appsettings.json` | Connection string, JWT secrets, Google OAuth keys |
| `API/firebase-admin-key.json` | Firebase service account (không commit lên git) |

### Core Business Logic
| File | Mô tả |
|---|---|
| `Infrastructure/Services/AuthService.cs` | Đăng ký, đăng nhập, Google OAuth, token management |
| `Infrastructure/Services/BookingService.cs` | Tạo booking, cancel, validate business rules |
| `Infrastructure/Services/StaffBookingService.cs` | Xử lý booking flow + tính điểm loyalty + đánh giá tier |
| `Infrastructure/Services/TokenService.cs` | Generate JWT & Refresh Token |
| `Infrastructure/Services/FirebaseService.cs` | OTP verify qua Firebase IdToken |

### Database
| File | Mô tả |
|---|---|
| `Infrastructure/Persistence/ApplicationDbContext.cs` | DbContext, DbSets, Seed Data |
| `Infrastructure/Persistence/Configurations/` | Entity type configurations (10 files) |
| `Infrastructure/DependencyInjection.cs` | Register DbContext, services, Firebase |

### Domain Model
| File | Mô tả |
|---|---|
| `Domain/Entities/` | 9 entity classes (User, Customer, Vehicle, Booking, WashPackage, WashBay, TimeSlot, Tier, Branch) |
| `Domain/Enums/` | 7 enum types |

### API Response & Error Handling
| File | Mô tả |
|---|---|
| `Application/Common/ApiResponse.cs` | Standard response: `{success, message, data}` |
| `Application/Exceptions/AppException.cs` | Custom exception với HTTP status code |
| `API/Middleware/ExceptionMiddleware.cs` | Global exception → ApiResponse mapping |

### Interfaces (Contracts)
| File | Mô tả |
|---|---|
| `Application/Interfaces/` | 11 service interfaces (IAuthService, IBookingService, IStaffBookingService, ITokenService, IOtpService, IVehicleService, ICustomerService, IWashPackageService, IWashBayService, IAdminUserService) |
