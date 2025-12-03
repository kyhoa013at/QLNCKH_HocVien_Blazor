using System.Net.Http.Json;
using QLNCKH_HocVien.Client.Models;

namespace QLNCKH_HocVien.Client.Services
{
    public class KetQuaService
    {
        private readonly HttpClient _http;
        public KetQuaService(HttpClient http) { _http = http; }

        // Sơ loại
        public async Task<List<KetQuaSoLoai>> GetSoLoai()
            => await _http.GetFromJsonAsync<List<KetQuaSoLoai>>("api/KetQua/soloa-all") ?? new();

        public async Task SaveSoLoai(KetQuaSoLoai item)
            => await _http.PostAsJsonAsync("api/KetQua/soloa", item);

        public async Task AutoTop15()
            => await _http.PostAsync("api/KetQua/auto-top15", null);

        // Chung khảo
        public async Task<List<PhieuCham>> GetPhieuCham(int idCD)
            => await _http.GetFromJsonAsync<List<PhieuCham>>($"api/KetQua/phieucham/{idCD}") ?? new();

        public async Task SavePhieuCham(PhieuCham item)
            => await _http.PostAsJsonAsync("api/KetQua/phieucham", item);

        // Tham chiếu
        public async Task<List<ChuyenDeNCKH>> GetChuyenDe()
            => await _http.GetFromJsonAsync<List<ChuyenDeNCKH>>("api/ChuyenDeNCKH") ?? new();
        public async Task<List<HoiDong>> GetHoiDong()
            => await _http.GetFromJsonAsync<List<HoiDong>>("api/HoiDong") ?? new();
        public async Task<List<GiaoVien>> GetGiaoVien()
            => await _http.GetFromJsonAsync<List<GiaoVien>>("api/GiaoVien") ?? new();

        // Lấy TẤT CẢ phiếu chấm - Performance tốt hơn
        public async Task<List<PhieuCham>> GetAllPhieuCham()
            => await _http.GetFromJsonAsync<List<PhieuCham>>("api/KetQua/phieucham-all") ?? new();
    }
}