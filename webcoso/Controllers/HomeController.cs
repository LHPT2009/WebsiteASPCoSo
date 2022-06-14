using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webcoso.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace webcoso.Controllers
{
    public class HomeController : Controller
    {
        private WebcosoContext db = new WebcosoContext();

        public ActionResult Index()
        {
            var sanPham = db.SanPham.Include(s => s.LoaiSP);
            return View(sanPham.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }
        public ActionResult BlogDetails()
        {
            return View();
        }
        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(string id)
        {
            LienHesController addLienHe = new LienHesController();
            LienHe lienHe = new LienHe();
            if (Request["txtNoiDung"] != null)
            {
                string content = Request["txtNoiDung"].ToString() + " ";
                addLienHe.Create(content, lienHe);
            }
            return RedirectToAction("Contact");
        }

        public ActionResult ShopDetails()
        {
            return View();
        }
        public ActionResult ShopGrid()
        {
            return View();
        }
        public ActionResult ShopCart()
        {
            return View();
        }
    }
}