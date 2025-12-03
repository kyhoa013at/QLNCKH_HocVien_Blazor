using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Pages;
using QLNCKH_HocVien.Client.Services;
using QLNCKH_HocVien.Components;
using QLNCKH_HocVien.Client.Services;
using QLNCKH_HocVien.Data;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Cấu hình HttpClient trỏ về chính địa chỉ server đang chạy (Localhost)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(sp.GetRequiredService<NavigationManager>().BaseUri)
});

//Đăng ký Service cho Server
builder.Services.AddScoped<SinhVienService>();
builder.Services.AddScoped<GiaoVienService>();
builder.Services.AddScoped<ChuyenDeNCKHService>();
builder.Services.AddScoped<NopSanPhamService>();
builder.Services.AddScoped<HoiDongService>();
builder.Services.AddScoped<KetQuaService>();
builder.Services.AddScoped<XepGiaiService>();

// Đăng ký kết nối SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Đăng ký Controllers để tạo API nội bộ
builder.Services.AddControllers();
builder.Services.AddMudServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(QLNCKH_HocVien.Client._Imports).Assembly);

// Thêm dòng này để Server map được các Controller API
app.MapControllers();

app.Run();
