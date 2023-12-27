using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatTour.Models
{
    public class DatChoTour
    {
        [Key]
        public int ID { get; set; }
        public int TourID { get; set; }
        public int NguoiDungID { get; set; }
        public int SoCho { get; set; }
        public DateTime NgayDat { get; set; }
        public double ThanhTien { get; set; }
        public int TrangThai { get; set; }
        public NguoiDung NguoiDung{ get; set; }
        public Tour Tour { get; set; }
    }
}
