using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webcoso.Models;
using PagedList;
using webcoso.Models.LinQ;

namespace webcoso.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext data = new ApplicationDbContext();
        MyDataDataContext context = new MyDataDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            var kh = data.Users.ToList();
            return View(kh);
        }
        public ActionResult Error401()
        {
            return View();
        }
        public bool AuthAdmin()
        {
            var user = data.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
                return false;
            var userExist = user.Roles.FirstOrDefault(r => r.UserId == user.Id);
            if (userExist == null)
                return false;
            if (userExist.RoleId != "1")
                return false;
            return true;
        }
        public ActionResult GetData()
        {
            var data = context.DonHangs
                .GroupBy(p => p.NgayDat)
                .Select(g => new { Ngay = g.Key, tongtien = g.Sum(n => n.TongTien) }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}