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

namespace webcoso.Controllers
{
    public class NhomSanPhamsController : Controller
    {
        private WebcosoContext db = new WebcosoContext();

        // GET: NhomSanPham
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var all_sach = (from s in db.NhomSanPham select s).OrderBy(m => m.MaNhom);
            int pageSize = 5;
            int pageNum = page ?? 1;
            return View(all_sach.ToPagedList(pageNum, pageSize));
        }

        // GET: NhomSanPham/Details/5
        public ActionResult Details(int? id)
        {
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
            return View();
        }

        // POST: NhomSanPham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNhom,TenNhom")] NhomSanPham nhomSanPham)
        {
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
            NhomSanPham nhomSanPham = db.NhomSanPham.Find(id);
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
    }
}
