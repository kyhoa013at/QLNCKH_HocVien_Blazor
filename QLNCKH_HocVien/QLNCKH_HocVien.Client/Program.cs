using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using QLNCKH_HocVien.Client.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// 1. Cấu hình HttpClient
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://apidanhmuc.6pg.org/")
});

// 2. ✅ Đăng ký Authentication Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => 
    provider.GetRequiredService<CustomAuthStateProvider>());

// Thêm Authorization
builder.Services.AddAuthorizationCore();

// 3. Đăng ký các Services khác
builder.Services.AddScoped<SinhVienService>();
builder.Services.AddScoped<GiaoVienService>();
builder.Services.AddScoped<ChuyenDeNCKHService>();
builder.Services.AddScoped<NopSanPhamService>();
builder.Services.AddScoped<HoiDongService>();
builder.Services.AddScoped<KetQuaService>();
builder.Services.AddScoped<XepGiaiService>();

builder.Services.AddMudServices();

var host = builder.Build();

// ✅ Khởi tạo Authentication khi app start (restore token từ localStorage)
var authService = host.Services.GetRequiredService<AuthService>();
await authService.InitializeAuth();

await host.RunAsync();