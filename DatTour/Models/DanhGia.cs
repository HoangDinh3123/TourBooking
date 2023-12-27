using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatTour.Models
{
    public class DanhGia
    {
        public int ID;
        public int NguoiDungID;
        public int DiaDiemDuLichID;
        public double DiemDanhGia;
        public string NoiDung;

        public NguoiDung NguoiDung;
        public DiaDiemDuLich DiaDiemDuLich;
    }
}
