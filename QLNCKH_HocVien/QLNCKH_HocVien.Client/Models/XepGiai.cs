using System.ComponentModel.DataAnnotations;

namespace QLNCKH_HocVien.Client.Models
{
    public class XepGiai
    {
        public int Id { get; set; }
        public int IdChuyenDe { get; set; } // Liên kết đề tài

        public double DiemTrungBinh { get; set; } // Lưu lại điểm chốt

        public string TenGiai { get; set; } = "Không đạt"; // Nhất, Nhì, Ba...
        public int XepHang { get; set; } // Thứ hạng (1, 2, 3...)
    }
}