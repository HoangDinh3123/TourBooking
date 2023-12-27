using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatTour.Models
{
    public class CSDL_Dat_TourContext :DbContext
    {
        public CSDL_Dat_TourContext(DbContextOptions<CSDL_Dat_TourContext> options) : base(options) { }

        public DbSet<DiaDiemDuLich> DiaDiemDuLichs { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<DatChoTour> DatChoTours { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<TinTuc> TinTucs { get; set; }
        public DbSet<DanhGiaDiaDiem> DanhGiaDiaDiems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiaDiemDuLich>().ToTable("DiaDiemDuLich");
            modelBuilder.Entity<Tour>().ToTable("Tour");
            modelBuilder.Entity<DatChoTour>().ToTable("DatChoTour");
            modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");
            modelBuilder.Entity<TinTuc>().ToTable("TinTuc");
            modelBuilder.Entity<DanhGiaDiaDiem>().ToTable("DanhGiaDiaDiem");
        }

    }
}
