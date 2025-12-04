namespace QLNCKH_HocVien.Models
{
    // Cấu hình JWT từ appsettings
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; } = 60;
    }

    // DTO cho Login Request
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // DTO cho Register Request
    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string Role { get; set; } = AppRoles.SinhVien; // Mặc định là SinhVien

        // Tùy chọn: Liên kết với SinhVien hoặc GiaoVien
        public int? IdSinhVien { get; set; }
        public int? IdGiaoVien { get; set; }
    }

    // DTO cho Response sau khi Login/Register thành công
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? HoTen { get; set; }
        public int? IdSinhVien { get; set; }
        public int? IdGiaoVien { get; set; }
    }
}