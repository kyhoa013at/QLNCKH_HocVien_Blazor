namespace QLNCKH_HocVien.Client.Models
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string Role { get; set; } = "SinhVien";
        public int? IdSinhVien { get; set; }
        public int? IdGiaoVien { get; set; }
    }

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

    public class UserInfo
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public int? IdSinhVien { get; set; }
        public int? IdGiaoVien { get; set; }
    }
}