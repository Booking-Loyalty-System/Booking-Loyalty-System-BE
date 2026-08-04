using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfirmSeedCustomerEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tài khoản demo customer@system.com bị sót IsEmailConfirmed nên không đăng nhập được
            // (403 "Please verify your email"), trong khi các user seed khác đều đã confirm.
            // Scaffold gốc còn kèm ~88 UpdateData churn (CreatedAt/PasswordHash sinh lại do
            // DateTime.UtcNow + BCrypt không tất định) — đã lược bỏ để không ghi đè mật khẩu user seed.
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "IsEmailConfirmed",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "IsEmailConfirmed",
                value: false);
        }
    }
}
