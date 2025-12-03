using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using QLNCKH_HocVien.Client; // Namespace gốc
using QLNCKH_HocVien.Client.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// 1. Cấu hình HttpClient (để gọi API Danh mục bên ngoài)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://apidanhmuc.6pg.org/")
});

// 2. Đăng ký Service SinhVien
// Đảm bảo bạn đã "using" đúng namespace chứa file Service
builder.Services.AddScoped<SinhVienService>();
builder.Services.AddScoped<GiaoVienService>();
builder.Services.AddScoped<ChuyenDeNCKHService>();
builder.Services.AddScoped<NopSanPhamService>();
builder.Services.AddScoped<HoiDongService>();
builder.Services.AddScoped<KetQuaService>();
builder.Services.AddScoped<XepGiaiService>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();