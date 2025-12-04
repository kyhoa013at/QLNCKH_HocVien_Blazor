using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models; // Sử dụng Model SinhVien từ Client
using QLNCKH_HocVien.Models;


namespace QLNCKH_HocVien.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Đăng ký bảng SinhVien để EF Core quản lý
        public DbSet<SinhVien> SinhViens { get; set; }
        // Đăng ký bảng GiaoVien để EF Core quản lý
        public DbSet<GiaoVien> GiaoViens { get; set; }
        // Đăng ký bảng ChuyenDeNCKH để EF Core quản lý
        public DbSet<ChuyenDeNCKH> ChuyenDeNCKHs { get; set; }
        // Đăng ký bảng NopSanPham để EF Core quản lý
        public DbSet<NopSanPham> NopSanPhams { get; set; }
        // Đăng ký bảng HoiDong để EF Core quản lý
        public DbSet<HoiDong> HoiDongs { get; set; }
        // Đăng ký bảng ThanhVienHoiDong để EF Core quản lý
        public DbSet<ThanhVienHoiDong> ThanhVienHoiDongs { get; set; }
        // Đăng ký bảng KetQuaSoLoai để EF Core quản lý
        public DbSet<KetQuaSoLoai> KetQuaSoLoais { get; set; }
        // Đăng ký bảng PhieuCham để EF Core quản lý
        public DbSet<PhieuCham> PhieuChams { get; set; }
        // Đăng ký bảng XepGiai để EF Core quản lý
        public DbSet<XepGiai> XepGiais { get; set; }

    }
}