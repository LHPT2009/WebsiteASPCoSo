using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webcoso.Models;

namespace webcoso.Controllers
{
    public class DanhGiasController : Controller
    {

        private WebcosoContext db = new WebcosoContext();

        // GET: DanhGias
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            ViewBag.MaSP = new SelectList(db.DanhGia, "MaSP", "Ten");
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int numberStar, int id, [Bind(Include ="MaDG,SoSao,MaSP,MaKH,NgayTao")] DanhGia danhGia)
        {
            ModelState.Remove("MaKH");
            if (!ModelState.IsValid)
            {
                ViewBag.MaSP = new SelectList(db.SanPham, "MaSP", "Ten", danhGia.MaSP);
                return View(danhGia);
            }

            //Lay login user id
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            danhGia.MaKH = user.Id;
            danhGia.MaSP = id;
            danhGia.SoSao = numberStar;
            danhGia.NgayTao = DateTime.Now;
            db.DanhGia.Add(danhGia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}