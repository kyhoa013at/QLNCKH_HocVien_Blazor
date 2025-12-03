using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNCKH_HocVien.Migrations
{
    /// <inheritdoc />
    public partial class TaoBangGiaoVien_Chuan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GiaoViens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSoCB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdTinh = table.Column<int>(type: "int", nullable: true),
                    IdXa = table.Column<int>(type: "int", nullable: true),
                    IdDanToc = table.Column<int>(type: "int", nullable: true),
                    IdTonGiao = table.Column<int>(type: "int", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdTrinhDoChuyenMon = table.Column<int>(type: "int", nullable: true),
                    IdTrinhDoLLCT = table.Column<int>(type: "int", nullable: true),
                    IdDonViCongTac = table.Column<int>(type: "int", nullable: true),
                    IdChucVu = table.Column<int>(type: "int", nullable: true),
                    IdCapBac = table.Column<int>(type: "int", nullable: true),
                    HeSoLuong = table.Column<double>(type: "float", nullable: true),
                    IdChucDanh = table.Column<int>(type: "int", nullable: true),
                    IdHocHam = table.Column<int>(type: "int", nullable: true),
                    IdHocVi = table.Column<int>(type: "int", nullable: true),
                    LinhVuc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiaoViens", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiaoViens");
        }
    }
}
