using System.ComponentModel.DataAnnotations;

namespace QLNCKH_HocVien.Client.Models
{
    public class GiaoVien
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã số CB là bắt buộc")]
        public string MaSoCB { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string HoTen { get; set; }

        public string GioiTinh { get; set; } = "Nam";
        public DateTime? NgaySinh { get; set; } = DateTime.Now;

        // Quê quán (Tỉnh/Xã)
        public int? IdTinh { get; set; }
        public int? IdXa { get; set; }

        // Thông tin chung
        public int? IdDanToc { get; set; }
        public int? IdTonGiao { get; set; }
        public string? SoDienThoai { get; set; }

        // Chuyên môn & Công tác
        public int? IdTrinhDoChuyenMon { get; set; }
        public int? IdTrinhDoLLCT { get; set; }
        public int? IdDonViCongTac { get; set; } // Map với ToChuc
        public int? IdChucVu { get; set; }
        public int? IdCapBac { get; set; }
        
        public double? HeSoLuong { get; set; } // Nhập số

        public int? IdChucDanh { get; set; }
        public int? IdHocHam { get; set; }
        public int? IdHocVi { get; set; } // Map với TrinhDoDaoTao

        public string? LinhVuc { get; set; } // Lĩnh vực giảng dạy/chuyên môn
    }
}