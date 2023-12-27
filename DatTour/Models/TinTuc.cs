using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatTour.Models
{
    public class TinTuc
    {
        [Key]
        public int ID { get; set; }
        public string TenTinTuc { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayDang { get; set; }

        public int NguoiDungID { get; set; }
        public string HinhAnh { get; set; }
        public NguoiDung NguoiDung { get; set; }
    }
}
