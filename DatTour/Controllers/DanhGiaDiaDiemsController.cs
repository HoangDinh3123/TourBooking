using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatTour.Models;
using Microsoft.AspNetCore.Http;

namespace DatTour.Controllers
{
    public class DanhGiaDiaDiemsController : Controller
    {
        private readonly CSDL_Dat_TourContext _context;

        public DanhGiaDiaDiemsController(CSDL_Dat_TourContext context)
        {
            _context = context;
        }

        public ActionResult LayTour()
        {
            var cSDL_Dat_TourContext = _context.Tours.Include(t => t.DiaDiemDuLich).Include(t => t.NguoiDung).Take(6);
            ViewData["ListTour"] = cSDL_Dat_TourContext.ToList();
            return View();
        }

        // GET: DanhGiaDiaDiems
        public async Task<IActionResult> Index(int id)
        {
            var cSDL_Dat_TourContext = _context.DanhGiaDiaDiems.Where(t=>t.DiaDiemDuLich.ID == id);
            ViewBag.idDiaDiem = id;
            LayTour();
            return View(await cSDL_Dat_TourContext.ToListAsync());
        }

        // GET: DanhGiaDiaDiems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhGiaDiaDiem = await _context.DanhGiaDiaDiems
                .Include(d => d.DiaDiemDuLich)
                .Include(d => d.NguoiDung)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (danhGiaDiaDiem == null)
            {
                return NotFound();
            }

            return View(danhGiaDiaDiem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NguoiDungID,DiaDiemDuLichID,DiemDanhGia,NoiDung")] DanhGiaDiaDiem danhGiaDiaDiem)
        {
            LayTour();
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetInt32("ID") == null)
                {
                    return RedirectToAction("Login", "NguoiDungs");
                }  
                else
                {
                    danhGiaDiaDiem.NguoiDungID = (int)HttpContext.Session.GetInt32("ID");
                    danhGiaDiaDiem.NgayDanhGia = DateTime.Now;
                    _context.Add(danhGiaDiaDiem);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", new { id = danhGiaDiaDiem.DiaDiemDuLich.ID });
                }    
            }

            return View();
        }

        // GET: DanhGiaDiaDiems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhGiaDiaDiem = await _context.DanhGiaDiaDiems.FindAsync(id);
            if (danhGiaDiaDiem == null)
            {
                return NotFound();
            }
            ViewData["DiaDiemDuLichID"] = new SelectList(_context.DiaDiemDuLichs, "ID", "ID", danhGiaDiaDiem.DiaDiemDuLichID);
            ViewData["NguoiDungID"] = new SelectList(_context.NguoiDungs, "ID", "ID", danhGiaDiaDiem.NguoiDungID);
            return View(danhGiaDiaDiem);
        }

        // POST: DanhGiaDiaDiems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,NguoiDungID,DiaDiemDuLichID,DiemDanhGia,NoiDung")] DanhGiaDiaDiem danhGiaDiaDiem)
        {
            if (id != danhGiaDiaDiem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhGiaDiaDiem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhGiaDiaDiemExists(danhGiaDiaDiem.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiaDiemDuLichID"] = new SelectList(_context.DiaDiemDuLichs, "ID", "ID", danhGiaDiaDiem.DiaDiemDuLichID);
            ViewData["NguoiDungID"] = new SelectList(_context.NguoiDungs, "ID", "ID", danhGiaDiaDiem.NguoiDungID);
            return View(danhGiaDiaDiem);
        }

        // GET: DanhGiaDiaDiems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhGiaDiaDiem = await _context.DanhGiaDiaDiems
                .Include(d => d.DiaDiemDuLich)
                .Include(d => d.NguoiDung)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (danhGiaDiaDiem == null)
            {
                return NotFound();
            }

            return View(danhGiaDiaDiem);
        }

        private bool DanhGiaDiaDiemExists(int id)
        {
            return _context.DanhGiaDiaDiems.Any(e => e.ID == id);
        }
    }
}
