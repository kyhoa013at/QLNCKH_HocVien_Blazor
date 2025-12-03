using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Data;
using ClosedXML.Excel;

namespace QLNCKH_HocVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SinhVienController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SinhVienController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SinhVien (Lấy danh sách)
        [HttpGet]
        public async Task<ActionResult<List<SinhVien>>> GetSinhViens()
        {
            return await _context.SinhViens.ToListAsync();
        }

        // POST: api/SinhVien (Thêm mới)
        [HttpPost]
        public async Task<ActionResult<SinhVien>> CreateSinhVien(SinhVien sv)
        {
            _context.SinhViens.Add(sv);
            await _context.SaveChangesAsync();
            return Ok(sv);
        }

        // DELETE: api/SinhVien/5 (Xóa theo ID)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSinhVien(int id)
        {
            var sv = await _context.SinhViens.FindAsync(id);
            if (sv == null) return NotFound();

            _context.SinhViens.Remove(sv);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //EDIT Thông tin Sinh viên
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSinhVien(int id, SinhVien sv)
        {
            if (id != sv.Id) return BadRequest();

            // Tìm sinh viên cũ
            var existingSv = await _context.SinhViens.FindAsync(id);
            if (existingSv == null) return NotFound();

            // Cập nhật thông tin (EF Core tự theo dõi thay đổi hoặc gán đè)
            // Cách đơn giản nhất là gán giá trị mới sang
            _context.Entry(existingSv).CurrentValues.SetValues(sv);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/SinhVien/export (Xuất Excel)
        [HttpGet("export")]
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

            // Định dạng header
            var headerRow = worksheet.Range("A1:G1");
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.LightBlue;
            headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Data
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

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"DanhSachSinhVien_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}