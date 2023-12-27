using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatTour.Models
{
    public class Tour
    {
        [Key]
        public int ID { get; set; }
        public string TenTour { get; set; }
        public string ThongTin { get; set; }
        public string HinhAnh { get; set; }
        public string GiaTien { get; set; }
        public DateTime NgayDi { get; set; }
        public DateTime NgayVe { get; set; }
        public int SoCho { get; set; }
        public int DiaDiemDuLichID { get; set; }
        public int NguoiDungID { get; set; }
        public ICollection<DatChoTour> DanhSachDatChoTour { get; set; }
        public DiaDiemDuLich DiaDiemDuLich { get; set; }
        public NguoiDung NguoiDung { get; set; }

    }
}
