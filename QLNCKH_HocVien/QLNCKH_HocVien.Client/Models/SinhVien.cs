using System.ComponentModel.DataAnnotations;

namespace QLNCKH_HocVien.Client.Models
{
    public class SinhVien
    {
        public int Id { get; set; } // ID nội bộ để quản lý

        [Required(ErrorMessage = "Vui lòng nhập Mã SV")]
        public string MaSV { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ tên")]
        public string HoTen { get; set; }

        public string GioiTinh { get; set; } = "Nam";
        public DateTime? NgaySinh { get; set; } = DateTime.Now;

        // Thêm dấu ? vào sau string để cho phép để trống
        // Quê quán
        public int? IdTinh { get; set; }
        public int? IdXa { get; set; }

        // Thông tin khác
        public int? IdDanToc { get; set; }
        public int? IdTonGiao { get; set; }
        public string? SoDienThoai { get; set; }

        // Học tập
        public string? Lop { get; set; }
        public int? IdChucVu { get; set; } // Chức vụ trong lớp
        public int? IdNganh { get; set; } // Ngành học
        public string? ChuyenNganh { get; set; }
    }
}