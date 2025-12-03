using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNCKH_HocVien.Client.Models;
using QLNCKH_HocVien.Data;

namespace QLNCKH_HocVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiaoVienController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GiaoVienController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<GiaoVien>>> GetAll()
        {
            return await _context.GiaoViens.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<GiaoVien>> Create(GiaoVien gv)
        {
            _context.GiaoViens.Add(gv);
            await _context.SaveChangesAsync();
            return Ok(gv);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var gv = await _context.GiaoViens.FindAsync(id);
            if (gv == null) return NotFound();
            _context.GiaoViens.Remove(gv);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}