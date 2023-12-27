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
    public class TinTucsController : Controller
    {
        private readonly CSDL_Dat_TourContext _context;

        public TinTucsController(CSDL_Dat_TourContext context)
        {
            _context = context;
        }

        public ActionResult LayTour()
        {
            var cSDL_Dat_TourContext = _context.Tours.Include(t => t.DiaDiemDuLich).Include(t => t.NguoiDung).Take(6);
            ViewData["ListTour"] = cSDL_Dat_TourContext.ToList();
            return View();
        }
        // GET: TinTucs
        public async Task<IActionResult> Index()
        {
            var cSDL_Dat_TourContext = _context.TinTucs.Include(t => t.NguoiDung);
            LayTour();
            return View(await cSDL_Dat_TourContext.ToListAsync());
        }

        // GET: TinTucs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTucs
                .Include(t => t.NguoiDung)
                .FirstOrDefaultAsync(m => m.ID == id);
            LayTour();
            return View(tinTuc);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenTinTuc,NoiDung")] TinTuc tinTuc, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var tintucMoiNhat = _context.TinTucs.OrderByDescending(t => t.ID).FirstOrDefault();
                int NewId = tintucMoiNhat.ID + 1;
                tinTuc.HinhAnh = UploadFile.UploadAnh(NewId, "TinTucs", file);
                tinTuc.NgayDang = DateTime.Now;
                tinTuc.NguoiDungID = (int)HttpContext.Session.GetInt32("ID");
                _context.Add(tinTuc);
                await _context.SaveChangesAsync();
                return RedirectToAction("ListTinTuc", "NguoiDungs");
            }
            return RedirectToAction("ListTinTuc", "NguoiDungs");
        }

        // GET: TinTucs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.TinTucs.FindAsync(id);
            if (tinTuc == null)
            {
                return NotFound();
            }
            return View(tinTuc);
        }

        // POST: TinTucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TenTinTuc,NoiDung,NguoiDungID")] TinTuc tinTuc)
        {
            if (id != tinTuc.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    tinTuc.NgayDang = DateTime.Now;
                    _context.Update(tinTuc);
                    await _context.SaveChangesAsync();
                return RedirectToAction("ListTinTuc", "NguoiDungs");
            }
            return View(tinTuc);
        }

        // GET: TinTucs/Delete/5
        public IActionResult Delete(int? id)
        {
            var tinTuc = _context.TinTucs.Find(id);
            _context.TinTucs.Remove(tinTuc);
            _context.SaveChanges();
            string directoryPath = "./wwwroot/img/TinTucs/" + tinTuc.ID;
            Directory.Delete(directoryPath, true);
            return RedirectToAction("ListTinTuc", "NguoiDungs");
        }

        private bool TinTucExists(int id)
        {
            return _context.TinTucs.Any(e => e.ID == id);
        }
    }
}
