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
using webcoso.Message;

namespace webcoso.Controllers
{
    public class NhomSanPhamsController : Controller
    {
        private WebcosoContext db = new WebcosoContext();
        private ApplicationDbContext data = new ApplicationDbContext();

        // GET: NhomSanPham
        public ActionResult Index(int? page)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (page == null) page = 1;
            var all_sach = (from s in db.NhomSanPham select s).OrderBy(m => m.MaNhom);
            int pageSize = 5;
            int pageNum = page ?? 1;
            return View(all_sach.ToPagedList(pageNum, pageSize));
        }

        // GET: NhomSanPham/Details/5
        public ActionResult Details(int? id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            webcoso.Models.NhomSanPham nhomSanPham = db.NhomSanPham.Find(id);
            if (nhomSanPham == null)
            {
                return HttpNotFound();
            }
            return View(nhomSanPham);
        }

        // GET: NhomSanPham/Create
        public ActionResult Create()
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            return View();
        }

        // POST: NhomSanPham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNhom,TenNhom")] NhomSanPham nhomSanPham)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (ModelState.IsValid)
            {
                db.NhomSanPham.Add(nhomSanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhomSanPham);
        }

        // GET: NhomSanPham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomSanPham nhomSanPham = db.NhomSanPham.Find(id);
            if (nhomSanPham == null)
            {
                return HttpNotFound();
            }
            return View(nhomSanPham);
        }

        // POST: NhomSanPham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNhom,TenNhom")] NhomSanPham nhomSanPham)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (ModelState.IsValid)
            {
                db.Entry(nhomSanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhomSanPham);
        }

        // GET: NhomSanPham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomSanPham nhomSanPham = db.NhomSanPham.Find(id);
            if (nhomSanPham == null)
            {
                return HttpNotFound();
            }
            return View(nhomSanPham);
        }

        // POST: NhomSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            NhomSanPham nhomSanPham = db.NhomSanPham.Find(id);
            if (db.SanPham.Where(p => p.MaNhom == nhomSanPham.MaNhom).FirstOrDefault() != null)
            {
                Notification.set_flash("Không thể xoá nhóm \' " + nhomSanPham.TenNhom + " \'!", "error");
                return RedirectToAction("Index");
            }
            Notification.set_flash("Đã xoá nhóm \' " + nhomSanPham.TenNhom + " \'!", "error");
            db.NhomSanPham.Remove(nhomSanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
