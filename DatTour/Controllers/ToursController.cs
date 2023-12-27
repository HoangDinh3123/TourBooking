using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatTour.Models;
using DatTour.Upload;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DatTour.Controllers
{
    public class ToursController : Controller
    {
        private readonly CSDL_Dat_TourContext _context;

        public ToursController(CSDL_Dat_TourContext context)
        {
            _context = context;
        }
        //Lay tour du lich roi cho vao layout
        public ActionResult LayTour()
        {
            var cSDL_Dat_TourContext = _context.Tours.Include(t => t.DiaDiemDuLich).Include(t => t.NguoiDung).Take(6);
            ViewData["ListTour"] = cSDL_Dat_TourContext.ToList();
            return View();
        }
        // GET: Tours
        public async Task<IActionResult> Index()
        {
            var cSDL_Dat_TourContext = _context.Tours.Include(t => t.DiaDiemDuLich).Include(t => t.NguoiDung).Take(6);

            var result = _context.DiaDiemDuLichs
                .Join(_context.Tours, diadiem => diadiem.ID, tour => tour.DiaDiemDuLichID, 
                (diadiem, tour) => new
                {
                    DiaDiemID = diadiem.ID,
                    TenDiaDiem = diadiem.TenDiaDiem,
                    SoLuong = _context.Tours.Count(t => t.DiaDiemDuLichID == diadiem.ID),
                    HinhAnh = diadiem.HinhAnh
                })
                .AsEnumerable()
                .GroupBy(x => new { x.DiaDiemID, x.TenDiaDiem })
                .OrderByDescending(x => x.Count())
                .Select(g => new top
                {
                    DiaDiemID = g.Key.DiaDiemID,
                    TenDiaDiem = g.Key.TenDiaDiem,
                    SoLuong = g.Count(),
                    HinhAnh = g.FirstOrDefault().HinhAnh
                })
                .Take(6).ToList();

            var tinTuc = _context.TinTucs.Take(6).ToList();

            ViewData["Bip"] = result;
            ViewData["TinTuc"] = tinTuc;
            LayTour();
            return View("Index", await cSDL_Dat_TourContext.ToListAsync());
        }

        public async Task<IActionResult> ViewTourByLocation(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            var tours = _context.Tours.Include(t => t.DiaDiemDuLich).Include(t => t.NguoiDung).Where(t=>t.DiaDiemDuLichID == id);
            var tenDiaDiem = _context.DiaDiemDuLichs.FirstOrDefault(m => m.ID == id);
            ViewData["tenDiaDiem"] = tenDiaDiem.TenDiaDiem;
            ViewData["id"] = tenDiaDiem.ID;
            //Tính điểm đánh giá
            var listDanhGia = _context.DanhGiaDiaDiems.Where(m => m.DiaDiemDuLich.ID == id);
            float TongDiem = 0;
            foreach (var i in listDanhGia)
            {
                TongDiem += (float)i.DiemDanhGia;
            }
            float DiemTrungBinh = listDanhGia.Count() > 0 ? TongDiem / listDanhGia.Count() : 0;
            ViewBag.DiemDanhGia = DiemTrungBinh;

            if (tours == null)
            {
                return NotFound();
            }

            LayTour();

            return View(await tours.ToListAsync());
        }
        

        //Search
        public ActionResult Search(IFormCollection collection)
        {
            string DiaDiem = collection["selectDiaDiem"].ToString();
            string date1 = collection["date1"].ToString();
            string date2 = collection["date2"].ToString();
            int soNguoi = int.Parse(collection["soNguoi"].ToString());
            ViewBag.DiaDiem = DiaDiem;
            ViewBag.date1 = date1;
            ViewBag.date2 = date2;
            DateTime TuNgay;
            DateTime DenNgay;
            if (DateTime.TryParse(date1, out TuNgay) && DateTime.TryParse(date2, out DenNgay))
            {
                LayTour();
                var tours = _context.Tours.Include(t => t.DiaDiemDuLich).Include(t => t.NguoiDung).Where(t => t.DiaDiemDuLich.TenDiaDiem.Contains(DiaDiem) && t.NgayDi >= TuNgay && t.NgayVe <= DenNgay && t.SoCho >=  soNguoi);
                return View(tours.ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Tours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = _context.Tours
                .Include(t => t.DiaDiemDuLich)
                .Include(t => t.NguoiDung)
                .FirstOrDefault(m => m.ID == id);
            var DatChoTourbyIdTour = _context.DatChoTours.Include(t => t.NguoiDung).Include(t => t.Tour)
                .Where(m => m.Tour.ID == id);
            ViewData["DatChoTourbyIdTour"] = DatChoTourbyIdTour;
            LayTour();
            return View(tour);
        }



        // POST: Tours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenTour,ThongTin,GiaTien,NgayDi,NgayVe,SoCho,DiaDiemDuLichID")] Tour tour, IFormFile file)
        {
            var tourCaoNhat = _context.Tours.OrderByDescending(t => t.ID).FirstOrDefault();
            int NewId = tourCaoNhat.ID + 1;
            tour.HinhAnh = UploadFile.UploadAnh(NewId, "Tours", file);
            if (ModelState.IsValid)
            {
                tour.NguoiDungID = (int)HttpContext.Session.GetInt32("ID");

                _context.Add(tour);
                await _context.SaveChangesAsync();
                return RedirectToAction("ListTour", "NguoiDungs");
            }
            return View();
        }

        // GET: Tours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            var cSDL_Dat_TourContext = _context.Tours.Include(t => t.DiaDiemDuLich).Include(t => t.NguoiDung);
            ViewData["DiaDiemDuLich"] = _context.DiaDiemDuLichs.ToList();
            return View(tour);
        }

        // POST: Tours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TenTour,ThongTin,GiaTien,NgayDi,NgayVe,SoCho,DiaDiemDuLichID, HinhAnh")] Tour tour, IFormFile file)
        {
            if (id != tour.ID)
            {
                return NotFound();
            }
            string fileName = UploadFile.UploadAnh(tour.ID, "Tours", file);
            if (ModelState.IsValid)
            {
                tour.NguoiDungID = (int)HttpContext.Session.GetInt32("ID");
                if (fileName != "")
                {
                    tour.HinhAnh = fileName;
                }
                try
                {
                    _context.Update(tour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(tour.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ListTour", "NguoiDungs");
            }
            ViewData["DiaDiemDuLichID"] = new SelectList(_context.DiaDiemDuLichs, "ID", "ID", tour.DiaDiemDuLichID);
            ViewData["NguoiDungID"] = new SelectList(_context.NguoiDungs, "ID", "ID", tour.NguoiDungID);
            return View(tour);
        }

        // GET: Tours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var datchotour = _context.DatChoTours.Where(t => t.Tour.ID == id);
            _context.DatChoTours.RemoveRange(datchotour);
            var tour = await _context.Tours.FindAsync(id);
            _context.Tours.Remove(tour);
            _context.SaveChanges();
            string directoryPath = "./wwwroot/img/Tours/" + tour.ID;
            Directory.Delete(directoryPath, true);
            return RedirectToAction("ListTour", "NguoiDungs");
        }

        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.ID == id);
        }
    }
}
