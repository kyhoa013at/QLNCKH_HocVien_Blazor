using QLNCKH_HocVien.Client.Models;
using System.Net.Http.Json;

namespace QLNCKH_HocVien.Client.Services
{
    public class GiaoVienService
    {
        private readonly HttpClient _http;

        public GiaoVienService(HttpClient http)
        {
            _http = http;
        }

        // --- API NỘI BỘ ---
        public async Task<List<GiaoVien>> LayDsGiaoVien() => await _http.GetFromJsonAsync<List<GiaoVien>>("api/GiaoVien") ?? new();

        public async Task ThemGiaoVien(GiaoVien gv)
        {
            var res = await _http.PostAsJsonAsync("api/GiaoVien", gv);
            if (!res.IsSuccessStatusCode)
            {
                var err = await res.Content.ReadAsStringAsync();
                throw new Exception(err);
            }
        }

        public async Task XoaGiaoVien(int id) => await _http.DeleteAsync($"api/GiaoVien/{id}");

        // --- API DANH MỤC (6PG.ORG) ---
        // Lưu ý: Tôi dùng link gốc vì bạn đang chạy InteractiveServer (Server gọi Server)

        public async Task<List<Tinh>> GetTinh() => await GetCatalog<List<Tinh>>("tinh");
        public async Task<List<DanToc>> GetDanToc() => await GetCatalog<List<DanToc>>("dantoc");
        public async Task<List<TonGiao>> GetTonGiao() => await GetCatalog<List<TonGiao>>("tongiao");

        public async Task<List<TrinhDoChuyenMon>> GetTDCM() => await GetCatalog<List<TrinhDoChuyenMon>>("trinhdochuyenmon");
        public async Task<List<TrinhDoLLCT>> GetTDLLCT() => await GetCatalog<List<TrinhDoLLCT>>("trinhdollct");
        public async Task<List<ToChuc>> GetToChuc() => await GetCatalog<List<ToChuc>>("tochuc");
        public async Task<List<ChucVu>> GetChucVu() => await GetCatalog<List<ChucVu>>("chucvu");
        public async Task<List<CapBac>> GetCapBac() => await GetCatalog<List<CapBac>>("capbac");
        public async Task<List<ChucDanh>> GetChucDanh() => await GetCatalog<List<ChucDanh>>("chucdanh");
        public async Task<List<HocHam>> GetHocHam() => await GetCatalog<List<HocHam>>("hocham");
        public async Task<List<HocVi>> GetHocVi() => await GetCatalog<List<HocVi>>("trinhdodaotao");

        // Hàm helper gọi API cho gọn
        private async Task<T> GetCatalog<T>(string endpoint)
        {
            var res = await _http.GetFromJsonAsync<ApiResponse<T>>($"http://apidanhmuc.6pg.org/api/{endpoint}/getall");
            return res.Data;
        }
    }
}