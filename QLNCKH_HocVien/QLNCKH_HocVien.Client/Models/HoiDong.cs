using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QLNCKH_HocVien.Client.Models
{
    public enum VongCham
    {
        SoLoai = 1,
        ChungKhao = 2
    }

    public class HoiDong
    {
        public int Id { get; set; }

        public int IdChuyenDe { get; set; } // Chấm cho chuyên đề nào

        public VongCham VongThi { get; set; } = VongCham.SoLoai;

        public DateTime NgayCham { get; set; } = DateTime.Now;

        public string? DiaDiem { get; set; }

        // Danh sách thành viên (Để hứng dữ liệu từ API)
        public List<ThanhVienHoiDong> ThanhViens { get; set; } = new();
    }

    public class ThanhVienHoiDong
    {
        public int Id { get; set; }
        public int IdHoiDong { get; set; }
        public int IdGiaoVien { get; set; }
        public string VaiTro { get; set; } = "Ủy viên"; // VD: Người chấm, Chủ tịch, Thư ký...
    }
}