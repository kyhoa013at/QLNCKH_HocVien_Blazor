using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Data;
using QLNCKH_HocVien.Models;
using ClosedXML.Excel;
using System.Security.Claims;

namespace QLNCKH_HocVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // ✅ Yêu cầu đăng nhập
    public class SinhVienController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SinhVienController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Sinh viên chỉ xem được thông tin của mình, Admin/GiaoVien xem tất cả
        [HttpGet]
        public async Task<ActionResult<List<SinhVien>>> GetSinhViens()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role == AppRoles.SinhVien)
            {
                var idSinhVienClaim = User.FindFirst("IdSinhVien")?.Value;
                if (string.IsNullOrEmpty(idSinhVienClaim) || !int.TryParse(idSinhVienClaim, out int idSinhVien))
                {
                    return BadRequest("Không tìm thấy thông tin sinh viên");
                }

                // Chỉ trả về thông tin của chính sinh viên đó
                var sinhVien = await _context.SinhViens.FindAsync(idSinhVien);
                return sinhVien != null ? new List<SinhVien> { sinhVien } : new List<SinhVien>();
            }

            // Admin/GiaoVien xem tất cả
            return await _context.SinhViens.ToListAsync();
        }

        // ✅ Chỉ Admin mới được thêm sinh viên
        [HttpPost]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<ActionResult<SinhVien>> CreateSinhVien(SinhVien sv)
        {
            _context.SinhViens.Add(sv);
            await _context.SaveChangesAsync();
            return Ok(sv);
        }

        // ✅ Chỉ Admin mới được xóa
        [HttpDelete("{id}")]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> DeleteSinhVien(int id)
        {
            var sv = await _context.SinhViens.FindAsync(id);
            if (sv == null) return NotFound();

            _context.SinhViens.Remove(sv);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Sinh viên có thể cập nhật thông tin của mình, Admin cập nhật tất cả
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSinhVien(int id, SinhVien sv)
        {
            if (id != sv.Id) return BadRequest();

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            // Nếu là sinh viên, chỉ cho phép sửa thông tin của chính mình
            if (role == AppRoles.SinhVien)
            {
                var idSinhVienClaim = User.FindFirst("IdSinhVien")?.Value;
                if (string.IsNullOrEmpty(idSinhVienClaim) || !int.TryParse(idSinhVienClaim, out int idSinhVien))
                {
                    return Unauthorized();
                }

                if (id != idSinhVien)
                {
                    return Forbid(); // Không được sửa thông tin sinh viên khác
                }
            }

            var existingSv = await _context.SinhViens.FindAsync(id);
            if (existingSv == null) return NotFound();

            _context.Entry(existingSv).CurrentValues.SetValues(sv);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Xuất Excel - Admin và GiaoVien
        [HttpGet("export")]
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.GiaoVien}")]
        public async Task<IActionResult> ExportExcel()
        {
            var sinhViens = await _context.SinhViens.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Danh sách sinh viên");

            // Header
            worksheet.Cell(1, 1).Value = "STT";
            worksheet.Cell(1, 2).Value = "Mã SV";
            worksheet.Cell(1, 3).Value = "Họ và Tên";
            worksheet.Cell(1, 4).Value = "Ngày sinh";
            worksheet.Cell(1, 5).Value = "Giới tính";
            worksheet.Cell(1, 6).Value = "Lớp";
            worksheet.Cell(1, 7).Value = "SĐT";

            var headerRow = worksheet.Range("A1:G1");
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.LightBlue;
            headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            int stt = 1;
            foreach (var sv in sinhViens)
            {
                worksheet.Cell(row, 1).Value = stt++;
                worksheet.Cell(row, 2).Value = sv.MaSV;
                worksheet.Cell(row, 3).Value = sv.HoTen;
                worksheet.Cell(row, 4).Value = sv.NgaySinh?.ToString("dd/MM/yyyy") ?? "";
                worksheet.Cell(row, 5).Value = sv.GioiTinh;
                worksheet.Cell(row, 6).Value = sv.Lop ?? "";
                worksheet.Cell(row, 7).Value = sv.SoDienThoai ?? "";
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"DanhSachSinhVien_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}