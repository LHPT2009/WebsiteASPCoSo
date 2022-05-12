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
using PagedList;

namespace webcoso.Controllers
{
    public class SanPhamsController : Controller
    {
        // GET: SanPhams
        private WebcosoContext db = new WebcosoContext();
        private ApplicationDbContext data = new ApplicationDbContext();

        // GET: SanPhams
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.Keyword = searchString;
            var loaiSP = db.LoaiSP.ToList();
            //if (page == null) page = 1;
            //var all_sanPham = (from s in db.SanPham select s).OrderBy(m => m.MaSP);
            int pageSize = 12;
            int pageNum = page ?? 1;
            SanPhamViewModel sp = new SanPhamViewModel
            {

                LoaiSPs = loaiSP,
                //SanPhams = (PagedList<SanPham>)all_sanPham.ToPagedList(pageNum, pageSize)
                SanPhams = (PagedList<SanPham>)SanPham.getAll(searchString).ToPagedList(pageNum, pageSize)
            };

            //ViewBag.AllProduct = all_sanPham.ToPagedList(pageNum, pageSize);

            return View(sp);
        }

        // GET: SanPhams Admin

        public ActionResult IndexAdmin(int? page, string searchString)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            ViewBag.Keyword = searchString;
            //var all_sanPham = db.SanPham.ToList();
            int pageSize = 5;
            int pageNum = page ?? 1;
            return View(SanPham.getAll(searchString).ToPagedList(pageNum, pageSize));
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            foreach (var i in sanPham.BinhLuan)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.MaKH);
                i.Name = user.Name;
            }
            int pageSize = 5;
            int pageNum = page ?? 1;
            SanPhamDetailModel sp = new SanPhamDetailModel
            {
                SanPham = sanPham,
                BinhLuans = (PagedList<BinhLuan>)sanPham.BinhLuan.ToPagedList(pageNum, pageSize)
            };
            return View(sp);
        }

        // GET: SanPhams/Details/5 Admin
        public ActionResult DetailsAdmin(int? id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id)
        {
            BinhLuansController addbinhluan = new BinhLuansController();
            BinhLuan binhLuan = new BinhLuan();
            string content = Request["txtContent"].ToString() + " ";
            if (content == " ")
            {
                return RedirectToAction("Details");
            }
            addbinhluan.Create(content, id, binhLuan);
            return RedirectToAction("Details");
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            ViewBag.MaLoai = new SelectList(db.LoaiSP, "MaLoai", "TenLoai");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,MaLoai,Ten,MoTa,Gia,SoLuong,DonVi,GiamGia,Hinh")] SanPham sanPham)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (ModelState.IsValid)
            {
                db.SanPham.Add(sanPham);
                db.SaveChanges();
                TempData["AlertMessage"] = "Thêm thành công!";
                return RedirectToAction("Create");
            }

            ViewBag.MaLoai = new SelectList(db.LoaiSP, "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoai = new SelectList(db.LoaiSP, "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,MaLoai,Ten,MoTa,Gia,SoLuong,DonVi,GiamGia,Hinh")] SanPham sanPham)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "Chỉnh sửa thành công!";
                return RedirectToAction("IndexAdmin");
            }
            ViewBag.MaLoai = new SelectList(db.LoaiSP, "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            SanPham sanPham = db.SanPham.Find(id);
            db.SanPham.Remove(sanPham);
            db.SaveChanges();
            TempData["AlertMessage"] = "Xoá thành công!";
            return RedirectToAction("IndexAdmin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
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
    }
}