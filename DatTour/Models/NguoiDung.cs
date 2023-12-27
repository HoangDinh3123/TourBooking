using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace DatTour.Models
{
    public class NguoiDung
    {
        [Key]
        public int ID { get; set; }
        public string TenNguoiDung { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string email { get; set; }
        public int ChucVu { get; set; }
        public string TenTaiKhoan { get; set; }
        public string MatKhau { get; set; }

        public ICollection<TinTuc> DanhSachTinTuc { get; set; }
        public ICollection<Tour> DanhSachTour { get; set; }
    }
}
