using System.ComponentModel.DataAnnotations;

namespace QLNCKH_HocVien.Client.Models
{
    public class ChuyenDeNCKH
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mã Chuyên đề")]
        public string MaSoCD { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Tên Chuyên đề")]
        public string TenChuyenDe { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Học viên thực hiện")]
        public int? IdHocVien { get; set; } // Khóa ngoại trỏ sang bảng SinhVien

        public int? IdLinhVuc { get; set; } // Lấy từ API LinhVucDeTai
    }
}