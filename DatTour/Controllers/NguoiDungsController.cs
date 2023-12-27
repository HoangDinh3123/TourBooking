using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatTour.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DatTour.Controllers
{
    public class NguoiDungsController : Controller
    {
        private readonly CSDL_Dat_TourContext _context;

        public NguoiDungsController(CSDL_Dat_TourContext context)
        {
            _context = context;
        }



        // GET: NguoiDungs
        public async Task<IActionResult> Index()
        {
            int id = (int)HttpContext.Session.GetInt32("ID");
            var ThongTinNguoiDung = _context.NguoiDungs.FirstOrDefault(t => t.ID == id);
            return View(ThongTinNguoiDung);
        }

        public async Task<IActionResult> TourCuaBan(int id)
        {
            var cSDL_Dat_TourContext = _context.DatChoTours.Include(t => t.Tour).Where(i => i.NguoiDungID == id);
            return View(await cSDL_Dat_TourContext.ToListAsync());
        }

        public async Task<IActionResult> ListTour()
        {
            var cSDL_Dat_TourContext = _context.Tours.Include(t => t.DiaDiemDuLich).Include(t => t.NguoiDung);
            ViewData["DiaDiemDuLich"] = _context.DiaDiemDuLichs.ToList();
            return View(await cSDL_Dat_TourContext.ToListAsync());
        }

        public async Task<IActionResult> ListKhachHang()
        {
            var cSDL_Dat_TourContext = _context.DatChoTours.Include(m => m.NguoiDung).Include(m => m.Tour);
            return View(await cSDL_Dat_TourContext.ToListAsync());
        }

        public async Task<IActionResult> ListDiaDiem()
        {
            var cSDL_Dat_TourContext = _context.DiaDiemDuLichs;
            return View(await cSDL_Dat_TourContext.ToListAsync());
        }

        public async Task<IActionResult> ListTinTuc()
        {
            var cSDL_Dat_TourContext = _context.TinTucs.Include(m=>m.NguoiDung);
            return View(await cSDL_Dat_TourContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IFormCollection collection)
        {
            
            if (ModelState.IsValid)
            {
                string UserName = collection["Username"].ToString();
                string password = collection["password"].ToString();
                var loginUser = _context.NguoiDungs.FirstOrDefault(t => t.TenTaiKhoan == UserName);
                if (loginUser == null)
                {
                    ModelState.AddModelError("", "Dang nhap that bai");
                    return View();
                }
                else
                {
                    SHA256 hashMethod = SHA256.Create();
                    if (md5.MaHoaMd5.VerifyHash(hashMethod, password, loginUser.MatKhau))
                    {
                        HttpContext.Session.SetInt32("ID", loginUser.ID);
                        HttpContext.Session.SetString("User", UserName);
                        HttpContext.Session.SetInt32("ChucVu", loginUser.ChucVu);
                        //luu trang thai user
                        if (loginUser.ChucVu == 1)
                        {
                            return RedirectToAction("Index", "Tours");
                        }
                        else
                            return RedirectToAction("Index", "Tours");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Dang nhap that bai");
                        return View("Login");
                    }
                }
            }
            return View("Login");
        }

        //logout
        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("User");
            HttpContext.Session.Remove("ID");
            HttpContext.Session.Remove("ChucVu");
            return RedirectToAction("Index", "Tours");
        }

        //register
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("TenNguoiDung", "NgaySinh", "DiaChi", "SoDienThoai", "email", "ChucVu", "TenTaiKhoan", "MatKhau")] NguoiDung account)
        {
            if (ModelState.IsValid)
            {
                SHA256 hashMethod = SHA256.Create();
                account.MatKhau = md5.MaHoaMd5.GetHash(hashMethod, account.MatKhau);
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(account);
        }


        // GET: NguoiDungs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }
            return View(nguoiDung);
        }

        // POST: NguoiDungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TenNguoiDung,NgaySinh,DiaChi,SoDienThoai,email,ChucVu,TenTaiKhoan,MatKhau")] NguoiDung nguoiDung)
        {
            if (id != nguoiDung.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nguoiDung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguoiDungExists(nguoiDung.ID))
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
            return View(nguoiDung);
        }

        private bool NguoiDungExists(int id)
        {
            return _context.NguoiDungs.Any(e => e.ID == id);
        }
    }
}
