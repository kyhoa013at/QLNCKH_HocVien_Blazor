using System.ComponentModel.DataAnnotations;

namespace QLNCKH_HocVien.Client.Models
{
    // Model lưu kết quả vòng sơ loại (1 người chấm)
    public class KetQuaSoLoai
    {
        public int Id { get; set; }
        public int IdChuyenDe { get; set; }

        [Range(0, 10, ErrorMessage = "Điểm từ 0 đến 10")]
        public double DiemSo { get; set; }

        public bool KetQua { get; set; } // True = Đi tiếp, False = Loại
        public string? NhanXet { get; set; }
    }

    // Model phiếu chấm chi tiết của từng thành viên hội đồng vòng Chung khảo
    public class PhieuCham
    {
        public int Id { get; set; }
        public int IdChuyenDe { get; set; }
        public int IdGiaoVien { get; set; } // Người chấm

        [Range(0, 10, ErrorMessage = "Điểm từ 0 đến 10")]
        public double Diem { get; set; }

        public string? YKien { get; set; }
    }
}