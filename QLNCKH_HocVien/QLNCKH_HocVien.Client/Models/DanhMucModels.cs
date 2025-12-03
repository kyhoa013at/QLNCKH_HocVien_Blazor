using System.Text.Json.Serialization;

namespace QLNCKH_HocVien.Client.Models
{
    // Cấu trúc chung API trả về
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class Tinh
    {
        public int IdTinh { get; set;}
        public string TenTinh { get; set;}
    }

    public class Xa
    {
        public int IdXa { get; set;}
        public string TenXa { get; set;}
        public int? IdTinh { get; set;}
    }

    public class DanToc
    {
        public int IdDanToc { get; set; }

        [JsonPropertyName("danToc")] // <-- Thêm dòng này để map chính xác với API
        public string TenDanToc { get; set; }
    }

    public class TonGiao
    {
        public int IdTonGiao { get; set; }

        [JsonPropertyName("tonGiao")] // <-- Thêm dòng này (API trả về "tonGiao")
        public string TenTonGiao { get; set; }
    }

    public class ChucVu
    {
        public int IdChucVu { get; set; }

        [JsonPropertyName("chucVu")] // <-- Thêm dòng này (API trả về "chucVu")
        public string TenChucVu { get; set; }
    }

    public class Nganh
    {
        public int IdLVNganh { get; set;}
        public string TenLVNganh { get; set;}
    }

    public class TrinhDoChuyenMon
    {
        [JsonPropertyName("idTDCM")] // Map đúng tên biến trong JSON
        public int Id { get; set; }

        [JsonPropertyName("trinhDoChuyenMon")]
        public string Ten { get; set; }
    }

    public class TrinhDoLLCT
    {
        [JsonPropertyName("idTDLLCT")] // Map đúng tên biến trong JSON
        public int Id { get; set; }

        [JsonPropertyName("trinhDoLLCT")]
        public string Ten { get; set; }
    }

    public class CapBac
    {
        [JsonPropertyName("idCapBac")]
        public int Id { get; set; }

        [JsonPropertyName("capBac")]
        public string Ten { get; set; }
    }

    public class ToChuc // Dùng cho Đơn vị công tác
    {
        [JsonPropertyName("idToChuc")]
        public int Id { get; set; }

        [JsonPropertyName("tenToChuc")]
        public string Ten { get; set; }
    }

    public class HocHam
    {
        [JsonPropertyName("idHocHam")]
        public int Id { get; set; }

        [JsonPropertyName("hocHam")]
        public string Ten { get; set; }
    }

    public class ChucDanh
    {
        [JsonPropertyName("idChucDanh")]
        public int Id { get; set; }

        [JsonPropertyName("chucDanh")]
        public string Ten { get; set; }
    }

    // Học vị dùng lại bảng TrinhDoDaoTao (đã có hoặc thêm mới nếu chưa)
    public class HocVi
    {
        [JsonPropertyName("idTrinhDoDaoTao")]
        public int Id { get; set; }

        [JsonPropertyName("tenTrinhDoDaoTao")]
        public string Ten { get; set; }
    }
    public class LinhVucDeTai
    {
        [JsonPropertyName("idLinhVucDeTai")]
        public int Id { get; set; }

        [JsonPropertyName("linhVucDeTai")]
        public string Ten { get; set; }
    }

    public class CapDonVi
    {
        [JsonPropertyName("idCap")]
        public int Id { get; set; }

        [JsonPropertyName("Cap")]
        public string Ten { get; set; }
    }
}