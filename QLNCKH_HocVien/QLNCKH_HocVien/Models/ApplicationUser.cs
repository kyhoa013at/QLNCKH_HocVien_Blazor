using Microsoft.AspNetCore.Identity;

namespace QLNCKH_HocVien.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Trường bổ sung: Liên kết với SinhVien hoặc GiaoVien
        public int? IdSinhVien { get; set; }
        public int? IdGiaoVien { get; set; }

        public string? HoTen { get; set; }
    }

    // Các vai trò trong hệ thống
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string GiaoVien = "GiaoVien";
        public const string SinhVien = "SinhVien";
    }
}