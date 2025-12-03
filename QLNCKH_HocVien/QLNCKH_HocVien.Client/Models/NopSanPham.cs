using System.ComponentModel.DataAnnotations;

namespace QLNCKH_HocVien.Client.Models
{
    // Tạo Enum cho trạng thái nộp
    public enum TrangThaiNop
    {
        NopBanMem = 1,
        NopBanCung = 2,
        NopSauChinhSua = 3
    }

    public class NopSanPham
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Chuyên đề")]
        public int IdChuyenDe { get; set; } // Khóa ngoại liên kết bảng ChuyenDeNCKH

        public TrangThaiNop TrangThai { get; set; } = TrangThaiNop.NopBanMem;

        public DateTime NgayNop { get; set; } = DateTime.Now;

        public string? GhiChu { get; set; } // Link driver hoặc ghi chú thêm
    }
}