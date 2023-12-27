using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatTour.Models
{
    public class DiaDiemDuLich
    {
        [Key]
        public int ID { get; set; }
        public string TenDiaDiem { get; set; }
        public string DiaChi { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public ICollection<Tour> DanhSachTour{ get; set; }
        public ICollection<DanhGiaDiaDiem> DanhSachDanhGiaDiaDiem{ get; set; }
    }
}
