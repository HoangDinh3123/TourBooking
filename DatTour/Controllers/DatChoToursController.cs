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
    public class DatChoToursController : Controller
    {
        private readonly CSDL_Dat_TourContext _context;
        

        public DatChoToursController(CSDL_Dat_TourContext context, ToursController tours)
        {
            _context = context;
        }

        // GET: DatChoTours
        public async Task<IActionResult> Index()
        {
            var cSDL_Dat_TourContext = _context.DatChoTours.Include(d => d.NguoiDung).Include(d => d.Tour);
            
            return View(await cSDL_Dat_TourContext.ToListAsync());
        }

        // GET: DatChoTours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datChoTour = await _context.DatChoTours
                .Include(d => d.NguoiDung)
                .Include(d => d.Tour)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (datChoTour == null)
            {
                return NotFound();
            }

            return View(datChoTour);
        }
        public ActionResult LayTour()
        {
            var cSDL_Dat_TourContext = _context.Tours.Include(t => t.DiaDiemDuLich).Include(t => t.NguoiDung).Take(6);
            ViewData["ListTour"] = cSDL_Dat_TourContext.ToList();
            return View();
        }


        // GET: DatChoTours/Create
        [HttpGet]
        public IActionResult Create(int id, int variable)
        {
            if(HttpContext.Session.GetString("User")!= null)
            {
                int socho = variable;
                List<SelectListItem> danhSachSoCho = new List<SelectListItem>();
                for (int i = 1; i <= socho; i++)
                {
                    danhSachSoCho.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                }
                ViewBag.SoCho = danhSachSoCho;
                ViewData["DetailTour"] = _context.Tours.Include(t => t.DiaDiemDuLich).Include(t => t.NguoiDung).FirstOrDefault(t => t.ID == id);
                LayTour();
            }
            else
            {
                return RedirectToAction("Login", "NguoiDungs");
            }
            
            return View();
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TourID,NguoiDungID,SoCho")] DatChoTour datChoTour)
        {
            if (ModelState.IsValid)
            {
                var tour = await _context.Tours.FindAsync(datChoTour.TourID);
                if (tour != null)
                {
                    datChoTour.NgayDat = DateTime.Now;
                    datChoTour.ThanhTien = float.Parse(tour.GiaTien) * datChoTour.SoCho;
                    datChoTour.TrangThai = 0;
                }
                _context.Add(datChoTour);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Tours");
            }
            return View();
        }*/

        public async Task<IActionResult> DatCho(int TourID, int NguoiDungID, int SoCho, string type = "Normal")
        {
            if (TourID != 0)
            {
                DatChoTour datChoTour = new DatChoTour();

                var tour = await _context.Tours.FindAsync(TourID);
                if (tour != null)
                {
                    datChoTour.NguoiDungID = NguoiDungID;
                    datChoTour.TourID = TourID;
                    datChoTour.SoCho = SoCho;
                    datChoTour.NgayDat = DateTime.Now;
                    datChoTour.ThanhTien = float.Parse(tour.GiaTien) * datChoTour.SoCho;
                    datChoTour.TrangThai = 0;
                }
                _context.Add(datChoTour);
                await _context.SaveChangesAsync();
                if(type == "ajax")
                {
                    return Json(datChoTour.ID);
                }    
            }
            return View();
        }

        private IActionResult Json()
        {
            throw new NotImplementedException();
        }

        // GET: DatChoTours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datChoTour = await _context.DatChoTours.FindAsync(id);
            if (datChoTour == null)
            {
                return NotFound();
            }
            ViewData["NguoiDungID"] = new SelectList(_context.NguoiDungs, "ID", "ID", datChoTour.NguoiDungID);
            ViewData["TourID"] = new SelectList(_context.Tours, "ID", "ID", datChoTour.TourID);
            return View(datChoTour);
        }

        // POST: DatChoTours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TourID,NguoiDungID,SoCho,NgayDat,ThanhTien,TrangThai")] DatChoTour datChoTour)
        {
            if (id != datChoTour.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datChoTour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatChoTourExists(datChoTour.ID))
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
            ViewData["NguoiDungID"] = new SelectList(_context.NguoiDungs, "ID", "ID", datChoTour.NguoiDungID);
            ViewData["TourID"] = new SelectList(_context.Tours, "ID", "ID", datChoTour.TourID);
            return View(datChoTour);
        }

        public ActionResult ThanhToan(int? id)
        {
            var idNguoiDung = HttpContext.Session.GetInt32("ID");
            var DatChoTour = _context.DatChoTours.Find(id);
            DatChoTour.TrangThai = 1;
            _context.DatChoTours.Update(DatChoTour);
            _context.SaveChanges();
            return RedirectToAction("TourCuaBan", "NguoiDungs", new { id = idNguoiDung });
        }

        // GET: DatChoTours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var idNguoiDung = HttpContext.Session.GetInt32("ID");
            var datChoTour = _context.DatChoTours.Find(id);
            _context.DatChoTours.Remove(datChoTour);
            _context.SaveChanges();

            return RedirectToAction("TourCuaBan", "NguoiDungs", new{id = idNguoiDung});
        }

        private bool DatChoTourExists(int id)
        {
            return _context.DatChoTours.Any(e => e.ID == id);
        }
    }
}
