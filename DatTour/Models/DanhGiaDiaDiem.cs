using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatTour.Models
{
    public class DanhGiaDiaDiem
    {
        public int ID { get; set; }
        public int NguoiDungID { get; set; }
        public int DiaDiemDuLichID { get; set; }
        public double DiemDanhGia { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayDanhGia { get; set; }
        public NguoiDung NguoiDung { get; set; }
        public DiaDiemDuLich DiaDiemDuLich { get; set; }
    }
}
