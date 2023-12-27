using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatTour.Models;
using Microsoft.AspNetCore.Http;
using DatTour.Upload;
using System.IO;

namespace DatTour.Controllers
{
    public class DiaDiemDuLichesController : Controller
    {
        private readonly CSDL_Dat_TourContext _context;

        public DiaDiemDuLichesController(CSDL_Dat_TourContext context)
        {
            _context = context;
        }

        public ActionResult LayTour()
        {
            var cSDL_Dat_TourContext = _context.Tours.Include(t => t.DiaDiemDuLich).Include(t => t.NguoiDung).Take(6);
            ViewData["ListTour"] = cSDL_Dat_TourContext.ToList();
            return View();
        }

        // GET: DiaDiemDuLiches
        public async Task<IActionResult> Index()
        {
            LayTour();
            return View(await _context.DiaDiemDuLichs.ToListAsync());
        }

        // GET: DiaDiemDuLiches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diaDiemDuLich = await _context.DiaDiemDuLichs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (diaDiemDuLich == null)
            {
                return NotFound();
            }

            return View(diaDiemDuLich);
        }

        // GET: DiaDiemDuLiches/Create

        // POST: DiaDiemDuLiches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenDiaDiem,DiaChi,MoTa")] DiaDiemDuLich diaDiemDuLich, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var DiaDiemDuLichCaoNhat = _context.DiaDiemDuLichs.OrderByDescending(t => t.ID).FirstOrDefault();
                int NewId = DiaDiemDuLichCaoNhat.ID + 1;
                diaDiemDuLich.HinhAnh = UploadFile.UploadAnh(NewId, "DiaDiemDuLichs", file);
                _context.Add(diaDiemDuLich);
                await _context.SaveChangesAsync();
                return RedirectToAction("ListDiaDiem", "NguoiDungs");
            }
            return View(diaDiemDuLich);
        }

        // GET: DiaDiemDuLiches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diaDiemDuLich = await _context.DiaDiemDuLichs.FindAsync(id);
            if (diaDiemDuLich == null)
            {
                return NotFound();
            }
            return View(diaDiemDuLich);
        }

        // POST: DiaDiemDuLiches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TenDiaDiem,DiaChi,MoTa, HinhAnh")] DiaDiemDuLich diaDiemDuLich, IFormFile file)
        {
            if (id != diaDiemDuLich.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = UploadFile.UploadAnh(diaDiemDuLich.ID, "DiaDiemDuLichs", file);
                    if (fileName != "")
                    {
                        diaDiemDuLich.HinhAnh = fileName;
                    }
                    _context.Update(diaDiemDuLich);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiaDiemDuLichExists(diaDiemDuLich.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ListDiaDiem", "NguoiDungs");
            }
            return View(diaDiemDuLich);
        }

        // GET: DiaDiemDuLiches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = _context.Tours.Where(t => t.DiaDiemDuLich.ID == id);
            _context.Tours.RemoveRange(tour);
            foreach (var i in tour)
            {
                string directoryPath = "./wwwroot/img/Tours/" + i.ID;
                Directory.Delete(directoryPath, true);
            }

            var danhgia = _context.DanhGiaDiaDiems.Where(t => t.DiaDiemDuLich.ID == id);
            _context.DanhGiaDiaDiems.RemoveRange(danhgia);

            var datchotour = _context.DatChoTours.Where(t => t.Tour.DiaDiemDuLich.ID == id);
            _context.DatChoTours.RemoveRange(datchotour);


            var diaDiemDuLich = await _context.DiaDiemDuLichs.FindAsync(id);
            _context.DiaDiemDuLichs.Remove(diaDiemDuLich);
            _context.SaveChanges();
            string directoryPath1 = "./wwwroot/img/DiaDiemDuLichs/" + diaDiemDuLich.ID;
            Directory.Delete(directoryPath1, true);
            return RedirectToAction("ListDiaDiem", "NguoiDungs");

        }

        private bool DiaDiemDuLichExists(int id)
        {
            return _context.DiaDiemDuLichs.Any(e => e.ID == id);
        }
    }
}
